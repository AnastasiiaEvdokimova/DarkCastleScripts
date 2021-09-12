using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;

public class OnlineRecordKeeper : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();
    // Start is called before the first frame update
    async System.Threading.Tasks.Task StartAsync()
    {
        var values = new Dictionary<string, string>
        {
            { "user_id", "1" },
            { "user_nickname", "Skylin" },
            { "hero_id", "0" },
            { "boss_id", "0" },
            { "result_time", "2321232321" },
            { "result_points", "10" },
        };
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("http://138.197.142.167:5000/upload", content);
        var responseString = await response.Content.ReadAsStringAsync();
        Debug.Log(responseString);
         response = await client.GetAsync("http://138.197.142.167:5000/getresult");
         responseString = await response.Content.ReadAsStringAsync();
        Debug.Log(responseString);
    }

    private void Start()
    {
        StartAsync();
    }
}


