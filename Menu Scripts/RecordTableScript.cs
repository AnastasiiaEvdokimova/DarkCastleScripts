using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecordTableScript : MonoBehaviour
{
    GameObject recordPanel;
    Transform recordContainer;
    GameObject header;
    void Start()
    {
        recordPanel = GameObject.Find("Record");
        recordContainer = GameObject.Find("RecordContainer").transform;
        DataTable table = DataBaseClass.GetTable("SELECT records.result_points, records.result_time, bosses.name, heroes.name FROM records JOIN bosses on bosses.id = records.boss_id JOIN heroes on heroes.id = records.hero_id ORDER BY records.result_points DESC, records.result_time LIMIT 10");
       FillRecordTable(table);
    }

    private void FillRecordTable(DataTable table)
    {
        Vector2 transform = recordPanel.transform.position;
        float panelHeight = recordPanel.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < table.Rows.Count; i++)
        {
            GameObject newPanel = Instantiate(recordPanel, recordContainer);
            RectTransform rect = newPanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(recordPanel.GetComponent<RectTransform>().anchoredPosition.x, recordPanel.GetComponent<RectTransform>().anchoredPosition.y - panelHeight * i - panelHeight / 5 * i);
            rect.Find("No").GetComponent<Text>().text = (i + 1).ToString();
            rect.Find("Boss").GetComponent<Text>().text = ConvertNameLanguage(table.Rows[i][2].ToString());
            rect.Find("Hero").GetComponent<Text>().text = ConvertNameLanguage(table.Rows[i][3].ToString());
            float currentTime = System.Convert.ToSingle(table.Rows[i][1]);
            string recordTime = "";
            int currentHours = (int)currentTime / 360;
            int currentMinutes = (int)(currentTime - currentHours * 360) / 60;
            int currentSeconds = (int)currentTime - currentHours * 360 - currentMinutes * 60;
            if (currentHours != 0)
            {
                recordTime = currentHours.ToString("00") + ":";
            }
            recordTime = currentMinutes.ToString("00") + ":" + currentSeconds.ToString("00");
            rect.Find("Time").GetComponent<Text>().text = recordTime;
            float displayedScore = Mathf.Round(System.Convert.ToSingle(table.Rows[i][0]));
            rect.Find("Score").GetComponent<Text>().text = displayedScore.ToString();
          //  newPanel.SetActive(true);
        }
        recordPanel.SetActive(false);
        header = GameObject.Find("HeaderPanel");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void ReloadRecords()
    {
        Dropdown bossList = GameObject.FindWithTag("Boss").GetComponent<Dropdown>();
        string boss = bossList.options[bossList.value].text;
        Dropdown heroList = GameObject.FindWithTag("Player").GetComponent<Dropdown>();
        string hero = heroList.options[heroList.value].text;
        DataTable table = new DataTable();
        if (hero != "Герой" && boss != "Босс")
        {
            boss = ConvertNameLanguage(boss);
            hero = ConvertNameLanguage(hero);
             table = DataBaseClass.GetTable("SELECT records.result_points, records.result_time, bosses.name, heroes.name FROM records JOIN bosses on bosses.id = records.boss_id JOIN heroes on heroes.id = records.hero_id WHERE bosses.name = '" + boss + "' AND heroes.name = '" + hero + "'  ORDER BY records.result_points DESC, records.result_time LIMIT 10");
        }
        else if (hero != "Герой")
        {
            hero = ConvertNameLanguage(hero);
            table = DataBaseClass.GetTable("SELECT records.result_points, records.result_time, bosses.name, heroes.name FROM records JOIN bosses on bosses.id = records.boss_id JOIN heroes on heroes.id = records.hero_id WHERE  heroes.name = '" + hero + "'  ORDER BY records.result_points DESC, records.result_time LIMIT 10");
        }
        else if (boss != "Босс")
        {
            boss = ConvertNameLanguage(boss);
             table = DataBaseClass.GetTable("SELECT records.result_points, records.result_time, bosses.name, heroes.name FROM records JOIN bosses on bosses.id = records.boss_id JOIN heroes on heroes.id = records.hero_id WHERE bosses.name = '" + boss + "'  ORDER BY records.result_points DESC, records.result_time LIMIT 10");
        }
        else
        {
           table = DataBaseClass.GetTable("SELECT records.result_points, records.result_time, bosses.name, heroes.name FROM records JOIN bosses on bosses.id = records.boss_id JOIN heroes on heroes.id = records.hero_id ORDER BY records.result_points DESC, records.result_time LIMIT 10");
        }
        GameObject[] currentPanels = GameObject.FindGameObjectsWithTag("Fire");
        for (int i = 0; i < currentPanels.Length; i++)
        {
            Destroy(currentPanels[i]);
        }
        recordPanel.SetActive(true);
        FillRecordTable(table);
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
            default: return "";
        }
    }
}
