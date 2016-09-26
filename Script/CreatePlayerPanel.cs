using UnityEngine;
using System.Collections;

public class CreatePlayerPanel : MonoBehaviour {
	
	public UILabel nameLable;
	public UILabel jobLable;
	public GameObject panel;
	private int job=0;
	// Use this for initialization
	void Start () {
		panel.SetActive (false);
	
	}
	
	public void SelectJob(int job){
		this.job=job;
		switch (job){
		case 1:
			jobLable.text = "girl";
			break;
		case 2:
			jobLable.text = "boy";
			break;
		}
	}
	
	public void CreateClick(){
		if(GameInfo.GAME_STATE!=GameState.PLAYER_CREATE){
			return;
		}
		if(job==0||nameLable.text==string.Empty){
			//Debug.Log ("create error!");
			WindowConstans.windowList.Add (WindowConstans.JOB_CREATE_ERROR);
			return;
		}
		CreateDTO dto = new CreateDTO ();
		dto.job = job;
		dto.name = nameLable.text;
		string message = Coding<CreateDTO>.encode (dto);
		NetWorkScript.getInstance ().sendMessage (Protocol.USER,0,UserProtocol.CREATE_CREQ,message);
		//GameInfo.GAME_STATE = GameState.RUN;
		panel.SetActive (false);
	}
	
	public void CancelClick(){
		if(GameInfo.GAME_STATE!=GameState.PLAYER_CREATE){
			return;
		}
		GameInfo.LAST_STATE=GameInfo.GAME_STATE;
		GameInfo.GAME_STATE = GameState.RUN;
		gameObject.SetActive (false);
	}
}
