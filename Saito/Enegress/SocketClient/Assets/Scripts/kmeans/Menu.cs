using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public GameObject[] dataList  ;
	public GameObject[] lineList  ;
	public GameObject[] groupCenter;
	
	public int size=10000;
	public int numberOfGroupe;
	public float range=50.0f;
	
	private Agent agent ;	
	public GameObject data;
	public GameObject center; 
    public GameObject maincam;
    public BarSetter bs;
    public GameObject earth;


    private int counts = 0;
    private float raiselevel = 0.3f;
    private float scalelevel = 0.5f;
    GameObject parentObject;
    private float min_lat = 180.0f;
    private float min_lng = 180.0f;
    private float max_lat = -180.0f;
    private float max_lng = -180.0f;
	
	void Start () {
		//dataList = new GameObject[size];
		//lineList = new GameObject[numberOfGroupe];
		//groupCenter = new GameObject[numberOfGroupe];
        parentObject = GameObject.Find("parentObject");
        agent = GameObject.Find("Agent").GetComponent<Agent>();
        maincam = GameObject.Find("Main Camera");
        bs = GameObject.Find("BarSetter").GetComponent<BarSetter>();
        earth = GameObject.Find("Earth");
        earth.transform.position = Vector3.zero;

	}

    void Init(int dsize) {
        dataList = new GameObject[dsize];
        this.size = dsize;
        lineList = new GameObject[numberOfGroupe];
        groupCenter = new GameObject[numberOfGroupe];
    }

    void SetNumberOfGroup(int num) {
        this.numberOfGroupe = num;
    
    }

    void SetdataList(JSONObject data) {

        Init(data.Count);

        for (int i = 0; i < numberOfGroupe; i++)
        {
            if (lineList[i] != null)
            {
                GameObject.Destroy(lineList[i]);
                lineList[i] = null;
            }
        }

        Vector2 ave = Vector2.zero;
        float[] scales = new float[data.Count];
        for (int i = 0; i < data.Count; i++)
        {
            if (dataList[i] != null)
            {
                GameObject.Destroy(dataList[i]);
                dataList[i] = null;
            }

            JSONObject data_one = data[i];

            //float lat = float.Parse(data_one.GetField("lat").str);
            //float lng = float.Parse(data_one.GetField("lng").str);
            float lat = data_one.GetField("lat").n;
            float lng = -data_one.GetField("lng").n;
            float solar = data_one.GetField("solar").n;
            string username = data_one.GetField("user_name").str;

            Debug.Log("lat,lng,solar=(" + lat + "," + lng + "," + solar + ")");

            if (min_lat > lat)min_lat = lat;
            else if (max_lat < lat) max_lat = lat;

            if (min_lng > lng) min_lng = lng;
            else if (max_lng < lng) max_lng = lng;
            


            ave = ave + new Vector2(lng, lat);

            // プレハブを取得
            GameObject clone = Resources.Load("Prehabs/BarPrehab") as GameObject;

            if (bs.isWorld) {
                float[] v = new float[3] { 0.0f, 20.0f + solar * 0.2f, 0.0f };
                
                scales[i] = solar * 0.2f;
                
                v = RotateXZ(v, lat + 90.0f, lng);
                Vector3 pos = new Vector3(v[0], v[1], v[2]);

                //clone.transform.localScale = new Vector3(1, solar, 1);
                //clone.transform.localScale = new Vector3(1, 1, 1);
                clone.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                clone.transform.position = pos;
                
            }
            else
            {
                Vector3 pos = new Vector3(lat, solar * scalelevel + raiselevel, lng);

                //clone.transform.localScale = new Vector3(1, solar, 1);
                //clone.transform.localScale = new Vector3(1, 1, 1);
                clone.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                clone.transform.position = pos;

            }

            // プレハブからインスタンスを生成
            dataList[i] = Instantiate(clone, clone.transform.position, Quaternion.identity) as GameObject;
            if (username == "izumida")
            {
                dataList[i].transform.position = new Vector3(dataList[i].transform.position.x, dataList[i].transform.position.y/10.0f, dataList[i].transform.position.z);
            }
            else dataList[i].name = counts + dataList[i].name;
            dataList[i].transform.parent = parentObject.transform;

            //if (bs.isWorld) agent.particleCreate();
        }

        if (bs.isWorld) { 
                agent.particleCreateWorld(scales);
                maincam.SendMessage("DetectPos",Vector3.zero);
                maincam.SendMessage("SetDistance", 30.0f);

        }
    }

    void SetCenter() {
        Debug.Log("lat//min,max=(" + min_lat+","+ max_lat+")");
        Debug.Log("lng//min,max=(" + min_lng + "," + max_lng + ")");

        for (int i = 0; i < numberOfGroupe; i++)
        {
            if (lineList[i] != null)
            {
                GameObject.Destroy(lineList[i]);
                lineList[i] = null;
            }
        }

        for (int i = 0; i < numberOfGroupe; i++)
        {
            if (groupCenter[i] != null)
            {
                GameObject.Destroy(groupCenter[i]);
                groupCenter[i] = null;
            }
            float x = Random.Range(min_lat, max_lat);
            float z = Random.Range(min_lng, max_lng);
            groupCenter[i] = (GameObject)Instantiate(center, new Vector3(x, 0, z), Quaternion.identity);
        }
    
    }


    void SetAgentAlive() {
        agent.alive = true; 
    }


    float[] RotateX(float[] array, float angle)
    {
        float rad = angle / 180.0f * Mathf.PI;
        float[] f = new float[3] { array[0], array[1] * Mathf.Cos(rad) + array[2] * Mathf.Sin(rad), array[1] * (-Mathf.Sin(rad)) + array[2] * Mathf.Cos(rad) };
        return f;
    }

    float[] RotateY(float[] array, float angle)
    {
        float rad = angle / 180.0f * Mathf.PI;
        float[] f = new float[3] { array[1], array[0] * Mathf.Cos(rad) + array[2] * Mathf.Sin(rad), array[0] * (-Mathf.Sin(rad)) + array[2] * Mathf.Cos(rad) };
        return f;
    }

    float[] RotateZ(float[] array, float angle)
    {
        float rad = angle / 180.0f * Mathf.PI;
        float[] f = new float[3] { array[2], array[0] * Mathf.Cos(rad) + array[1] * Mathf.Sin(rad), array[0] * (-Mathf.Sin(rad)) + array[1] * Mathf.Cos(rad) };
        return f;
    }

    float[] RotateXZ(float[] array, float lat, float lng)
    {
        return RotateY(RotateX(array, lat), lng);
    }



    /*
	void OnGUI () {
		// バックグラウンド ボックスを作成します。Data
		GUI.Box(new Rect(10,10,120,110), "Menu");
		
		// 1 つ目のボタンを作成します。 押すと、
		if(GUI.Button(new Rect(20,40,100,20), "create data")) {
			for( int i=0;i<numberOfGroupe ;i++){
				if( lineList[i] !=null ){
					GameObject.Destroy(lineList[i]) ;
					lineList[i]=null ;
				}  
			}
			for (int i=0; i<size; i++) {
				if( dataList[i]!=null ){
					GameObject.Destroy(dataList[i]) ;
					dataList[i]=null ;
				}
				float x = Random.Range (-range, range);
				float z = Random.Range (-range, range);
				dataList[i]=(GameObject) Instantiate (data, new Vector3 (x, 0, z), Quaternion.identity);
			}
		}
		
		if(GUI.Button(new Rect(20,70,100,20), "create center")) {
			for( int i=0;i<numberOfGroupe ;i++){
				if( lineList[i] !=null ){
					GameObject.Destroy(lineList[i]) ;
					lineList[i]=null ;
				}  
			}
			for (int i=0; i<numberOfGroupe; i++) {
				if( groupCenter[i]!=null ){
					GameObject.Destroy(groupCenter[i]) ;
					groupCenter[i]=null ;
				}
				float x = Random.Range (-range, range);
				float z = Random.Range (-range, range);
				groupCenter[i]=(GameObject) Instantiate (center, new Vector3 (x, 0, z), Quaternion.identity);
			}
			
		}
		if(GUI.Button(new Rect(20,100,100,20), "start")) {
			agent.alive=true;;
		}
		
	}*/
	
}
