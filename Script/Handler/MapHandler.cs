using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapHandler : MonoBehaviour {
	
	//public GameObject[] pre;
	//public GameObject[] mpre;
	public GameObject hpUp;
	public GameObject levelUpPre;
	
	//private Dictionary<string,MonsterModel> monsterList = new Dictionary<string, MonsterModel>();
	//private Dictionary<string,GameObject> monstergoList = new Dictionary<string, GameObject>();
	
	
	private bool isLoading = false;
	public GameObject[] playerProfabs;
	private Dictionary<string,PlayerModel> playerList = new Dictionary<string, PlayerModel>();
	private Dictionary<string,GameObject> playergoList = new Dictionary<string, GameObject>();
	
	public void OnMessage(SocketModel model){
		switch (model.command){
		case MapProtocol.ENTER_SRES:
			myEnter (model.message);
			break;
		case MapProtocol.ENTER_BRO:
			playerEnter (model.message);
			break;
		case MapProtocol.MOVE_BRO:
			move (model.message);
			break;
		case MapProtocol.LEAVE_BRO:
			leave (model.message);
			break;
		case MapProtocol.TALK_BRO:
			chat (model.message);
			break;
		case MapProtocol.ATTACK_BRO:
			attack (model.message);
			break;
		case MapProtocol.BE_ATTACE_BRO:
			beAttack (model.message);
			break;
		}
		
	}
	
	
	
	
	public void levelUp(string message){
		StringDTO dto = Coding<StringDTO>.decode (message);
		playerList[dto.value].level += 1;
		playerList[dto.value].exp = 0;
		playergoList[dto.value].BroadcastMessage ("levelUp");
		GameObject go = playergoList[dto.value];
		Instantiate (levelUpPre,new Vector3(go.transform.position.x,go.transform.position.y,go.transform.position.z),new Quaternion(go.transform.rotation.x,go.transform.rotation.y,go.transform.rotation.z,go.transform.rotation.w));
        if(dto.value == GameInfo.myModel.id){
			gameObject.BroadcastMessage ("infoChange");
		}
	}
	
/*
	public void relive(string message){
		StringDTO dto = Coding<StringDTO>.decode (message);
		monsterList[dto.value].hp = monsterList[dto.value].maxHp;
		monstergoList[dto.value].BroadcastMessage("relive");
		monstergoList[dto.value].collider.enabled = true;
	}
*/
/*
	public void monsterDie(string message){
		StringDTO dto = Coding<StringDTO>.decode (message);
		monstergoList[dto.value].BroadcastMessage ("die");
		monstergoList[dto.value].collider.enabled = false;
	}
*/
/*
	void monsterInit(string message){
		MonsterModel[] ms = Coding<MonsterMondel>.decode (message);
		if(isLoading){
			LoadData.getMonsterModels().AddRange(ms);
			
		}
		else{
			foreach(MonsterModel mm in ms)
			{
				createMonster(mm);
			}
		}
	}
*/
	
	public void expUp(string message){
		Debug.Log("expUp");
		IntDTO dto = Coding<IntDTO>.decode (message);
		GameInfo.myModel.exp += dto.value;
		gameObject.BroadcastMessage ("infoChange");
	}
	
	
	public void beAttack(string message){
		BeAtkDTO dto = Coding<BeAtkDTO>.decode (message);
		playerList[dto.id].hp -= dto.value;
		GameObject go = playergoList[dto.id];
		GameObject hp = (GameObject)Instantiate (hpUp,new Vector3(go.transform.position.x,go.transform.position.y+go.transform.collider.bounds.size.y/2,go.transform.position.z),new Quaternion(go.transform.rotation.x,go.transform.rotation.y,go.transform.rotation.z,go.transform.rotation.w));
		//hp.BroadcastMessage ("setValue",dto.value);
		go.BroadcastMessage ("beAttack",dto.id);
	}
	
	private void attack(string message){
		AttackDTO dto = Coding<AttackDTO>.decode (message);
		if(dto.userId == GameInfo.myModel.id){
			GameInfo.PLAYER_STATE = PlayerStateConstans.ATTACK;
		}
		
		GameObject go = playergoList[dto.userId];
		//go.BroadcastMessage ("SetP",new UnityEngine.Vector3((float)dto.point.X,(float)dto.point.Y,(float)dto.point.Z));
		//go.BroadcastMessage ("SetR",new Quaternion((float)dto.Rotation.X,(float)dto.Rotation.Y,(float)dto.Rotation.Z,(float)dto.Rotation.W));
		go.BroadcastMessage ("PlayerState",PlayerStateConstans.ATTACK);
	}
	
	private void skill(string message){
		AttackDTO dto = Coding<AttackDTO>.decode (message);
		if(dto.userId == GameInfo.myModel.id){
			GameInfo.PLAYER_STATE = PlayerStateConstans.SKILL;
		}
		GameObject obj = playergoList[dto.userId];
		//obj.BroadcastMessage ("SetP",new Vector3((float)dto.point.X,(float)dto.point.Y,(float)dto.point.Z));
		//obj.BroadcastMessage ("SetR",new Quaternion((float)dto.Rotation.X,(float)dto.Rotation.Y,(float)dto.Rotation.Z,(float)dto.Rotation.W));
		obj.BroadcastMessage ("PlayerState",PlayerStateConstans.SKILL);
	}
	
	private void chat(string message){
		StringDTO dto = Coding<StringDTO>.decode (message);
		BroadcastMessage ("readChat",dto.value);
	}
	
	private void leave(string message){
		StringDTO dto = Coding<StringDTO>.decode (message);
		if(playerList[dto.value]!=null){
			playerList.Remove (dto.value);
		}
		if(playergoList[dto.value]!=null){
			GameObject go = playergoList[dto.value];
			playergoList.Remove (dto.value);
			Destroy(go);
		}
	}
	
	private void move(string message){
		//Debug.Log("player moving!");
		MoveDTO dto = Coding<MoveDTO>.decode (message);
		if(dto.Id == GameInfo.myModel.id){
			return;
		}
		GameObject go = playergoList[dto.Id];
		go.BroadcastMessage ("setR",new Quaternion((float)dto.Rotation.X,(float)dto.Rotation.Y,(float)dto.Rotation.Z,(float)dto.Rotation.W));
		go.BroadcastMessage ("setP",new Vector3((float)dto.Point.X,(float)dto.Point.Y,(float)dto.Point.Z));
		go.BroadcastMessage ("PlayerState",dto.Dir);
	}
	
	private void playerEnter(string message){
		PlayerModel player = Coding<PlayerModel>.decode(message);
		if(isLoading){
			LoadData.loadingPlayerList.Add (player);
		}else{
			//对此对象进行实例化
			
			createPlayer (player);
			//Debug.Log ("createPlayer();");
		}
	}
	
	private void myEnter(string message){
		PlayerModel[] players = Coding<PlayerModel[]>.decode(message);
		LoadData.loadingPlayerList.AddRange (players);
		isLoading = true;
		BroadcastMessage ("Loading",GameInfo.myModel.map);
	}
	
	void OnLevelWasLoaded(int level){
		if(level < 2){
			return;
		}
		if(level != GameInfo.myModel.map)return;
		//开始进行列表解析生成对象
		
		foreach(PlayerModel model in LoadData.loadingPlayerList){
			createPlayer (model);
		}
		LoadData.loadingPlayerList.Clear ();
		GameInfo.GAME_STATE = GameState.RUN;
	}
	
	private void createPlayer(PlayerModel model){
		playerList.Add (model.id, model);
		Assets.Model.Vector3 point = model.point;
		
		Assets.Model.Vector4 rotation = model.rotation;
		GameObject GO = (GameObject)Instantiate (playerProfabs[model.job],new Vector3((float)point.X,(float)point.Y,(float)point.Z),new Quaternion((float)rotation.X,(float)rotation.Y,(float)rotation.Z,(float)rotation.W));
		GO.name = model.id;
		GO.tag = "Player";
		GO.transform.position =new Vector3(188f,10f,88f);//set position;
		playergoList.Add(model.id,GO);
		GO.BroadcastMessage ("InfoInit",model);
		if(model.id == GameInfo.myModel.id){//make sure our camera wouln't look at other players.
			//GameObject tg= GameObject.Find ("Root");
			BroadcastMessage ("setTarget",GO);
			//BroadcastMessage ("setTarget",tg);
			//BroadcastMessage ("InfoInit",model);
		}
	}
}
