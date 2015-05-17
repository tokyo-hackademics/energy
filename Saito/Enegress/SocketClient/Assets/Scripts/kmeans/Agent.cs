using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
	public GameObject line; // ここにLineRendererをアタッチしたGameObjectをInspector上でいれてる
	public float er=0.1f ;
	public bool alive=false ;
	private Menu menu ;
	
	private ArrayList[] groupLists;
	private int[] groupNumber ;
	private Vector3[]  groupCenter0;
	private float totalDistance = 0.0f;

	// Use this for initialization
	void Start () {
        menu = GameObject.Find("Menu").GetComponent<Menu>();  
	}

    void Init(int count)
    {
        groupLists = new ArrayList[menu.numberOfGroupe];
        for (int i = 0; i < menu.numberOfGroupe; i++)
        {
            groupLists[i] = new ArrayList();
        }

        groupCenter0 = new Vector3[menu.numberOfGroupe];
        groupNumber = new int[count];

    }

	
	// Update is called once per frame
	void Update () {
        /*
		for (int i=0; i<menu.numberOfGroupe; i++) {
			groupLists [i] = new ArrayList ();
		}
         * */
		if (!alive) return;
		calcuGroup ();
		findGroupeCenter ();
		if ( totalDistance < er ){
			alive = false;
			particleCreate();
            drawGroup();
		}
	}

    public void particleCreate() {
        
        for (int i = 0; i < menu.dataList.Length; i++) {
            // プレハブを取得
            GameObject spark = Resources.Load("Prehabs/Lightning Orb") as GameObject;
            float scale = menu.dataList[i].transform.position.y;
            spark.transform.localScale = new Vector3(scale, scale, scale);
            spark.transform.position = menu.dataList[i].transform.position;
            
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
            pe.startSize = 0.8f + scale / 2.0f;
            pe.Play();
        } 



    
    }

    public void particleCreateWorld(float[] scales)
    {

        for (int i = 0; i < menu.dataList.Length; i++)
        {
            // プレハブを取得
            GameObject spark = Resources.Load("Prehabs/Lightning Orb") as GameObject;
            if (scales[i] > 3)
            {
                float scale = scales[i] * 0.5f;
                spark.transform.localScale = new Vector3(scale, scale, scale);
                spark.transform.position = menu.dataList[i].transform.position;

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
                pe.Play();


            } 
            
        }




    }



	void calcuGroup(){
		for (int i=0; i<menu.size; i++) {
			float [] dis =new float[menu.numberOfGroupe] ;
			for (int j=0; j<menu.numberOfGroupe; j++) {
				//dis[j]=Vector3.Distance (menu.dataList[i].GetComponent<Rigidbody>().position, menu.groupCenter[j].GetComponent<Rigidbody>().position);
                Debug.Log(i+":"+menu.dataList.Length+":"+menu.size);
                //
                Vector3 mdi = new Vector3(menu.dataList[i].transform.position.x, 0.0f, menu.dataList[i].transform.position.z);
                Vector3 mdj = new Vector3(menu.groupCenter[j].transform.position.x, 0.0f, menu.groupCenter[j].transform.position.z);
                dis[j] = Vector3.Distance(mdi,mdj);
			}
			float min=dis[0] ; 
			int mini=0 ;
			for(int j=1; j<menu.numberOfGroupe;j++){
				if( min > dis[j] ) { min=dis[j]; mini=j; }
			}
			groupNumber[i]=mini;
            //Debug.Log("aaa:"+i + ":" + menu.dataList.Length + ":" + menu.size +":" + mini);
			groupLists[mini].Add (menu.dataList[i]) ;
		}
	}
	
	void findGroupeCenter(){
		totalDistance = 0.0f;
		for (int j=0; j<menu.numberOfGroupe; j++) {
            groupCenter0[j] = menu.groupCenter[j].transform.position;
            menu.groupCenter[j].transform.position = new Vector3(0.0f, 0.0f, 0.0f);
			for(int i=0;i<groupLists[j].Count;i++){
                menu.groupCenter[j].transform.position += ((GameObject)groupLists[j][i]).transform.position;
			}
            if (groupLists[j].Count > 0) menu.groupCenter[j].transform.position /= groupLists[j].Count;
            totalDistance += Vector3.Distance(groupCenter0[j], menu.groupCenter[j].transform.position);
			print (totalDistance) ;
		}
		print (" er="+totalDistance);
	}
	void drawGroup(){
		alive = false;
		for( int i=0 ; i<menu.numberOfGroupe ; i++ ){
            GameObject center = menu.groupCenter[i];
            center.transform.localScale = new Vector3(center.transform.position.y * 2, center.transform.position.y * 2, center.transform.position.y * 2);
			if( groupLists[i].Count==0 ) continue ;
                Vector3 p = ((GameObject)groupLists[i][0]).transform.position;
			    
                LineRenderer[] alr= new LineRenderer[groupLists[i].Count];
                Debug.Log("lrcount:"+groupLists[i].Count);
			    for( int a=0 ; a< groupLists[i].Count ; a++ ){
                    menu.lineList[i] = (GameObject)Instantiate(line, p, Quaternion.identity);
                    alr[a] = line.GetComponent<LineRenderer>();
                    //Debug.Log("i,a = "+i+","+a );
                    alr[a].material = new Material(Shader.Find("Legacy Shaders/Diffuse"));
                    Color c;
                    c = Color.blue;
                    alr[a].SetColors(c, c);
                    alr[a].SetWidth(0.02f, 0.04f * ((GameObject)groupLists[i][a]).transform.position.y);
                    alr[a].SetVertexCount(2);
                    alr[a].SetPosition(0, center.transform.localPosition);
                    alr[a].SetPosition(1, ((GameObject)groupLists[i][a]).transform.position);
                    
			    }
		    }
	    }
    }

/*
public class Comp : IComparer  {
	int IComparer.Compare( object x, object y )  {
		if (((MyDistance)x).d > ((MyDistance)y).d) return 1;
		if (((MyDistance)x).d < ((MyDistance)y).d) return -1;
		return 0;
	}
}

public class MyDistance {
	public int i,j ;
	public float d ;
	
	public MyDistance(int i, int j, float d){
		this.i = i;this.j = j;this.d = d;	
	}
}

*/
