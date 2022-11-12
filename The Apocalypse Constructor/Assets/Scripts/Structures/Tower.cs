using UnityEngine;

public class Tower : Structure
{
    public float damage, speed, range;
	public int deplete;
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
		//Erased track of this structure as tower
		StructureManager.i.towers.Remove(this);
		base.Die();
	}
}