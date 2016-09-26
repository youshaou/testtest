using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour {
	
	public GameObject WarningPanel;
	private WarningPanelScript script;
	//public GameObject RegPanel;
	
	// Use this for initialization
	void Start () {
		script=WarningPanel.GetComponent<WarningPanelScript>();	
	}
	
	// Update is called once per frame
	void Update () {
		if(WindowConstans.windowList.Count>0){
			GameInfo.LAST_STATE = GameInfo.GAME_STATE;
			
			GameInfo.GAME_STATE = GameState.WINDOW;		
			int type= WindowConstans.windowList[0];
			OnWindow(type);
			WindowConstans.windowList.RemoveAt(0);
		}
	}
	public void OnWindow(int type){	
		switch(type){
		case WindowConstans.INPUT_ERROR:
			script.setMessage ("Input error! Please try again.");
			//WarningPanel.BroadcastMessage("setMessage"+"Input error! Please try again.");
			break;
		case WindowConstans.SOCKET_TYPE_ERROR:
			script.setMessage ("Socket type error!");
			break;
		case WindowConstans.ACC_REG_SUCCEED:
			script.setMessage ("Reg succeed.");
			break;
		case WindowConstans.ACC_REG_FAIL:
			script.setMessage ("Reg fail.");
			break;
		case WindowConstans.LOGIN_FAIL:
			script.setMessage ("Login fail.");
			break;
		case WindowConstans.STATE_ERROR:
			script.setMessage ("State error.");
			break;
		case WindowConstans.JOB_CREATE_ERROR:
			script.setMessage ("Job create error.");
			break;
		case WindowConstans.JOB_CREATE_SUCCEED :
			script.setMessage ("Job create succeed.");
			break;
		default:
			script.setMessage ("Unknown error!");
				//WarningPanel.BroadcastMessage ("setMessage"+"Unknown error!");
			break;
		}
		//GameInfo.LAST_STATE = GameInfo.GAME_STATE;
		//GameInfo.GAME_STATE = GameInfo.LAST_STATE;
		//RegPanel.SetActive (false);
		WarningPanel.SetActive(true);	
	}
}
