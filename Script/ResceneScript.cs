using UnityEngine;
using System.Collections;

public class ResceneScript : MonoBehaviour {
	
	public GameObject loading;
	private AsyncOperation async;
	
	// Update is called once per frame
	void Update () {
		if(GameInfo.GAME_STATE == GameState.LOADING ){
			GameInfo.LOAD_PRORESS = async.progress;
		}
	
	}
	public void Loading(int level){
		
		GameInfo.LOAD_PRORESS = 0f;
		gameObject.SetActive (true);
		GameInfo.GAME_STATE = GameState.LOADING;
		StartCoroutine ("load",level);
	}
	IEnumerator load(int level){
		async = Application.LoadLevelAsync (level);
		yield return async;
	}
	void OnLevelWasLoaded(int level){
		GameInfo.GAME_STATE=GameState.RUN;
	}
}
