using UnityEngine;

public class Enemy_MoveWalker : Enemy_Move
{
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	void FixedUpdate()
	{
		//Stop if encounter something
		if(encounter) return;
		//Move the enemy in the red arrow with speed has set
		rb.MovePosition(rb.position + ((Vector2)transform.right * speed) * Time.fixedDeltaTime);
	}
}
