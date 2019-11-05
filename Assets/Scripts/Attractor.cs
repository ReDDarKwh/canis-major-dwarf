using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

	const float G = 667.4f;

	public static List<Attractor> Attractors;

	public Rigidbody rb;

	public Vector2 startForce;

	private bool forceApplied =false;

	public bool destroyable = true;

	public GameObject explosionEffect;
    public PlayerController playerControl;



    void FixedUpdate ()
	{
		foreach (Attractor attractor in Attractors)
		{
			if (attractor != this)
				Attract(attractor);
		}

		if(!forceApplied){
			rb.AddForce(startForce, ForceMode.Impulse);
			forceApplied = true;
		}
	}



	void ByeBye(){

			Destroy(this.gameObject);
			var explosion = Instantiate(explosionEffect, transform.position, Quaternion.Euler(90,0,0));
			Destroy(explosion, 5);

			if(playerControl){
				playerControl.planetDead();
			}
	}

	void Update(){
		if((this.transform.position - new Vector3(0,0,200)).magnitude > 500 ){
			ByeBye();
		}

		if(playerControl && (this.transform.position - playerControl.end.position).magnitude < 16 ){
			playerControl.YEAHHH();

			ByeBye();
			
		}
	}
	void OnEnable ()
	{
		if (Attractors == null)
			Attractors = new List<Attractor>();

		Attractors.Add(this);
	}

	void OnDisable ()
	{
		Attractors.Remove(this);
	}

	void Attract (Attractor objToAttract)
	{
		Rigidbody rbToAttract = objToAttract.rb;

		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.magnitude;

		if (distance == 0f)
			return;

		float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
		Vector3 force = direction.normalized * forceMagnitude;

		rbToAttract.AddForce(force);
	}

	void OnCollisionEnter(Collision collision)
    {

		if(collision.collider.GetComponent<Attractor>() == null){
			return;
		}

		if(destroyable){
			ByeBye();
		}
    }

}
