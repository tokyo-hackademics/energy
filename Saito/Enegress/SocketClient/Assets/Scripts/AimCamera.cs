using UnityEngine;
using System.Collections;
 
public class AimCamera : MonoBehaviour {
	private Transform cam_target;
    public GameObject target;
	Vector3 f0Dir= Vector3.zero;
	Vector3 upVal;
	public float zoomDistance= 8.0F;
	public float theta= 5.000F;
	public float fai= -0.70F;
	public float loc_x= 0.0F;
	public float loc_y= 0.0F;
	public float panWeight= 0.5F;
	
	private Quaternion targetRotation;
	
	private Vector3 original_targetpos;
	private GameObject targetPart;
	private float posdamp = 30f;
	private float rotdamp = 30f;

	//For Get Screen Size
	private int screenwidth,screenheight;

	
	void Start(){
        target = GameObject.Find("target");
		cam_target = GameObject.Find("target").transform;
		upVal = this.transform.position;

		Debug.Log("w:" + Screen.width +",h:"+Screen.height);
		this.screenwidth = Screen.width; this.screenheight = Screen.height;
	}
	

	void Update() {
		if(Input.GetKey("z") && Input.GetMouseButton(0)) {
			f0Dir= new Vector3(Input.GetAxis("Mouse X")*5.0F, -Input.GetAxis("Mouse Y")*5.0F, 0);
		
			if(	Input.GetKey("left alt")) {
				loc_x= -Input.GetAxis("Mouse X")*1;
				loc_y= -Input.GetAxis("Mouse Y")*1;
				f0Dir= Vector3.zero;
				cam_target = GameObject.Find("target").transform;
			}
		} else if( Input.GetMouseButton(1)) {
			zoomDistance= zoomDistance+(-Input.GetAxis("Mouse X")+Input.GetAxis("Mouse Y"))*0.5F;
		} else if( Input.GetMouseButton(2) ) {
			loc_x= -Input.GetAxis("Mouse X")*panWeight;
			loc_y= -Input.GetAxis("Mouse Y")*panWeight;
		} else if(Input.GetKey("right alt")){
			//cam_target = GameObject.Find("ThirdCursor").transform;
		} else {
			f0Dir= Vector3.zero;
			loc_x= 0.0F;
			loc_y= 0.0F;
		}
		
		theta+= Mathf.Deg2Rad*f0Dir.x*1;
		fai+= -Mathf.Deg2Rad*f0Dir.y*1;
		
		upVal.z= zoomDistance*Mathf.Cos(theta)*Mathf.Sin(fai+Mathf.PI/2);
		upVal.x= zoomDistance*Mathf.Sin(theta)*Mathf.Sin(fai+Mathf.PI/2);
		upVal.y= zoomDistance*Mathf.Cos(fai+Mathf.PI/2);
 
		transform.position= upVal;
		cam_target.transform.Translate( Camera.main.transform.up*loc_y+Camera.main.transform.right*(loc_x), Space.World);
		transform.position+= cam_target.position;
		//transform.position = Vector3.Lerp(transform.position,transform.position+cam_target.position,posdamp*Time.deltaTime);
		
		Camera.main.transform.LookAt(cam_target.position);

		Vector3 vectorToTarget = cam_target.position - this.transform.position;
		targetRotation = Quaternion.LookRotation(vectorToTarget);
		Camera.main.transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotdamp*Time.deltaTime);
	}
	
	//
	void DetectPart(string s){
		targetPart = GameObject.Find(s);
		original_targetpos = targetPart.transform.position;
        //this.cam_target.transform.position = Vector3.Lerp(this.cam_target.transform.position,original_targetpos, posdamp * Time.deltaTime);
        this.cam_target.transform.position = original_targetpos;
	}

    void DetectPos(Vector3 pos) {
        original_targetpos = pos;
        Debug.Log("t_pos:"+pos);
        //this.cam_target.transform.position = Vector3.Lerp(this.cam_target.transform.position, original_targetpos, posdamp * Time.deltaTime);
        this.cam_target.transform.position = original_targetpos;
    }
	
	void SetCameraUpperCourse(int i){
		//0.5f + i*0.4f;
		cam_target.transform.position = new Vector3(0f,0.15f + (i-2)*0.3f,0f);
	}

	public int GetScreenWidth(){
		return this.screenwidth;
	}

	public int GetScreenHeight(){
		return this.screenheight;
	}

    public void SetDistance(float distance) {
        this.zoomDistance = distance;
    }


}