using UnityEngine;

public class Tower : Structure
{
    public float damage, rate, range;
	public int depleted;
	float countRate;
	public bool detected;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Keep track of this structure as tower
		StructureManager.i.towers.Add(this);
		//Erased track of this structure as filler
		StructureManager.i.fills.Remove(this);
		//Consume the deplete energy when tower got create
		Inventory.i.materials.Consume(0,0,0, depleted);
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
		if(countRate >= rate)
		{
			/// Tower attacking
			print(gameObject.name + " Attacked!");
			//Reset speed counter
			countRate -= countRate;
		}
	}

	public override void Die()
	{
		//No longer consume the amount of energy will be depleted
		Inventory.i.materials.Consume(0,0,0,-depleted);
		//Erased track of this structure as tower
		StructureManager.i.towers.Remove(this);
		base.Die();
	}
}