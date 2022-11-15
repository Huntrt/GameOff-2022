using UnityEngine;
using System;

public class Tower : Structure
{
	public bool detected; 
	public Stats stats, scaling;
	[HideInInspector] public bool insufficient;
	public int depleted;
	float countRate;
	public Action onAttack;
	GameObject insufIndicator;

	[Serializable] public class Stats {public float damage, rate, range;}

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

	void Update()
	{
		if(detected) Attacking();
	}

	void Attacking()
	{
		//Counting speed for attack
		countRate += Time.deltaTime;
		//If has count enough speed
		if(countRate >= stats.rate)
		{
			//Call attack
			onAttack?.Invoke();
			//Reset speed counter
			countRate -= countRate;
		}
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