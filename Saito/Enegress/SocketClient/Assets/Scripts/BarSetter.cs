using UnityEngine;
using System.Collections;

public class BarSetter : MonoBehaviour {

    GameObject csvReader;
    CSVReader cr;


	// Use this for initialization
	void Start () {
        //csvReader = GameObject.Find("CSVReader");
        /*
        cr = GetComponent<CSVReader>();
        Debug.Log("start" + cr.row + "," + cr.col);

        for (int i = 0; i < cr.row; i++)
        {
            string s="";
            Vector3 pos = Vector3.zero;
            for (int j = 0; j < cr.col; j++)
            {
                s += cr.grid[j,i] + ",";
                
                if (j == 1) pos.x = int.Parse(cr.grid[j, i]);
                if (j == 2) pos.y = int.Parse(cr.grid[j, i]);

                
            }
            //s += ";";
            // プレハブを取得
            GameObject prefab = (GameObject)Resources.Load("Prehabs/BarPrehab");

            // プレハブからインスタンスを生成
            Instantiate(prefab, pos, Quaternion.identity);
            Debug.Log(s);
        }
         * */
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetBars(JSONObject data) {
        //Debug.Log("BarData:"+data);
        //Debug.Log("Count:" + data.Count);
        JSONObject data2 = data[0];
        Debug.Log("BarData:" + data2);
        Debug.Log("Count:" + data2.Count);
        for (int i = 0; i < data2.Count;i++)
        {
            JSONObject data_one = data2[i];

            float lat = float.Parse(data_one.GetField("lat").str);
            float lng = float.Parse(data_one.GetField("lng").str);
            float solar = data_one.GetField("solar").n;

            Debug.Log("lat,lng,solar=("+lat+","+lng+","+solar+")");

            // プレハブを取得
            GameObject prefab = (GameObject)Resources.Load("Prehabs/BarPrehab");

            Vector3 pos = new Vector3(lat,0,lng);
            prefab.transform.localScale = new Vector3(1,solar,1);
            prefab.transform.position = pos;
            // プレハブからインスタンスを生成
            Instantiate(prefab, prefab.transform.position, Quaternion.identity);
            
        }
    }
}
