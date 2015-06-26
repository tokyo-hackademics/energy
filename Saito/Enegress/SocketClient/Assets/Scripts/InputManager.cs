using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputManager : MonoBehaviour {

    Touch touch;
    bool isSingleTouching = false;
    float[] moving_vector = {0.0f, 0.0f};
    GameObject MainCam, target;
    float xDelta, yDelta;
    float[] fsend = { 0.0f,0.0f};

    private Vector3 firstPos;
    private Quaternion firstRot;

	// Use this for initialization
	void Start () {
        MainCam = GameObject.Find("MainCamera");
        target = GameObject.Find("target");
        

        firstPos = target.transform.position;
        firstRot = target.transform.rotation;



#if UNITY_ANDROID
        Debug.Log("Unity Android");
#elif UNITY_IPHONE
        Debug.Log("Unity iphone");
#else
        Debug.Log("Any other platform");
#endif
    }
	
	// Update is called once per frame
	void Update ()
    {

#if UNITY_ANDROID || UNITY_IPHONE
        touchEvent();
#else
    //Debug.Log("Any other platform");
#endif



    }

    protected void touchEvent() {

        int touchCount = Input.touches.Count(t => t.phase != TouchPhase.Ended && t.phase != TouchPhase.Canceled);
        if (touchCount == 1)
        {

            Touch t = Input.touches.First();
            MainCam.SendMessage("TouchChange", true);
            if (t.tapCount >= 2) {
                Debug.Log("tap:"+t.tapCount);
                target.transform.position = firstPos;
                target.transform.rotation = firstRot;
                
            }

            

            switch (t.phase)
            {
                case TouchPhase.Began:
                    //移動量
                    xDelta = t.deltaPosition.x;
                    yDelta = t.deltaPosition.y;
                    fsend[0] = xDelta;
                    fsend[1] = yDelta;

                    //左右回転
                    target.transform.Rotate(0, xDelta, 0, Space.World);
                    //上下回転
                    //this.transform.position += new Vector3(0, -yDelta, 0);
                    target.transform.Rotate(-yDelta, 0, 0, Space.World);

                    MainCam.SendMessage("RotateCamera", fsend);
                    Debug.Log("Send:" + fsend[0] + "," + fsend[1]);
                    break;

                case TouchPhase.Moved:

                    //移動量
                    xDelta = t.deltaPosition.x;
                    yDelta = t.deltaPosition.y;
                    fsend[0] = xDelta;
                    fsend[1] = yDelta;

                    //左右回転
                    target.transform.Rotate(0, xDelta, 0, Space.World);
                    //上下回転
                    //this.transform.position += new Vector3(0, -yDelta, 0);
                    target.transform.Rotate(-yDelta, 0, 0, Space.World);

                    MainCam.SendMessage("RotateCamera", fsend);
                    Debug.Log("Send:"+fsend[0] + "," +fsend[1]);

                    break; 

                case TouchPhase.Ended:
                    MainCam.SendMessage("TouchChange", false);
                    break;
            }


        }
        else if (touchCount == 2)
        {
            Touch t = Input.touches[0];
            MainCam.SendMessage("TouchChange", true);
            if (t.tapCount >= 2)
            {
                Debug.Log("tap:" + t.tapCount);
                target.transform.position = firstPos;
                target.transform.rotation = firstRot;
            }

            switch (t.phase)
            {
                case TouchPhase.Moved:

                    Debug.Log("move");

                    //移動量
                    float xDelta = t.deltaPosition.x;
                    float yDelta = t.deltaPosition.y;

                    //左右回転
                    target.transform.position += new Vector3(-xDelta / 10, 0, 0);
                    //上下回転
                    target.transform.position += new Vector3(0, -yDelta / 10, 0);

                    break;
            }
        }
        else {
            MainCam.SendMessage("TouchChange", false);
            
        }


    }

    void SetFirstPosAndRot(Vector3[] pos_rot){

        this.firstPos = pos_rot[0];
        this.firstRot = Quaternion.Euler(pos_rot[1]);

    }
}
