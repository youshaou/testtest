using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

public class Camera3DControl : MonoBehaviour {
	
	private GameObject target;
	public GameObject camera;
	private Transform targetTransform;
	Vector3 targetPos;
	
	private float yOffSet = 0f;
	private float zOffSet = 8f;
	private float distance = -8f;
	//distance = camera.transform.position.z;
	private float maxDistance = 8f;
	private float minDistance = 2f;
	private float MouseWheelSensitivity = 5;
	//private GameObject root;
	//private GameObject head;
	
	
	// Use this for initialization
	void Awake(){
		//root= GameObject.Find ("Root");
	    //head= GameObject.Find ("Root");
	}
	void Start () {
		camera.transform.localPosition = new Vector3(0,6,distance);
	
	}
	
	// Update is called once per frame
	void Update () {
		MouseControl ();
	}
	
	public void setTarget(GameObject t){
		target = t;
		targetTransform = target.transform;//model t's transform.
		
		transform .parent = targetTransform;
		
		//this.transform.parent = targetTransform;
		target.AddComponent<AudioListener>();
		if(target){
			InitPos();
		}
	}
	
	void InitPos(){
		targetPos = targetTransform.position;
		Vector3 v1 = targetTransform.InverseTransformPoint (targetPos);
		Vector3 v2 = new Vector3(v1.x,v1.y + yOffSet,v1.z - zOffSet);
		transform.position = targetTransform.TransformPoint (v2);
		transform.LookAt (targetPos);
	}
	
	private void MouseControl(){
		if(-distance > maxDistance){
			distance = -maxDistance;
		}
		if(-distance < minDistance){
			distance = -minDistance;
		}
		
		if(Input.GetAxis ("Mouse ScrollWheel")!=0){
			//Debug.Log ("Mouse ScrollWheel..."+distance);
			distance += Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
			//camera.transform.position = new Vector3(0,0,distance);
			camera.transform.localPosition = new Vector3(0,6,distance);
			
			transform.LookAt (camera.transform.parent);
		}
	/*
		if(distance == -2f){
			camera.transform.localPosition = new Vector3(0,1,-distance*2);
		}
	*/
	}
	private void idle(){
		if(GameInfo.GAME_STATE != GameState.RUN)return;
		GameInfo.PLAYER_STATE = PlayerStateConstans.IDLE;
		MoveDTO dto = new MoveDTO();
		dto.Dir = PlayerStateConstans.IDLE;
		dto.Rotation = new Assets.Model.Vector4(targetTransform.rotation);
		dto.Point = new Assets.Model.Vector3(targetTransform.position);
		string message = LitJson.JsonMapper.ToJson (dto);
		NetWorkScript.getInstance().sendMessage(Protocol.MAP, GameInfo.myModel.map, MapProtocol.MOVE_CREQ, message);
		target.BroadcastMessage ("PlayerState",PlayerStateConstans.IDLE);
		//BroadcastMessage ("PlayerState",PlayerStateConstans.IDLE);
	}
	
	private void playerMove(int state){
		if(GameInfo.GAME_STATE != GameState.RUN)return;
		if(GameInfo.PLAYER_STATE == PlayerStateConstans.ATTACK || GameInfo.PLAYER_STATE == PlayerStateConstans.SKILL){return;}
		GameInfo.PLAYER_STATE = state;
		MoveDTO dto = new MoveDTO();
		dto.Dir = state;
		dto.Rotation = new Assets.Model.Vector4(targetTransform.rotation);
		dto.Point = new Assets.Model.Vector3(targetTransform.position);
		string message = LitJson.JsonMapper.ToJson (dto);
		NetWorkScript.getInstance().sendMessage(Protocol.MAP, GameInfo.myModel.map, MapProtocol.MOVE_CREQ, message);
		target.BroadcastMessage ("PlayerState",state);
		//BroadcastMessage ("PlayerState",state);
	}
	
	private void attack(int state){
		if(GameInfo.GAME_STATE != GameState.RUN)return;
		if(GameInfo.PLAYER_STATE == PlayerStateConstans.ATTACK || GameInfo.PLAYER_STATE == PlayerStateConstans.SKILL){return;}
		GameInfo.PLAYER_STATE = state;
		AttackDTO dto = new AttackDTO();
		dto.attType = 0;
		dto.point = new Assets.Model.Vector3(targetTransform.position);
		dto.Rotation = new Assets.Model.Vector4(targetTransform.rotation);
		dto.targetId = "";
		string message = LitJson.JsonMapper.ToJson (dto);
		NetWorkScript.getInstance ().sendMessage (Protocol.MAP, GameInfo.myModel.map, MapProtocol.ATTACK_CREQ, message);
	    target.BroadcastMessage ("PlayerState",state);
	}
	private void skill(int state){
		if(GameInfo.GAME_STATE != GameState.RUN)return;
		if(GameInfo.PLAYER_STATE == PlayerStateConstans.ATTACK || GameInfo.PLAYER_STATE == PlayerStateConstans.SKILL){return;}
		AttackDTO dto = new AttackDTO();
		dto.attType = 1;
		dto.point = new Assets.Model.Vector3(targetTransform.position);
		dto.Rotation = new Assets.Model.Vector4(targetTransform.rotation);
		dto.targetId = "";
		string message = LitJson.JsonMapper.ToJson (dto);
		NetWorkScript.getInstance ().sendMessage (Protocol.MAP, GameInfo.myModel.map, MapProtocol.ATTACK_CREQ, message);
	    target.BroadcastMessage ("PlayerState",state);
	}
}
