using UnityEngine;

public class Enemy : Entity
{
	public Combat_Caster caster;
	public Stash.Ingredients drops;

	void OnValidate() 
	{
		//Get the caster of the tower if needed
		if(caster == null) caster = GetComponent<Combat_Caster>();
		//Refresh dps amount of initial stats
		caster.InitialStats.RefreshDPS();
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
		//If inventory exist
		if(Inventory.i != null)
		{
			//Gain the amount of materials this enemy will drop
			Inventory.i.materials.Gain(drops.wood,drops.steel, drops.gunpowder,0,0);
		}
		//Entity die then deactive the enemy after
		base.Die(); gameObject.SetActive(false);
	}
}