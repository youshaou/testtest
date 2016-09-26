using UnityEngine;
using System.Collections;

public class RunTest : MonoBehaviour {
	
	//public Transform TargetObject = null;
	
	private Vector3 point;
    private float time;
	public Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		/*
		if (TargetObject != null)
			GetComponent<NavMeshAgent>().destination = TargetObject.position;
			*/
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(1)){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition );
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit)){
				//
				point = hit.point;
				transform.LookAt (new Vector3(point.x,transform.position.y,point.z));
				
			}
			GetComponent<NavMeshAgent>().destination = point;
			//Debug.Log (GetComponent<NavMeshAgent>().remainingDistance);
			
			
		}
		if(GetComponent<NavMeshAgent>().remainingDistance > GetComponent<NavMeshAgent>().stoppingDistance){
			animator.SetBool ("run",true);
		}else{
			animator.SetBool ("run",false);
		}
		
		
		
		
		
	
	}
}
