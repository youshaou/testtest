using UnityEngine;
using System.Collections;

public class UICameraManager : MonoBehaviour {
	
	public Camera camera3D;
	public UITextList textList;
	private bool refresh = false;
	public UIScrollBar scrollBar;
	
	void FixedUpdate(){
		if(refresh){
			scrollBar.scrollValue = 1;
			refresh = false;
		}
	}
	
	void setTarget(GameObject obj){
		camera3D.BroadcastMessage ("setTarget",obj);
	}
	
	void Update () {
		//moveCon ();
	}
/*	void moveCon(){
		if(Input.GetAxis ("Vertical")==0f && Input.GetAxis ("Horizontal")==0f){
			//idle
			camera3D.BroadcastMessage ("idle");
		}
		if(Input.GetAxis ("Vertical") >= 1f){
			//forward
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.FORWARD);
		}
		if(Input.GetAxis ("Vertical") <= -1f){
		    //back
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.BACK);
		}
		if(Input.GetAxis ("Horizontal") <= -1f){
		    //left
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.LEFT);
		}
		if(Input.GetAxis ("Horizontal")>= 1f){
			//right
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.RIGHT);
		}

	}
*/
	
	void readChat(string text){
		textList.Add (text);
		refresh = true;
	}
	

	
	void backRelease(){
		if(GameInfo.PLAYER_STATE == PlayerStateConstans.BACK){
			camera3D.BroadcastMessage ("idle");
		}
	}
	void backPress(){
		if(GameInfo.PLAYER_STATE != PlayerStateConstans.BACK){
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.BACK);
		}
	}
	
	void forwardRelease(){
		if(GameInfo.PLAYER_STATE == PlayerStateConstans.FORWARD){
			camera3D.BroadcastMessage ("idle");
		}
	}
	void forwardPress(){
		if(GameInfo.PLAYER_STATE != PlayerStateConstans.FORWARD){
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.FORWARD);
		}
	}
	
	void leftRelease(){
		if(GameInfo.PLAYER_STATE == PlayerStateConstans.LEFT){
			camera3D.BroadcastMessage ("idle");
		}
	}
	void leftPress(){
		if(GameInfo.PLAYER_STATE != PlayerStateConstans.LEFT){
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.LEFT);
		}
	}
	
	void rightRelease(){
		if(GameInfo.PLAYER_STATE == PlayerStateConstans.RIGHT){
			camera3D.BroadcastMessage ("idle");
		}
	}
	void rightPress(){
		if(GameInfo.PLAYER_STATE != PlayerStateConstans.RIGHT){
			camera3D.BroadcastMessage ("playerMove",PlayerStateConstans.RIGHT);
		}
	}
	void attackClick(){
		if(GameInfo.PLAYER_STATE != PlayerStateConstans.ATTACK){
			camera3D.BroadcastMessage ("attack",PlayerStateConstans.ATTACK);
		}
	}
	void skillClick(){
		if(GameInfo.PLAYER_STATE != PlayerStateConstans.SKILL){
			camera3D.BroadcastMessage ("skill",PlayerStateConstans.SKILL);
		}
	}

}
