using UnityEngine;

public class Enemy_MoveWalker : Enemy_Move
{
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	void FixedUpdate()
	{
		//Current velocity as speed
		float velocity = movementSpeed;
		//Stop if caster detected something
		if(caster.detected) velocity = 0;
		//Move the enemy in the red arrow with velocity has set
		rb.MovePosition(rb.position + ((Vector2)transform.right * velocity) * Time.fixedDeltaTime);
	}
}
