using UnityEngine;

public class Tower : Structure
{
    public float damage, speed, range;
	public int deplete;
	float countSpeed;
	[HideInInspector] public bool detected;

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
}