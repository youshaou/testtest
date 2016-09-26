using UnityEngine;
using System.Collections;

public class PlayerSelectScript : MonoBehaviour {

	private PlayerModel model;
	private UITexture tex;
	private Texture[] texs;
	private GameObject target;
	private UILabel nameLabel;
	private UILabel levelLabel;
	
	public void init(PlayerModel m, UITexture t, Texture[] ts, GameObject target, UILabel nameLabel, UILabel levelLabel){
		this.model = m;
		this.tex = t;
		this.texs = ts;
		this.target = target;
		this.nameLabel = nameLabel;
		this.levelLabel = levelLabel;
		
	}
	
	public void OnClick(){
		GameInfo.selectModel = model;
		tex.mainTexture = texs[model.job];
		nameLabel.text = "NickName:"+model.name;
		levelLabel.text = "Level:"+model.level;
		target.BroadcastMessage ("selected");
	}
}
