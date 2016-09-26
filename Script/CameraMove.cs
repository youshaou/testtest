using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	
	private static int maxX=2000;
	private static int maxZ=1000;
	private int minX;
	private int minZ;

	// Use this for initialization
	void Start () {
		//initPosition();
	
	}
	
	private void initPosition(){
		
		minX = Screen.width /2/100;
		minZ = Screen.height/2/100;
		

		Vector3 position = new Vector3 (minX,0,minZ);
		transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {
		
		moveView();
	}
	
	private void moveView(){
		
		if(Input.mousePosition .x <5&&transform.position.x >minX ){
			transform.Translate (Vector3.left *30*Time.deltaTime );
		}
		
		if(Input.mousePosition .x >(Screen.width-5) &&transform .position.x<maxX ){
			transform.Translate (Vector3.right *30*Time.deltaTime );
		}
		
		if(Input.mousePosition.y<5&&transform.position.z>minZ ){
			transform.Translate (Vector3.back*30*Time.deltaTime );
		}
		
		if(Input.mousePosition .y >(Screen.height-5) &&transform.position.z<maxZ ){
			transform.Translate (Vector3.forward *30*Time.deltaTime );
		}
	}
}
