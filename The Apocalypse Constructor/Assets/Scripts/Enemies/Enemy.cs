using UnityEngine;

public class Enemy : Entity
{
	public Combats.Stats stats;

	void OnValidate() 
	{
		//Update stats DPS
		stats.DPS = (float)System.Math.Round(stats.damage / stats.rateTimer,2);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		EnemiesManager.Record(this);
	}

	public override void Die()
	{
		EnemiesManager.Erased(this);
		base.Die();
	}
}