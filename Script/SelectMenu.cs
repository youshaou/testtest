using UnityEngine;
using System.Collections;

public class SelectMenu : MonoBehaviour {
	public GameObject CreatePanel;
	
	public GameObject panel;
	public UIAtlas atlas;
	public UITexture tex;
	public Texture[] texs;
	
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UIButton startButton;
	public UIButton deleteButton;
	
	// Use this for initialization
	void Start () {
		startButton.collider.enabled = false;
		deleteButton.collider.enabled = false;
		string m = Coding<StringDTO>.encode (new StringDTO(GameInfo.ACC_ID));
		NetWorkScript.getInstance ().sendMessage (Protocol.USER,0,UserProtocol.LIST_CREQ,m);
		/*
		PlayerModel model = new PlayerModel();
		CreateButtonModel cbm = new CreateButtonModel();
		cbm.index = 0;
		cbm.player = model;
		CreatePlayerButton (cbm);
		
		model = new PlayerModel();
		cbm = new CreateButtonModel();
		cbm.index = 1;
		cbm.player = model;
		CreatePlayerButton (cbm);
	*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CreatePlayerButton(CreateButtonModel model){
		string name = "job";
		int depth = NGUITools.CalculateNextDepth (panel);
        GameObject go = NGUITools.AddChild (panel);
		go.tag="PlayerSelectButton";
		go.transform.localPosition  = new Vector3(-180+model.index*360,20,0);
		
		UISprite sprite = NGUITools.AddWidget<UISprite>(go);
		sprite.depth = depth;
		sprite.atlas=atlas;
		sprite.name="Background";
		//sprite.transform.localScale = new Vector3(150f,40f,1f);
		
		BoxCollider box = NGUITools.AddWidgetCollider (go);
		box.center = new Vector3(0,0,-1);
		box.size = new Vector3(sprite.transform.localScale.x,sprite.transform.localScale.y,0);
		
		UIImageButton button = go.AddComponent<UIImageButton>();
		int job = model.player.job;
		if(job == 0){
			button.normalSprite = name+job;
			button.hoverSprite = name+job;
			button.pressedSprite = name+job;
			button.disabledSprite = name+job;
			box.enabled = false;
			button.enabled = false;
			sprite.spriteName = button.disabledSprite;
		}else {
			button.normalSprite = name+job;
			button.hoverSprite = name+job+"s";
			button.pressedSprite = name+job;
			button.disabledSprite = name+job+"s";
			PlayerSelectScript select = go.AddComponent<PlayerSelectScript>();
			select.init(model.player,tex,texs,gameObject,nameLabel,levelLabel);
			sprite.spriteName = button.disabledSprite;
		}
	}
	
	public void cleanButtons(){
		GameObject[] buttons = GameObject.FindGameObjectsWithTag ("PlayerSelectButton");
		foreach(GameObject obj in buttons){
			Destroy (obj);
		}
	}
	
	public void CreateClick(){
		if(GameInfo.GAME_STATE != GameState.RUN){
			return;
		}
		GameInfo.GAME_STATE = GameState.PLAYER_CREATE;
		CreatePanel.SetActive (true);
	}
	
	public void selected(){
		startButton.collider.enabled = true;
		deleteButton.collider.enabled = true;
	}
	
	public void GameStart(){
		if(GameInfo.GAME_STATE != GameState.RUN)return;
		if(GameInfo.selectModel == null)return;
		string message = Coding<StringDTO>.encode(new StringDTO(GameInfo.selectModel.id));
		NetWorkScript.getInstance ().sendMessage (Protocol.USER,0,UserProtocol.SELECT_CREQ,message);
		GameInfo.GAME_STATE = GameState.WAIT;
	}
}
