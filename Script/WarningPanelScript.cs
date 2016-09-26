using UnityEngine;
using System.Collections;

public class WarningPanelScript : MonoBehaviour {
	
	public UILabel label;
	
	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	
	}
	
	public void setMessage(string value){
		label.text = value;
	}
	
	public void OnClick(){	
		gameObject.SetActive(false);
		GameInfo.GAME_STATE = GameInfo.LAST_STATE;
	}
}
