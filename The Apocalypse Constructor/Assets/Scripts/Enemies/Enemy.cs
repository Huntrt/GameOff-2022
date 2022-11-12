using UnityEngine;

public class Enemy : Entity
{
	protected override void OnEnable()
	{
		base.OnEnable();
		EnemyManager.Record(gameObject);
	}

	public override void Die()
	{
		EnemyManager.Erased(gameObject);
		base.Die();
	}
}