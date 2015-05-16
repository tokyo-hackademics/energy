using UnityEngine;
using System.Collections;

public class JPNReqClick : MonoBehaviour {

    GameObject client;
    BarSetter bs;

	// Use this for initialization
	void Start () {
        client = GameObject.Find("Client");
        bs = GameObject.Find("BarSetter").GetComponent<BarSetter>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickTest()
    {
        Debug.Log("Clicked.");
        bs.isWorld = false;
        client.SendMessage("SendOn", "jpn");
    }

}
