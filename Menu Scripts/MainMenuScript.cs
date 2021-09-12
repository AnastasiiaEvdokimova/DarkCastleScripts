using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    GameObject[] buttons;
    GameObject nicknameChange;
    private void Start()
    {
        nicknameChange = GameObject.Find("UserNameChange");
        buttons = GameObject.FindGameObjectsWithTag("GameController");
        string userID = DataBaseClass.ExecuteQueryWithAnswer("SELECT id From user WHERE 1");
        if (userID != null)
        {
            SetNicknameChangeDisplay(false);
        }
        else
        {
            SetButtonsDisplay(false);
        }

    }
    public void StoryMode()
    {
        SceneManager.LoadScene("Character Menu");
        GeneralMethods.isStoryMode = true;
        GeneralMethods.userID = DataBaseClass.ExecuteQueryWithAnswer("SELECT id From user WHERE 1");
    }

    public void EndlessMode()
    {
        SceneManager.LoadScene("Character Menu");
        GeneralMethods.isStoryMode = false;
        GeneralMethods.userID = DataBaseClass.ExecuteQueryWithAnswer("SELECT id From user WHERE 1");
    }

    public void RecordsDisplay()
    {
        SceneManager.LoadScene("Records");
    }

    public void InitiateNicknameChange()
    {
        SetButtonsDisplay(false);
        SetNicknameChangeDisplay(true);
        string currentNickname = DataBaseClass.ExecuteQueryWithAnswer("SELECT userNickname From user WHERE 1");
        if (currentNickname != null)
        {
            GameObject.Find("NicknameInput").GetComponent<InputField>().text = currentNickname;
        }
    }

    public void OpenTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TutorialScene");
    }

    private void SetNicknameChangeDisplay(bool isDisplayed)
    {
        nicknameChange.SetActive(isDisplayed);
    }

    private void SetButtonsDisplay(bool isDisplayed)
    {
        for (int i = 0; i<buttons.Length; i++)
        {
            buttons[i].SetActive(isDisplayed);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }


    public void NicknameChange()
    {
        string newNickname = GameObject.Find("NicknameInput").GetComponent<InputField>().text.Replace("'", "`");
        string userID = DataBaseClass.ExecuteQueryWithAnswer("SELECT id From user WHERE 1");
        if (newNickname == "") return;
        if (userID != null)
        {
            DataBaseClass.ExecuteQueryWithoutAnswer("UPDATE user SET userNickname = '" + newNickname + "' Where id =" + userID);
        }
        else
        {
            DataBaseClass.ExecuteQueryWithoutAnswer("INSERT INTO user (deviceID, userNickname) VALUES ('"+ SystemInfo.deviceUniqueIdentifier + "', '"+ newNickname + "')" + userID);
        }
        SetButtonsDisplay(true);
        SetNicknameChangeDisplay(false);
    }

    }
