using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public Font defaultFont, mariFont;
    public static string sceneName;
    public static bool isStart;
    DataTable dialogues;
    SpriteRenderer currentSprite, currentBackgroundPicture;
    Text speaker, line;
    byte currentLine;
    void Start()
    {
        byte start = (byte) (isStart ? 1 : 0);
        speaker = GameObject.Find("speakerName").GetComponent<Text>();
        line = GameObject.Find("dialogueLine").GetComponent<Text>();
        dialogues = DataBaseClass.GetTable("SELECT * FROM dialogues WHERE stage = '" + sceneName + "' AND isStart = " + start.ToString() + " ORDER BY id");
        if (dialogues.Rows.Count == 0)
        {
            if (isStart)
                SceneManager.LoadScene(sceneName);
            else
                SceneManager.LoadScene("Character Menu");
        }
        else
        {
            currentLine = 0;
           
            DisplayCurrentLine();
        }
    }

    private void DisplayCurrentLine()
    {
        currentSprite = GameObject.Find(dialogues.Rows[currentLine]["speaker"].ToString() + "Sprite").GetComponent<SpriteRenderer>();
        if (dialogues.Rows[currentLine]["speakerPictureName"].ToString() == "default")
        {
            currentSprite = GameObject.Find(dialogues.Rows[currentLine]["speaker"].ToString() + "Sprite").GetComponent<SpriteRenderer>();
        }
        else
        {
            currentSprite = GameObject.Find(dialogues.Rows[currentLine]["speakerPictureName"].ToString()).GetComponent<SpriteRenderer>();
        }
        currentSprite.sortingLayerName = "Characters";
        if (dialogues.Rows[currentLine]["background"].ToString() == "default")
        {
            currentBackgroundPicture = GameObject.Find("DefaultBackgroundPicture").GetComponent<SpriteRenderer>();
        }
        else
        {
            currentBackgroundPicture = GameObject.Find(dialogues.Rows[currentLine]["background"].ToString()).GetComponent<SpriteRenderer>();
        }
        currentBackgroundPicture.sortingLayerName = "Background";
        speaker.text = ConvertNameLanguage(dialogues.Rows[currentLine]["speaker"].ToString());
        if (dialogues.Rows[currentLine]["isThoughts"].ToString() == "0")
        {
            if (dialogues.Rows[currentLine]["speaker"].ToString() == "Mari")
            {
                line.font = mariFont;
                line.fontSize = 12;
            }
            else
            {
                line.font = defaultFont;
                line.fontSize = 14;
            }
            line.fontStyle = FontStyle.Normal;
        }
        else
        {
            line.font = defaultFont;
            line.fontSize = 14;
            line.fontStyle = FontStyle.Italic;
        }
        line.text = dialogues.Rows[currentLine]["line"].ToString();
    }

    private void OnMouseDown()
    {
        currentLine++;
        if (currentLine < dialogues.Rows.Count)
        {
            currentSprite.sortingLayerName = "Default";
            currentBackgroundPicture.sortingLayerName = "Default";
            DisplayCurrentLine();
        }
        else
        {
            if (isStart)
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {

                SceneManager.LoadScene("Character Menu");
            }
        }
    }
    public void Skip()
    {
        if (isStart)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene("Character Menu");
        }
    }

    private string ConvertNameLanguage(string name)
    {
        switch (name)
        {
            case "Миюна": return "Miuna";
            case "Ринго": return "Ringo";
            case "Рио": return "Rio";
            case "Miuna": return "Миюна";
            case "Ringo": return "Ринго";
            case "Rio": return "Рио";
            case "Орион": return "Orion";
            case "Orion": return "Орион";
            case "Lydia": return "Лидия";
            case "Лидия": return "Lydia";
            case "Mari": return "Мари";
            case "Мари": return "Mari";
            case "Hyun": return "Хьюн";
            case "Хьюн": return "Hyun";
            case "Sheryl": return "Шерил";
            case "Шерил": return "Sheryl";
            default: return name;
        }
    }
}
