using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Observer : MonoBehaviour
{
    bool recordsUpdate = false;
    bool gameWon = false;
    Text timerDisplay, scoreDisplay;
    float gameStartTime;
    float lastBossHit, fatiqueTime;
    float currentBossAttackCooldown;
    private float endlessModeScore;
    Boss_Abilities bossAbilities;
    GameObject endGameMenu;
    float bossFatiqueRate, bossAcceleration;
    void Start()
    {
        endGameMenu = GameObject.Find("GameEndContainer");
        endGameMenu.SetActive(false);
        timerDisplay = GameObject.Find("timerDisplay").GetComponent<Text>();
        if (GeneralMethods.isStoryMode)
        {
            GameObject.Find("scoreDisplay").SetActive(false);
            GameObject.Find("scoreMark").SetActive(false);
        }
        else
        {
            scoreDisplay = GameObject.Find("scoreDisplay").GetComponent<Text>();
        }
        GameObject boss = GameObject.FindWithTag("Boss");
        bossAbilities = boss.GetComponent<Boss_Abilities>();
        lastBossHit = Time.time;
        currentBossAttackCooldown = 2f;
        bossAcceleration = 0.1f;
        bossFatiqueRate = 0.2f;
        bossAbilities.attackCooldown = currentBossAttackCooldown;
        gameStartTime = Time.time;
        endlessModeScore = 0;
        fatiqueTime = 30;
    }

    private void TimerManagment()
    {
        timerDisplay.text = "";
        float currentTime = Time.time - gameStartTime;
        int currentHours = (int)currentTime / 360;
        int currentMinutes = (int)(currentTime - currentHours * 360) / 60;
        int currentSeconds = (int)currentTime - currentHours * 360 - currentMinutes * 60;
        if (currentHours != 0)
        {
            timerDisplay.text = currentHours.ToString("00") + ":";
        }
        timerDisplay.text += currentMinutes.ToString("00") + ":" + currentSeconds.ToString("00");
    }
    
    public void BossHit()
    {
        lastBossHit = Time.time;
        if ((!GeneralMethods.isStoryMode && currentBossAttackCooldown >0.5)||(currentBossAttackCooldown > 0.9))
        {
            currentBossAttackCooldown -= bossAcceleration;
        }
        bossAbilities.attackCooldown = currentBossAttackCooldown;
        fatiqueTime = 30;
    }

    public void GameWon()
    {
        gameWon = true;
        endGameMenu.SetActive(true);
        GameObject.Find("GameEndStatusText").GetComponent<Text>().text = "Победа";
        Time.timeScale = 0;
        GameObject.Find("MenuButton").GetComponent<Button>().enabled = false;
    }

    public void GameLost()
    {
        Time.timeScale = 0;
        endGameMenu.SetActive(true);
        if (!GeneralMethods.isStoryMode)
        {
            GameObject.Find("GameEndStatusText").GetComponent<Text>().text = "Игра закончена";
        }
        else
        {
            GameObject.Find("GameEndStatusText").GetComponent<Text>().text = "Поражение";
            gameWon = false;
        }

        GameObject.Find("MenuButton").GetComponent<Button>().enabled = false;
    }

    public void ExitBattleStage()
    {
        Time.timeScale = 1; 
        if (GeneralMethods.isStoryMode)
        {
            if (gameWon)
            {
                string bossName = GameObject.FindWithTag("Boss").name.Replace("Boss_", "");
                string bossID = DataBaseClass.ExecuteQueryWithAnswer("Select id from bosses where name = '" + bossName + "'");
                DataBaseClass.ExecuteQueryWithoutAnswer("UPDATE saveData SET isCleared = 1 WHERE (boss = " + bossID + " AND game = " + GeneralMethods.userID + ")"); //here userID is used to store gameID
                DialogueController.isStart = false;
                SceneManager.LoadScene("DialogueDisplay");
            }
            else
            {
                Debug.Log("Exit");
                SceneManager.LoadScene("Character Menu");
            }
        }
        else
        {
            if (!recordsUpdate)
            {
                string heroName = GameObject.FindWithTag("Player").name.Replace("Player_", "");
                string bossName = GameObject.FindWithTag("Boss").name.Replace("Boss_", "");
                string heroID = DataBaseClass.ExecuteQueryWithAnswer("Select id from heroes where name = '" + heroName + "'");
                string bossID = DataBaseClass.ExecuteQueryWithAnswer("Select id from bosses where name = '" + bossName + "'");
                string time = (Time.time - gameStartTime).ToString();
                DataBaseClass.ExecuteQueryWithoutAnswer("INSERT INTO records (user_id, hero_id, boss_id, result_time, result_points) VALUES (" + GeneralMethods.userID + ", " + heroID + ", " + bossID + ", " + time.Replace(",", ".") + ", " + endlessModeScore.ToString().Replace(",", ".") + ")");
                DataBaseClass.ExecuteQueryWithoutAnswer("DELETE FROM records WHERE (hero_id = " + heroID + " AND boss_id = " + bossID + " AND id NOT IN (SELECT id FROM records WHERE (hero_id = " + heroID + " AND boss_id = " + bossID + ") ORDER BY result_points DESC, result_time LIMIT 10 ))");
                recordsUpdate = true;
            }

            SceneManager.LoadScene("Character Menu");
        }
    }

    private void FatiqueSystem()
    {
     if (Time.time - lastBossHit >= fatiqueTime)
        {
            bossAbilities.attackCooldown += bossFatiqueRate;
            fatiqueTime *= 2;
        }
    }

    public void ScoreUp(float score) {
        if ((int)(endlessModeScore) < (int)(endlessModeScore + score))
        {
            BossHit();
        }
        endlessModeScore += score;
        scoreDisplay.text = Mathf.RoundToInt(endlessModeScore).ToString();
    }
    
    void Update()
    {
        TimerManagment();
        FatiqueSystem();
    }
}
