using UnityEngine;
using System.Collections;



public class RegPanelScript : MonoBehaviour {

	public UILabel accountLabel;
	public UILabel passwordLabel;
	
	
	void Start () {
		gameObject.SetActive (false);
	
	}
	
	public void CreatBtnClick(){
		if(GameInfo.GAME_STATE != GameState.ACC_REG){
			
			WindowConstans.windowList.Add (WindowConstans.STATE_ERROR);
			
			return;
		}
		
		LoginDTO dto=new LoginDTO();
		dto.userName = accountLabel.text;
		dto.passWord = passwordLabel.text;
		string message=Coding<LoginDTO>.encode(dto);
		
		if(accountLabel.text!=string.Empty && passwordLabel.text!=string.Empty){	
			NetWorkScript.getInstance().sendMessage (Protocol.LOGIN,0,LoginProtocol.REG_CREQ,message);
		}else{
			WindowConstans.windowList.Add (WindowConstans.INPUT_ERROR);
		}
		
		//GameInfo.GAME_STATE=GameState.RUN;
		//GameInfo.LAST_STATE = GameInfo.GAME_STATE;
		
		//Debug.Log("gameState = "+GameInfo.GAME_STATE);//,,,,,,,,,,,
		//gameObject.SetActive (false);
	}
	public void CancelBtnClick(){
		if(GameInfo.GAME_STATE != GameState.ACC_REG){
			WindowConstans.windowList.Add (WindowConstans.STATE_ERROR);	
			return;
		}
		GameInfo.LAST_STATE=GameInfo.GAME_STATE;
		GameInfo.GAME_STATE=GameState.RUN;
		gameObject.SetActive (false);
	}
}