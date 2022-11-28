using UnityEngine;

public class Tower : Structure
{
	public Combat_Caster caster;
	[HideInInspector] public bool insufficient;
	public int depleted;
	GameObject insufIndicator;

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
		//Keep track of this structure as tower
		manager.towers.Add(this);
		//Erased track of this structure as filler
		manager.fills.Remove(this);
		//Consume the deplete energy when tower got create
		Inventory.i.materials.Consume(0,0,0,depleted);
	}

	public override void LevelUp()
	{
		//Growing the intial stats stats with tower current level to get final stats
		caster.finalStats = Combats.GrowingStats(Level, caster.InitialStats, caster.GrowthStats);
		//Structure level up
		base.LevelUp();
	}

	public override void FlipStructure(bool isFlip)
	{
		base.FlipStructure(isFlip);
		//Send flip state to the caster whne it got flip
		GetComponent<Combat_Caster>().flipped = flipped;
	}

	public void RefreshInsufficient()
	{			
		//If havent got insufficient indicator
		if(insufIndicator == null)
		{
			//Create an new one 
			insufIndicator = Instantiate(manager.insufficientIndiPrefab, transform.position, Quaternion.identity);
			//Parent the indicator to manager
			insufIndicator.transform.SetParent(manager.transform);
		}
		//Show insufficient indicator base on tower's insufficient
		insufIndicator.SetActive(insufficient);
	}

	public override void Die()
	{
		//Erased track of this structure as tower
		manager.towers.Remove(this);
		//Regain the energy been depleted
		Inventory.i.materials.Gain(0,0,0,depleted,0);
		//Destroy it insufficient indicator
		Destroy(insufIndicator);
		base.Die();
	}
}