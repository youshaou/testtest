using UnityEngine;
using System.Collections;



public class LoginHandler : MonoBehaviour {
	
	void Start(){}
	void Update(){}
	
	
	public void OnMessage(SocketModel model){
		//Debug.Log ("read message...");
		
		switch (model.command){
		case LoginProtocol.REG_SRES:
			RegResult (model.message);
			break;
		case LoginProtocol.LOGIN_SRES:
			LoginResult (model.message);
			break;
		}
		
	}
	
	
	private void RegResult(string message){
		
		
		Debug.Log (message);
		
		
		BoolDTO dto = Coding<BoolDTO>.decode(message);
		if(dto.value){
			WindowConstans.windowList.Add (WindowConstans.ACC_REG_SUCCEED);
		}else{
			WindowConstans.windowList.Add (WindowConstans.ACC_REG_FAIL);
		}
		
	}
	
	private void LoginResult(string message){
		StringDTO dto = Coding<StringDTO>.decode (message);
		if(dto.value == null || dto.value == string.Empty){
			WindowConstans.windowList.Add (WindowConstans.LOGIN_FAIL);
		}else{
			GameInfo.ACC_ID = dto.value;
			//GameInfo.GAME_STATE = GameState.LOADING;
			BroadcastMessage ("Loading",1);
		}
	}
	
}
