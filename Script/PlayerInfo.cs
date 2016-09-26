using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
	
	private Camera camera;
	// Use this for initialization
	void Start () {
		camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.transform.rotation != camera.transform.rotation){
			gameObject.transform.rotation = camera.transform.rotation;
		}
	}
}
