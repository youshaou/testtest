using UnityEngine;
using System.Collections;


public class UserHandler : MonoBehaviour
{
	public void OnMessage(SocketModel model){
		//Debug.Log (model.message);
		switch(model.command){
		case UserProtocol.LIST_SRES:
			list (model.message);
			break;
		case UserProtocol.CREATE_SRES:
			create (model.message);
			break;
		case UserProtocol.SELECT_SRES:
			selectPlayer (model.message);
			break;
		}
	}
	
	private void selectPlayer(string message){
		PlayerModel dto = Coding<PlayerModel>.decode(message);
		GameInfo.myModel = dto;
		EnterMapDTO edto = new EnterMapDTO();
		edto.map = dto.map;
		edto.point = dto.point;
		edto.rotation = dto.rotation;
		string m = Coding<EnterMapDTO>.encode (edto);
		
		NetWorkScript.getInstance().sendMessage(Protocol.MAP, dto.map, MapProtocol.ENTER_CREQ,m);
	}
	
	private void create(string message){
		if(message == null){
			WindowConstans.windowList.Add (WindowConstans.JOB_CREATE_ERROR);
		}else{
			//WindowConstans.windowList.Add (WindowConstans.JOB_CREATE_SUCCEED);
			string m = Coding<StringDTO>.encode (new StringDTO(GameInfo.ACC_ID));
		    NetWorkScript.getInstance ().sendMessage (Protocol.USER,0,UserProtocol.LIST_CREQ,m);
		}
	}
	
	private void list(string message){
		BroadcastMessage ("cleanButtons");
		if(message == null){
			for(int i=0; i<2; i++){
				CreateButtonModel model = new CreateButtonModel();
				model.index = i;
				PlayerModel dto = new PlayerModel();
				dto.job = 0;
				model.player = dto;
				BroadcastMessage ("CreatePlayerButton",model);
			}
			return;
		}
		PlayerModel[] dtos = Coding<PlayerModel[]>.decode (message);
		for(int i=0; i<2; i++){
			CreateButtonModel model = new CreateButtonModel();
			model.index = i;
			if(i > dtos.Length-1){
				PlayerModel dto = new PlayerModel();
				dto.job = 0;
				model.player = dto;
			}else{
				model.player = dtos[i];
			}
			BroadcastMessage ("CreatePlayerButton",model);
		}
		GameInfo.GAME_STATE = GameState.RUN;
	}
}


