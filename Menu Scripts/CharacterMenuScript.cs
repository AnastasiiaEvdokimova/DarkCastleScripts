using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMenuScript : MonoBehaviour
{
    GameObject[] doors;
    GameObject proceedButton, deleteConfirmation;
    float fixCount;
    string lastDoorName;

    private void Start()
    {
        lastDoorName = null;
        deleteConfirmation = GameObject.Find("SaveDeleteConfirmation");
        proceedButton = GameObject.Find("ContinueButton");
        if (GeneralMethods.isStoryMode)
        {
            doors = GameObject.FindGameObjectsWithTag("Boss");
            for (int i = 1; i<doors.Length; i++)
            {
                if (doors[i].name == "Ringo") //so Ringo always comes first
                {
                    GameObject buf = doors[i];
                    doors[i] = doors[0];
                    doors[0] = buf;
                }
            }
            DataTable oldSave = DataBaseClass.GetTable("SELECT * FROM saveData WHERE isCleared = 0 ORDER BY id");
            if (oldSave.Rows.Count != 0)
            {
                foreach (DataRow record in oldSave.Rows)
                {

                    string heroName = DataBaseClass.ExecuteQueryWithAnswer("Select name from heroes where id = " + record["hero"].ToString());
                    string bossName = DataBaseClass.ExecuteQueryWithAnswer("Select name from bosses where id = " + record["boss"].ToString());
                    GameObject.Find(bossName).GetComponent<DoorFix>().FixCharacter(GameObject.Find(heroName));
                    GameObject.Find(heroName).GetComponent<PlayerIcon_Behaviour>().Lock(true);
                    GameObject.Find(bossName).GetComponent<DoorFix>().CallStart();
                }
                for (int i = 0; i<doors.Length; i++)
                {
                    if (doors[i].GetComponent<DoorFix>().FixedCharacter() == "")
                    {
                        doors[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
                    }
                }
                string nextBossName = DataBaseClass.ExecuteQueryWithAnswer("Select name from bosses where id = " + oldSave.Rows[0]["boss"].ToString());
                lastDoorName = nextBossName;
                GeneralMethods.userID = oldSave.Rows[0]["game"].ToString(); 
            }
            else
            {
                proceedButton.SetActive(false);
                GameObject.Find("SaveDeleteButton").SetActive(false) ;
            }
        }
        else
        {
            GameObject.Find("SaveDeleteButton").SetActive(false);
            proceedButton.SetActive(false);
        }
        HideDeleteConfirmation();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void DeleteSaveFile()
    {
        DataBaseClass.ExecuteQueryWithoutAnswer("DELETE FROM saveData WHERE game =" + GeneralMethods.userID);
       Start();
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].GetComponent<DoorFix>().FixCharacter(null);
        }
        fixCount = 0;
        lastDoorName = null;
    }

    public void ShowDeleteConfirmation()
    {
        deleteConfirmation.SetActive(true);
        proceedButton.GetComponent<Button>().interactable = false;
    }

    public void HideDeleteConfirmation()
    {
        deleteConfirmation.SetActive(false);
        proceedButton.GetComponent<Button>().interactable = true;
    }

    public void Proceed()
    {
       if (GeneralMethods.isStoryMode)
        {
            if (lastDoorName == null)
            {
                int finalBoss = 1;
                string newGameID = DataBaseClass.ExecuteQueryWithAnswer("SELECT MAX(game) FROM saveData");
                string heroID, bossID;
                if (!System.Int32.TryParse(newGameID, out int lastGameID))
                {
                    newGameID = "0";
                }
                else
                {
                    newGameID =  (lastGameID + 1).ToString();
                }
                Debug.Log(newGameID);
                for (int i = 0; i<doors.Length; i++)
                {
                    if (doors[i].name != "Miuna")
                    {
                         heroID = DataBaseClass.ExecuteQueryWithAnswer("Select id from heroes where name = '" + doors[i].GetComponent<DoorFix>().FixedCharacter() + "'");
                         bossID = DataBaseClass.ExecuteQueryWithAnswer("Select id from bosses where name = '" + doors[i].name + "'");
                        DataBaseClass.ExecuteQueryWithoutAnswer("INSERT INTO saveData (game, boss, hero) VALUES ("+ newGameID + ", " + bossID + ", "+ heroID + ")");
                    }
                    else
                    {
                        finalBoss = i;
                    }
                }
                 heroID = DataBaseClass.ExecuteQueryWithAnswer("Select id from heroes where name = '" + doors[finalBoss].GetComponent<DoorFix>().FixedCharacter() + "'");
                 bossID = DataBaseClass.ExecuteQueryWithAnswer("Select id from bosses where name = '" + doors[finalBoss].name + "'");
                DataBaseClass.ExecuteQueryWithoutAnswer("INSERT INTO saveData (game, boss, hero) VALUES (" + newGameID + ", " + bossID + ", " + heroID + ")");
                lastDoorName = "Ringo";
                GeneralMethods.userID = newGameID;  //here userID is used to store gameID
            }
            string playerName = GameObject.Find(lastDoorName).GetComponent<DoorFix>().FixedCharacter();
            DialogueController.sceneName = lastDoorName + " & " + playerName;
            DialogueController.isStart = true;
            SceneManager.LoadScene("DialogueDisplay");
        }
        else
        {
            string playerName = GameObject.Find(lastDoorName).GetComponent<DoorFix>().FixedCharacter();
            SceneManager.LoadScene(lastDoorName  + " & " + playerName);
        }
        Time.timeScale = 1;
        Fire_Behaviour.fireAmount = 0;
        Iceberg_Behaviour.icebergCounter = 0;
    }

    public void CharacterFixed(bool isFixed, string doorname = "")
    {
        if (isFixed)
        {
            if (!GeneralMethods.isStoryMode)
            {
                proceedButton.SetActive(true);
                lastDoorName = doorname;
            }
            else
            {
                fixCount++;
                if (fixCount >= 3)
                {
                    proceedButton.SetActive(true);
                }
            }
        }
        else
        {
            proceedButton.SetActive(false);
            if (GeneralMethods.isStoryMode)
            {
                fixCount--;
            }
        }
    }
}


