using UnityEngine;
using System.Collections;

public class BarSetter : MonoBehaviour {

    GameObject csvReader;
    CSVReader cr;
    GameObject maincam,CalcTarget,menu,agent;
    GameObject japan;
    Menu m;
    public bool isWorld = false;
    
 

	// Use this for initialization
	void Start () {
        CalcTarget = GameObject.Find("CalcTarget");
        maincam = GameObject.Find("Main Camera");
        menu = GameObject.Find("Menu");
        m = menu.GetComponent<Menu>();
        agent = GameObject.Find("Agent");
        japan = GameObject.Find("JAPAN");
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
            Destroy(m.earth);
            menu.SendMessage("SetCenter");
            menu.SendMessage("SetAgentAlive");
        }
        else {

            Destroy(japan);
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

    public void SetIzumida(JSONObject data)
    {
        JSONObject data2 = data[0];
        Debug.Log("BarData:" + data2);
        Debug.Log("Count:" + data2.Count);
        float[] izumida_ave = new float[3];
        for (int i = 0; i < data2.Count; i++)
        {
            JSONObject data_one = data2[i];

            //float lat = float.Parse(data_one.GetField("lat").str);
            //float lng = float.Parse(data_one.GetField("lng").str);
            float lat = data_one.GetField("lat").n;
            float lng = -data_one.GetField("lng").n;
            float solar = data_one.GetField("solar").n;

            izumida_ave[0] = izumida_ave[0] + lat;
            izumida_ave[1] = izumida_ave[1] + lng;
            izumida_ave[2] = izumida_ave[2] + (solar * m.scalelevel + m.raiselevel);

            // プレハブを取得
            GameObject clone = Resources.Load("Prehabs/izumida_p") as GameObject;

            Vector3 pos = new Vector3(lat, solar * m.scalelevel + m.raiselevel, lng);

            //clone.transform.localScale = new Vector3(1, solar, 1);
            //clone.transform.localScale = new Vector3(1, 1, 1);
            clone.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            clone.transform.position = pos;
            //Material matc = Resources.Load("Matrials");
            //matc.color = Color.red;

            // プレハブからインスタンスを生成
            GameObject obj = Instantiate(clone, clone.transform.position, Quaternion.identity) as GameObject;
            obj.name = "izumida";
            obj.transform.parent = m.parentObject.transform;
            particleCreate(obj);
            //青色に変更
            //Material mat = obj.GetComponent<Material>();
            //mat.color = Color.red;

        }

        izumida_ave[0] = izumida_ave[0] / data2.Count;
        izumida_ave[1] = izumida_ave[1] / data2.Count;
        izumida_ave[2] = izumida_ave[2] / data2.Count;

        maincam.SendMessage("DetectPos", new Vector3(izumida_ave[0], izumida_ave[2], izumida_ave[1]));
        maincam.SendMessage("SetDistance", 8.0f);
    
    }

    public void particleCreate(GameObject obj)
    {

            // プレハブを取得
            GameObject spark = Resources.Load("Prehabs/Lightning Orb") as GameObject;
            float scale = obj.transform.position.y * 0.5f;
            spark.transform.localScale = new Vector3(scale, scale, scale);
            spark.transform.position = obj.transform.position;

            // プレハブからインスタンスを生成
            GameObject go = Instantiate(spark, spark.transform.position, Quaternion.identity) as GameObject;
            ParticleSystem pe = go.GetComponent<ParticleSystem>();
            int width = 5;
            int hegiht = 5;

            int[,] index = new int[width, hegiht];

            pe.Emit(index.Length);
            ParticleSystem.Particle[] particle = new ParticleSystem.Particle[index.Length];

            pe.GetParticles(particle);

            int count = 0;
            for (int z = 0; z < hegiht; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    index[x, z] = count;
                    particle[count].position = go.transform.position;
                    particle[count].size = 1.0f;
                    //particle[count].velocity = new Vector3(scale,scale,scale);
                    count++;
                }
            }

            pe.SetParticles(particle, index.Length);
            pe.maxParticles = (int)(10 * scale);
            pe.startSize = 0.3f + scale;
            pe.startColor = Color.yellow;
            pe.Play();
    }
}
