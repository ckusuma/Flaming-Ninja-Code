using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour {
	public static List<Boid> boidList = new List<Boid>();
	public Rigidbody body;
	public float speed = 5f;
	public float minFlockDistance = 2f;
	public float Xmin = -10f, Xmax = 10f, Ymin=-10f, Ymax=10f, Zmin=-10f, Zmax=10f;

 

	void Start () 
	{
		//Register this boid with the list of boids.
		boidList.Add(this);
		//Cache the rigidbody for later...
		body = GetComponent<Rigidbody>();
	}

	void OnDestroy()
	{
		boidList.Remove(this);
	}

	void FixedUpdate () 
	{
		//Add the rigid body of this boid to the gather() an flock() rules.
		body.velocity += gather() + flock();
        this.transform.rotation = Quaternion.LookRotation(body.velocity);
        this.transform.rotation = Quaternion.Euler(-90, transform.rotation.y, transform.rotation.z);
		//make sure this boid stays in boundaries...
		bound();
	}

	private Vector3 gather()
	{
		Vector3 centerOfSwarm = new Vector3();

		//Add all boid positions together...
		foreach(Boid b in boidList)
		{
			if(b != this)
			{
				centerOfSwarm += b.gameObject.transform.position;
			}
		}
		//Divide by the total number to find the average...
		centerOfSwarm /= boidList.Count - 1;
		//find the delta of the center and this boid position and add a percentage to move towards the center..
		return (centerOfSwarm - this.transform.position) / 100f;
	}

	private Vector3 flock()
	{
		Vector3 flockModifier = new Vector3();
		//loop over all other boids...
		foreach(Boid b in boidList)
		{
			//if boid isn't me..
			if(b != this)
			{
				//if boid is too close to me...
				if(Mathf.Abs(Vector3.Distance(b.transform.position, this.transform.position)) < minFlockDistance)
				{
					//tell boid to go away...
					flockModifier = flockModifier - (b.transform.position - this.transform.position);
				}
			}
		}
		return flockModifier;
	}

	private void bound()
	{
		Vector3 v = body.velocity;
		if(this.transform.localPosition.x < Xmin){
			v.x = speed;
		}else if(this.transform.localPosition.x > Xmax)
		{
			v.x = -speed;
		}
		if(this.transform.localPosition.y < Ymin){
			v.y = speed;
		}else if(this.transform.localPosition.y > Ymax)
		{
			v.y = -speed;
		}
		if(this.transform.localPosition.z < Zmin){
			v.z = speed;
		}else if(this.transform.localPosition.z > Zmax)
		{
			v.z = -speed;
		}
		body.velocity = v;
	}
}
