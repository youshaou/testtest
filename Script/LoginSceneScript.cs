using UnityEngine;
using System.Collections;


public class LoginSceneScript : MonoBehaviour {

	public GameObject RegPanel;
	public UILabel accLabel;
	public UILabel pswLabel;
	
	

	public void RegBtnClick(){
		if(GameInfo.GAME_STATE==GameState.RUN){
			GameInfo.GAME_STATE = GameState.ACC_REG;
			//gameObject.SetActive (false);		
		    RegPanel.SetActive (true);
			
		}else{
			//WindowConstans.windowList.Add (WindowConstans.STATE_ERROR);
			return;
		}
		
	}
	public void LoginBtnClick(){
		
		if(GameInfo.GAME_STATE != GameState.RUN){
			//WindowConstans.windowList.Add(WindowConstans.STATE_ERROR);
			return;
		}
		
		LoginDTO dto=new LoginDTO();
		dto.userName = accLabel.text;
		dto.passWord = pswLabel.text;
		string message=Coding<LoginDTO>.encode(dto);
		if(accLabel.text!=string.Empty && pswLabel.text!=string.Empty){		
			NetWorkScript.getInstance().sendMessage (Protocol.LOGIN,0,LoginProtocol.LOGIN_CREQ,message);
		}else{
			WindowConstans.windowList.Add(WindowConstans.INPUT_ERROR);
		}
	}
	
}