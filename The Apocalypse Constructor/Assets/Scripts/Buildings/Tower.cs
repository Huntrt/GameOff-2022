using UnityEngine;

public class Tower : Building
{
	public new SO_Tower so;
    public float damage, speed, range, crit;
	float countSpeed;
	public bool isAttack;

	void Start()
	{
		SetupStats();
	}

	void SetupStats()
	{
		maxHealth = so.maxHealth;
		consumption = so.consumption;
		damage = so.damage;
		speed = so.speed;
		range = so.range;
		crit = so.crit;
	}

	void Update()
	{
		if(isAttack) Attacking();
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
}