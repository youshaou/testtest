using UnityEngine;
using System.Collections;

public class GameStateTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ShowGameState(){
		Debug.Log ("GAME_STATE: "+GameInfo.GAME_STATE+"    LAST_STATE(before WINDOW): "+GameInfo.LAST_STATE);
	}
}
