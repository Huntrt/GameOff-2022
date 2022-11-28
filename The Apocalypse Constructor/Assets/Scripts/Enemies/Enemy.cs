using UnityEngine;

public class Enemy : Entity
{
	public Combat_Caster caster;

	void OnValidate() 
	{
		//Get the caster of the tower if needed
		if(caster == null) caster = GetComponent<Combat_Caster>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		EnemiesManager.Record(this);
		//If there is day mananger
		if(DaysManager.i != null)
		{
			//Grow enemy health that use day pass as level 
			GrowingHealth(DaysManager.i.passes);
		}
	}

	public override void Die()
	{
		EnemiesManager.Erased(this);
		//Deactive the enemy upon dealth
		gameObject.SetActive(false);
		base.Die();
	}
}