using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;

public class TutorialObserver : MonoBehaviour
{
    GameObject dialogueBox, finalMenu;
    DataTable tutorialLines;
    Text dialogueLine, speakerName;
    bool isDialogueDisplayed;
    int currentLine;
    // Start is called before the first frame update
    void Start()
    {
        finalMenu = GameObject.Find("GameEndContainer");
        finalMenu.SetActive(false);
        dialogueBox = GameObject.Find("DialogueContainer");
        dialogueLine = GameObject.Find("dialogueLine").GetComponent<Text>();
        speakerName = GameObject.Find("speakerName").GetComponent<Text>();
        tutorialLines = DataBaseClass.GetTable("SELECT line, speaker FROM dialogues WHERE stage = 'Tutorial' ORDER BY id");
        isDialogueDisplayed = true;
        currentLine = 0;
        dialogueLine.text = tutorialLines.Rows[currentLine][0].ToString();
        speakerName.text = tutorialLines.Rows[currentLine][1].ToString();
        gameObject.GetComponent<Default_Movement_Control>().enabled = false;
    }

    public void DisplayDialogue(bool isDisplayed)
    {
        isDialogueDisplayed = isDisplayed;
        if (!isDialogueDisplayed)
        {
            gameObject.GetComponent<TutorialMovementControl>().Activate(true);
            currentLine++;
            DisplayLine();
            dialogueBox.SetActive(false);
        }
        else
        {
            dialogueBox.SetActive(true);
        }
    }

    private void DisplayLine()
    {
        dialogueLine.text = tutorialLines.Rows[currentLine][0].ToString();
        speakerName.text = tutorialLines.Rows[currentLine][1].ToString();
    }

    public void ExitTutorial()
    {
        SceneManager.LoadScene("Main Menu");
    }

        private void OnMouseDown()
    {
        if (currentLine == 3 || currentLine == 8 || currentLine == 14)
        {
            DisplayDialogue(false);
        }
        if (currentLine == tutorialLines.Rows.Count-1)
        {
            finalMenu.SetActive(true);
            GameObject.Find("GameEndStatusText").GetComponent<Text>().text = "Обучение пройдено";
        }
        else
        if (isDialogueDisplayed)
        {
            currentLine++;
            DisplayLine();
        }
    }
}
