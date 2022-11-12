using UnityEngine;

public class Tower : Structure
{
    public float damage, speed, range;
	public int depleted;
	float countSpeed;
	[HideInInspector] public bool detected;
	[HideInInspector] public bool flipped;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Keep track of this structure as tower
		StructureManager.i.towers.Add(this);
		//Erased track of this structure as filler
		StructureManager.i.fills.Remove(this);
	}

	void Update()
	{
		if(detected) Attacking();
	}

	void Attacking()
	{
		//Counting speed for attack
		countSpeed += Time.deltaTime;
		//If has count enough speed
		if(countSpeed >= speed)
		{
			/// Tower attacking
			print(gameObject.name + " Attacked!");
			//Reset speed counter
			countSpeed -= countSpeed;
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