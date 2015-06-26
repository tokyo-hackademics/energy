using UnityEngine;
using System.Collections;

public class NonAimCamera : MonoBehaviour {

    GameObject target, targetPart, InputManager;
    Transform cam_target;

    Vector3 upval = Vector3.zero;

	// Update is called once per frame
	void Update () {
	
	}

    void Start()
    {
        target = GameObject.Find("target");
        cam_target = GameObject.Find("target").transform;
        InputManager = GameObject.Find("InputManager");
        this.DetectPart("JAPAN");
        Debug.Log("w:" + Screen.width + ",h:" + Screen.height);
        //this.screenwidth = Screen.width; this.screenheight = Screen.height;
    }

    //

    void DetectPos(Vector3 v) { 
    
    }

    void DetectPart(string s)
    {
        targetPart = GameObject.Find(s);
        //original_targetpos = targetPart.transform.position;
        //this.cam_target.transform.position = Vector3.Lerp(this.cam_target.transform.position,original_targetpos, posdamp * Time.deltaTime);
        target.transform.position = targetPart.transform.position;
        target.transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
        target.transform.Rotate(35.0f, 0.0f, 0.0f);
        Vector3[] vec = new Vector3[2];
        vec[0] = target.transform.position;
        vec[1] = target.transform.rotation.eulerAngles;
        InputManager.SendMessage("SetFirstPosAndRot",vec);
    }


    public void SetDistance(float distance)
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, distance);
    }

}
