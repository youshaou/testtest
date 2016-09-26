using UnityEngine;
using System.Collections;

public class RootMotionScript : MonoBehaviour {

	private float walkSpeed=5f;
	private float runSpeed=15f;
	private float MoveSpeed=5f;
	private bool hasJumped=false;
	private bool hasDrawBlade=false;
	private float comboTime=1.0f;
	private bool comboAble=false;
	private float timer;
	private float attackStateTime=5.0f;
	private bool doSkill=false;
	public Animator animator;
	// Use this for initialization
	void Start () {
		if(animator.layerCount >= 3){
			animator.SetLayerWeight (0,0);
			animator.SetLayerWeight (1,0);
			animator.SetLayerWeight (2,0);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		NormalState();
		AttackState();
	}
	
	private void NormalState(){
		
		AnimatorStateInfo state0 = animator.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo state1 = animator.GetCurrentAnimatorStateInfo (1);
		//---------------------
		//move control:
		//-------------------Control move not only in nomalState but also in attackState. 	
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			animator.SetBool ("run",true);
			MoveSpeed = runSpeed;
		}else if(Input.GetKeyUp(KeyCode.LeftShift)){
			animator.SetBool ("run",false);
			MoveSpeed = walkSpeed;
		}
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
		
		//---------------------
		//jump control:
		//---------------------
		if(Input.GetKeyDown(KeyCode.Space)&&!hasJumped){
			animator.SetBool ("JumpBool",true);
		}
		
		if(state0.IsName ("Base Layer.Jump_NoBlade")||state1.IsName ("DrawBlade Layer.Jump_DrawBlade")){
			animator.SetBool ("JumpBool",false);
			hasJumped = true;
		}
		
		if(!state0.IsName ("Base Layer.Jump_NoBlade")&& hasJumped &&!state1.IsName ("DrawBlade Layer.Jump_DrawBlade"))
		{
			hasJumped = false;
		}
		
		
	}
	
	private void AttackState(){
		
		AnimatorStateInfo state0 = animator.GetCurrentAnimatorStateInfo (0);
		AnimatorStateInfo state1 = animator.GetCurrentAnimatorStateInfo (1);
		AnimatorStateInfo state2 = animator.GetCurrentAnimatorStateInfo (2);
		//---------------------
		//attack control:
		//---------------------
		if(Input.GetMouseButtonDown(0) && !hasDrawBlade){//hasDrawBlade = false.
			animator.SetBool ("DrawBladeBool",true);//first time we need to draw blade.					
			animator.SetBool ("AttackBool",false);
			animator.SetBool ("isFirstAttack",false);
		}
		
		if(state0.IsName ("Base Layer.DrawBlade") || state0.IsName ("Base Layer.DrawBlade 0")){
			animator.SetBool ("DrawBladeBool",false); 
			hasDrawBlade = true;
			attackStateTime=5f;
		}
		
		if(hasDrawBlade){		
			animator.SetLayerWeight (0,0);
			animator.SetLayerWeight (1,1);
			animator.SetLayerWeight (2,0);
		}//turn into attack state.(draw blade)
		
		if(hasDrawBlade && Input.GetMouseButtonDown(1)){
			attackStateTime=5f;
			doSkill = true;
			animator.SetBool ("BlockBool",true);
		}
		if(state2.IsName ("Blade Layer.Block")){
			animator.SetBool ("BlockBool",false);
		}
		if(hasDrawBlade && Input.GetKeyDown(KeyCode.E)){
			attackStateTime=5f;
			doSkill = true;
			animator.SetBool ("BuffBool",true);
		}
		if(state2.IsName ("Blade Layer.Buff")){
			animator.SetBool ("BuffBool",false);
		}
		if(hasDrawBlade && Input.GetKeyDown(KeyCode.R)){
			attackStateTime=5f;
			doSkill = true;
			animator.SetBool ("SkillBool",true);
		}
		if(state2.IsName ("Blade Layer.Skill")){
			animator.SetBool ("SkillBool",false);
		}
		if(hasDrawBlade && Input.GetKeyDown(KeyCode.F)){
			attackStateTime=5f;
			doSkill = true;
			animator.SetBool ("ComboAttack",true);
		}
		if(state2.IsName ("Blade Layer.ComboAttack")){
			animator.SetBool ("ComboAttack",false);
		}
		
		
		if(Input.GetMouseButtonDown(0) && hasDrawBlade && !comboAble){//first attack.
		    timer=comboTime;
			attackStateTime=5f;
			animator.SetBool ("AttackBool",true);
			animator.SetBool ("isFirstAttack",true);
		}
		
		if(state2.IsName ("Blade Layer.Attack0")){
			animator.SetBool ("isFirstAttack",false);
			comboAble = true;
		}
		
		if(animator.GetBool ("AttackBool")){
			if(timer>0f){
				//comboAble = true;
				timer -= Time.deltaTime*0.3f;
			}
			else{
				comboAble = false;			
				animator.SetBool ("AttackBool",false);
				animator.SetBool ("ComboBool",false);
			}
		}
		
		if(comboAble && Input.GetMouseButtonDown(0) && state2.IsName ("Blade Layer.Attack0")){
			timer=comboTime;
			attackStateTime=5f;
			animator.SetBool ("ComboBool",true);
		}
		if(comboAble && Input.GetMouseButtonDown(0) && state2.IsName ("Blade Layer.Attack1")){
			timer=comboTime;
			attackStateTime=5f;
			animator.SetBool ("ComboBool",false);
		}//change attack action.
		
		if(comboAble||doSkill){
			
			animator.SetLayerWeight (0,0);
			animator.SetLayerWeight (1,0);
			animator.SetLayerWeight (2,1);
		}
		
		if(comboAble||doSkill){
			if(Input.GetAxis ("Vertical")!=0f || Input.GetAxis ("Horizontal")!=0f || Input.GetKeyDown(KeyCode.Space)){
				animator.SetLayerWeight (0,0);
			    animator.SetLayerWeight (1,1);
			    animator.SetLayerWeight (2,0);
			}
		}
		
		if(Input.GetKeyDown (KeyCode.Q) && hasDrawBlade){
			hasDrawBlade = false;
			comboAble = false;
			doSkill = false;
			animator.SetBool ("AttackBool",false);
			animator.SetBool ("ComboBool",false);
			animator.SetLayerWeight (0,1);
			animator.SetLayerWeight (1,0);
			animator.SetLayerWeight (2,0);
		}//back into normal state.
		
		if(hasDrawBlade){
			if(attackStateTime > 0f){
				attackStateTime -= Time.deltaTime*1f;
			}
			else{
				hasDrawBlade = false;
			    comboAble = false;
				doSkill = false;
			    animator.SetBool ("AttackBool",false);
			    animator.SetBool ("ComboBool",false);
			    animator.SetLayerWeight (0,1);
			    animator.SetLayerWeight (1,0);
			    animator.SetLayerWeight (2,0);
			}
		}
		
		
	}
}
