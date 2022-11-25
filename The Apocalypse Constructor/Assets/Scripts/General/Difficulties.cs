using UnityEngine;

public class Difficulties : MonoBehaviour
{
    #region Set this class to singleton
	public static Difficulties i {get{if(_i==null){_i = GameObject.FindObjectOfType<Difficulties>();}return _i;}} static Difficulties _i;
	#endregion

	[SerializeField] DaysManager days;
	[Header("Enemy Spawning")]
	[SerializeField] EnemiesSpawner spawner;
	[Tooltip("The bigger the number the rarer enemy could spawn")] [SerializeField] 
	float spawnBoost;

	void OnEnable() {DaysManager.i.onCycle += WhenDayCycleChange;}

	void OnDisable() {DaysManager.i.onCycle -= WhenDayCycleChange;}

	void WhenDayCycleChange(bool night)
	{
		//If cycle to the morning
		if(!night)
		{
			//Scale the rairty of enemy spawning
			ScaleEnemySpawnRarity();
		}
		//If cycle to the night
		if(night)
		{

		}
	}

	void ScaleEnemySpawnRarity()
	{
		//Get spawning difficulty by multiply days with boost 
		float diff = days.counter * spawnBoost;
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
}