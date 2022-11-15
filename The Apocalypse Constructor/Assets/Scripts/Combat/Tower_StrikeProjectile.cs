using System.Collections.Generic;
using UnityEngine;

public class Tower_StrikeProjectile : Tower_Strike
{
	public int piercing; List<Collider2D> pierced = new List<Collider2D>();
	public float velocity;
	public float distance; Vector2 prePos; float traveled;
    [SerializeField] Rigidbody2D rb;
	[SerializeField] Collider2D col;

	void OnEnable()
	{
		//Go through all the collider has pierced through
		for (int p = 0; p < pierced.Count; p++) 
		{
			//Unignore this collider has pierced if it still exist
			if(pierced[p] != null) Physics2D.IgnoreCollision(col,pierced[p],false);
		}
		pierced.Clear();
		prePos = transform.position;
		traveled -= traveled;
	}

	void LateUpdate()
	{
		//Get the travel distance between current position and previous
		traveled += (prePos - rb.position).sqrMagnitude;
		//Strike over when reached max distance
		if(traveled >= distance) Over();
		//Move the strike in the red arrow with velocity has get
		rb.MovePosition(rb.position + ((Vector2)transform.right * velocity) * Time.fixedDeltaTime);
		//Update the previous position
		prePos = rb.position;
	}

	void OnCollisionEnter2D(Collision2D other) 
	{
		//If collide with an enemy
		if(other.collider.CompareTag("Enemy"))
		{
			//Hurting the enemt collide with
			Hurting(other.collider.gameObject);
			//Has pierce this enemy
			pierced.Add(other.collider);
			//Ignore the collider of enemy pierced through
			Physics2D.IgnoreCollision(col, other.collider);
			//Over when reached the maximum amount pierced
			if(pierced.Count >= piercing) Over();
		}
		//Instantly over if collide with an ground
		if(other.collider.CompareTag("Ground")) Over();
	}
}