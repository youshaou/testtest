using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float runSpeed = 15f;
	public float walkSpeed = 5f;
	private float MoveSpeed = 5f;
	CharacterController con;
	public Animator animator;
	private int playerState = PlayerStateConstans.IDLE;
	public PlayerModel player;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UISprite hpSprite;
	//public GameObject fire;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		con = (CharacterController)GetComponent("CharacterController");
	}
	
	public void InfoInit(PlayerModel model){
		this.player = model;
		nameLabel.text = model.name;
		levelLabel.text = ""+model.level;
		hpSprite.fillAmount = player.hp/player.maxHp;
	}

	public void levelUp(){
		nameLabel.text = player.name;
		levelLabel.text = ""+player.level;
	}

	// Update is called once per frame
	void Update () {
		
		switch(playerState){
		case PlayerStateConstans.FORWARD:
			forward();
			break;
		case PlayerStateConstans.BACK:
			back ();
			break;
		case PlayerStateConstans.LEFT:
			left ();
			break;
		case PlayerStateConstans.RIGHT:
			right ();
			break;
		case PlayerStateConstans.IDLE:
			break;
		case PlayerStateConstans.ATTACK:
			attacking();
			break;
		case PlayerStateConstans.SKILL:
			//skilling();
			break;
		case PlayerStateConstans.DIE:
			break;
		}
	
	}
	
	
	private void infoChange(){
		
	}
	
	private void beAttack(string message){
		Debug.Log (message+" beAttack! ");
		BeAtkDTO dto = Coding<BeAtkDTO>.decode (message);
		
	}
	
	private void attacking(){
		//Debug.Log ("-attack-");
		
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo (0);
		float now = info.normalizedTime * animator.speed % info.length;
		if(now /info.length >= 0.97f){
			if(playerState == PlayerStateConstans.ATTACK){
				if(player.id == GameInfo.myModel.id){
					Vector3 v = new Vector3(transform.position.x,transform.position.y+transform.collider.bounds.size.y/2,transform.position.z);
					Ray ray = new Ray(v,transform.forward);
					RaycastHit hit;
					if(Physics.Raycast (ray,out hit,1f)){
						if(hit.transform.tag == "Player"){
							StringDTO dto = new StringDTO(hit.transform.name);
							string message = Coding<StringDTO>.encode (dto);
						    NetWorkScript.getInstance ().sendMessage (Protocol.MAP,GameInfo.myModel.map,MapProtocol.BE_ATTACK_CREQ,message);
							
						}
					}
				}
			}
			animator.SetBool ("attack",false);
			//Debug.Log ("-attack over-");
			playerState = PlayerStateConstans.IDLE;
			GameInfo.PLAYER_STATE = PlayerStateConstans.IDLE;
		}
	}
	
 	private void skilling(){
		//Debug.Log ("= skill =");
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo (0);
		float now = info.normalizedTime * animator.speed % info.length;
		if(now /info.length >= 0.97f){		
			animator.SetBool ("skill",false);
			
			//Debug.Log ("= skill over =");
			
			playerState = PlayerStateConstans.IDLE;
			GameInfo.PLAYER_STATE = PlayerStateConstans.IDLE;
		}
	}
	
	void forward(){
		transform.Translate (Vector3.forward*Time.deltaTime*MoveSpeed*1f);
	}
	void back(){
		transform.Translate (Vector3.back*Time.deltaTime*MoveSpeed*1f);
	}
	void left(){
		//transform.Translate (Vector3.left*Time.deltaTime*MoveSpeed*1f);
		transform.Rotate (Vector3.up*-40*Time.deltaTime);
	}
	void right(){
		//transform.Translate (Vector3.right*Time.deltaTime*MoveSpeed*1f);
		transform.Rotate (Vector3.up*40*Time.deltaTime);
	}
	
	
	void PlayerState(int state){
		
		playerState = state;
		switch(state){
		case PlayerStateConstans.FORWARD:
			animator.SetFloat("V",1f);
			break;
		case PlayerStateConstans.BACK:
			animator.SetFloat("V",-1f);
			break;
		case PlayerStateConstans.LEFT:
			animator.SetFloat("H",-1f);
			break;
		case PlayerStateConstans.RIGHT:
			animator.SetFloat("H",1f);
			break;
		case PlayerStateConstans.IDLE:
			animator.SetFloat("V",0f);
			animator.SetFloat("H",0f);
			break;
		case PlayerStateConstans.ATTACK:
			animator.SetBool ("attack",true);
			break;
		case PlayerStateConstans.SKILL:
			//animator.SetBool ("skill",true);
			break;
		case PlayerStateConstans.DIE:
			break;
		}
	}
	
	void setR(Quaternion q){
		transform.rotation = q;
	}
	void setP(Vector3 v){
		transform.position = v;
	}
	/*
	private void WalkNoBlade(){
		
		animator.SetFloat("V",Input.GetAxis ("Vertical"));
		animator.SetFloat("H",Input.GetAxis ("Horizontal"));
		if(Input.GetAxis ("Vertical")>= 1f){
			transform.Translate (Vector3.forward*Time.deltaTime*MoveSpeed*1f);
		}
		if(Input.GetAxis ("Vertical") <= -1f){
		    transform.Translate (Vector3.back*Time.deltaTime*MoveSpeed*1f);
		}
		if(Input.GetAxis ("Horizontal")>= 1f){
			transform.Translate (Vector3.right*Time.deltaTime*MoveSpeed*1f);
		}
		if(Input.GetAxis ("Horizontal") <= -1f){
		    transform.Translate (Vector3.left*Time.deltaTime*MoveSpeed*1f);
		}
	}
	
	*/
	
	
}
