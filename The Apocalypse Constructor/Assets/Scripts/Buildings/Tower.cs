using UnityEngine;

public class Tower : Building
{
    public float damage, speed, range, crit, consumption;
	float countSpeed;
	public bool isAttack;

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