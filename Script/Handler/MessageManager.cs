using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour {
	
	private LoginHandler login;
	private UserHandler user;
	private MapHandler map;
	// Use this for initialization
	void Start () {
		login = GetComponent<LoginHandler>();
		user = GetComponent<UserHandler>();
		map = GetComponent<MapHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		
		List<SocketModel> list = NetWorkScript.getInstance().getList();
		for(int i=0;i<8;i++){
			if(list.Count>0){//........................................
				SocketModel model = list[0];
				OnMessage(model);
				list.RemoveAt (0);
			}else{
				break;
			}
		}
		
	}
	
	public void OnMessage(SocketModel model){
		switch(model.type){
		case Protocol.LOGIN:
			login.OnMessage(model);
			break;
		case Protocol.USER:
			user.OnMessage (model);
			break;
		case Protocol.MAP:
			map.OnMessage (model);
			break;
		default:
			WindowConstans.windowList.Add (WindowConstans.SOCKET_TYPE_ERROR);
			break;			
		}
	}
	
}