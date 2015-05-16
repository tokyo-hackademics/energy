using UnityEngine;
using System.Collections;

public class BarSetter : MonoBehaviour {

    GameObject csvReader;
    CSVReader cr;
    GameObject maincam,CalcTarget,menu,agent;
    public bool isWorld = false;
    
 

	// Use this for initialization
	void Start () {
        CalcTarget = GameObject.Find("CalcTarget");
        maincam = GameObject.Find("Main Camera");
        menu = GameObject.Find("Menu");
        agent = GameObject.Find("Agent");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetBars(JSONObject data) {
        //Debug.Log("BarData:"+data);
        //Debug.Log("Count:" + data.Count);
        //counts++;
        JSONObject data2 = data[0];
        Debug.Log("BarData:" + data2);
        Debug.Log("Count:" + data2.Count);

        int k;
        if (isWorld) k = 8;
        else k = 4;

        menu.SendMessage("SetNumberOfGroup", k);
        agent.SendMessage("Init",data2.Count);
        menu.SendMessage("SetdataList",data2);
        if (!isWorld)
        {
            menu.SendMessage("SetCenter");
            menu.SendMessage("SetAgentAlive");
        }
        //Vector2 ave = Vector2.zero;

        /*
        for (int i = 0; i < data2.Count;i++)
        {
            JSONObject data_one = data2[i];

            //float lat = float.Parse(data_one.GetField("lat").str);
            //float lng = float.Parse(data_one.GetField("lng").str);
            float lat = data_one.GetField("lat").n;
            float lng = -data_one.GetField("lng").n;
            
            float solar = data_one.GetField("solar").n;

            Debug.Log("lat,lng,solar=("+lat+","+lng+","+solar+")");
            ave = ave + new Vector2(lng,lat);

            // プレハブを取得
            GameObject clone = Resources.Load("Prehabs/BarPrehab") as GameObject;
            
            Vector3 pos = new Vector3(lat,solar * scalelevel + raiselevel,lng);
            
            //clone.transform.localScale = new Vector3(1, solar, 1);
            //clone.transform.localScale = new Vector3(1, 1, 1);
            clone.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            clone.transform.position = pos;
            
            // プレハブからインスタンスを生成
            GameObject  obj = Instantiate(clone, clone.transform.position, Quaternion.identity) as GameObject;
            obj.name = counts + obj.name;
            obj.transform.parent = parentObject.transform;
            
        }
        ave = ave / data2.Count;
        
        maincam.SendMessage("DetectPos", new Vector3(ave.y,10,ave.x));
        maincam.SendMessage("SetDistance", 25.0f);
         * */
    }



}
