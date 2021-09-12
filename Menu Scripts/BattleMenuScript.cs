using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleMenuScript : MonoBehaviour
{
    GameObject menuContainer;
    void Start()
    {
        menuContainer = GameObject.Find("GameMenuContainer");
        menuContainer.SetActive(false);
    }

    public void OpenMenu()
    {
        menuContainer.SetActive(true);
        Time.timeScale = 0; //pauses the game
    }

    public void ReturnToBattle()
    { 
        menuContainer.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitBattle()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
