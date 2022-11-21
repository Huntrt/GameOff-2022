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
		float velocity = speed;
		//Stop if encounter something
		if(encounter) velocity = 0;
		//Move the enemy in the red arrow with velocity has set
		rb.MovePosition(rb.position + ((Vector2)transform.right * velocity) * Time.fixedDeltaTime);
	}
}
