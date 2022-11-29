using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDifficulties : MonoBehaviour
{
    [SerializeField] DaysManager days;
	[SerializeField] EnemiesSpawner spawner;
	[Tooltip("The bigger the number the rarer enemy could spawn (2 = avg")] [SerializeField] float spawnBoost;
	[SerializeField] float spawnRateGrowEveryDay;
	[SerializeField] float spawnRateGrowthFor;
	
	void OnEnable() {DaysManager.i.onCycle += WhenDayCycleChange;}
	void OnDisable() {DaysManager.i.onCycle -= WhenDayCycleChange;}

	void WhenDayCycleChange(bool night)
	{
		//If cycle to the morning
		if(!night)
		{
			//Scale the rairty of enemy spawning
			ScaleEnemySpawnRarity();
			//Begin growing spawn rate
			GrowthSpawnRate();
		}
		//If cycle to the night
		if(night)
		{

		}
	}
	
	public void ScaleEnemySpawnRarity()
	{
		//Get spawning difficulty by multiply days with boost 
		float diff = days.passes * spawnBoost;
		//Go through all the enemy could spawn
		for (int s = 0; s < spawner.spawns.Length; s++)
		{
			//Save this spawning
			EnemiesSpawner.EnemySpawning spawn = spawner.spawns[s];
			//Scaled initial rarity using decilog scale that take into account difficulty
			spawn.scaledRarity += diff * Mathf.Log(spawn.initialRarity) / Unity.Mathematics.math.LN10;
			//The final rarity will be use are the sum of scaled and initial
			spawn.finalRarity = spawn.initialRarity + spawn.scaledRarity;
		}
	}

	public void GrowthSpawnRate()
	{
		//When every set day has pass
		if(days.passes % spawnRateGrowEveryDay == 0)
		{	
			//Increase spawn rate with an set amount
			spawner.spawnRate += spawnRateGrowthFor;
		}
	}
}
