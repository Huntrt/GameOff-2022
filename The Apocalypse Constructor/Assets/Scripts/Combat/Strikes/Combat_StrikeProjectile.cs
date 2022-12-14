using System.Collections.Generic;
using UnityEngine;

public class Combat_StrikeProjectile : Combat_Strike
{
	public float velocity;
	public float travel; float traveled; Vector2 prePos;
	public int piercing; List<Collider2D> pierced = new List<Collider2D>();
    [SerializeField] Rigidbody2D rb;
	[SerializeField] Collider2D col;

	protected override void OnEnable()
	{
		base.OnEnable();
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

	void FixedUpdate()
	{
		//Get the travel distance between current position and previous
		traveled += Vector2.Distance(rb.position, prePos);
		//Despawn when reached max travel
		if(traveled >= travel) Despawn(transform.position);
		//Move the strike in the red arrow with velocity has get
		rb.MovePosition(rb.position + ((Vector2)transform.right * velocity) * Time.fixedDeltaTime);
		//Update the previous position
		prePos = rb.position;
	}

	void OnCollisionEnter2D(Collision2D other) 
	{
		//If collide with the ground
		if(other.collider.CompareTag("Ground"))
		{
			//Instantly despawn when collide with ground and send the contact point
			Despawn(other.collider.bounds.ClosestPoint(transform.position));
			//Dont collide with anything thing else
			return;
		}
		//? Use physic layer to determent contact
		//Stop when already reached the maximum amount of pierced
		if(pierced.Count >= piercing) return;
		//Get the contact point from projectile to entity it collide with
		Vector2 contactPoint = other.collider.bounds.ClosestPoint(transform.position);
		//Hurting the entity got collide with to dealing raw caster damage
		Hurting(damage, other.collider.gameObject, contactPoint);
		//Has pierce this entity
		pierced.Add(other.collider);
		/// Ignore the collider of entity pierced through
		Physics2D.IgnoreCollision(col, other.collider);
		//Over when reached the maximum amount pierced
		if(pierced.Count >= piercing) {Over(contactPoint); return;}
	}
}