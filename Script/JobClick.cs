using UnityEngine;
using System.Collections;

public class JobClick : MonoBehaviour {
	public int job;
	public GameObject target;
	void OnClick(){
		if(GameInfo.GAME_STATE == GameState.PLAYER_CREATE ){
		    target.BroadcastMessage ("SelectJob",job);
		}else {
			return;
		}
	}
}
