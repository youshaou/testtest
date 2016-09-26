using UnityEngine;
using System.Collections;

public class LoadingPanelScript : MonoBehaviour {
	
	public UISprite sprite;
	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		sprite.fillAmount=0f;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameInfo.GAME_STATE==GameState.LOADING){
			sprite.fillAmount = GameInfo.LOAD_PRORESS;
		}
	
	}
}
