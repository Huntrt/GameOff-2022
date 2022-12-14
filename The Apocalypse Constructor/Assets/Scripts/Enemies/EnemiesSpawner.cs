using UnityEngine;
// using Unity.Mathematics; (LN10)

public class EnemiesSpawner : MonoBehaviour
{
	public float spawnRate; float spawnTimer;
	public EnemySpawning[] spawns;
	[System.Serializable] public class EnemySpawning
	{
		public EnemySpawn enemy;
		public float initialRarity;
		[HideInInspector] public float scaledRarity;
		[HideInInspector] public float finalRarity;
		public int dayAppear; 
		public bool allowToAppear {get {return DaysManager.i.passes >= dayAppear;}}
	}
    [SerializeField] Vector2[] spawnPoint = new Vector2[2];
	[SerializeField] float spawnPointInward;
	[SerializeField] Ground ground;

	void OnEnable()
	{
		ground.onExpand += RefreshPoint;
		//@ Set the spawn point Y axis using ground hight move 1 block above
		spawnPoint[0].y = ground.initalSize.y + Map.i.spacing;
		spawnPoint[1].y = ground.initalSize.y + Map.i.spacing;
	}

	void RefreshPoint()
	{
		//@ Get spawn point X position by current ground left/righ decrease with inward amount
		spawnPoint[0].x = Map.Spaced(ground.groundLeft + spawnPointInward);
		spawnPoint[1].x = Map.Spaced(ground.groundRight - spawnPointInward);
	}

	void Update()
	{
		//If is night time
		if(DaysManager.i.isNight)
		{
			//Timing spawn timer
			spawnTimer += Time.deltaTime;
			//If timer has reach spawn rate then begin spawing and reset the timer
			if(spawnTimer >= (1/spawnRate)) {DeicideSpawning(); spawnTimer -= spawnTimer;}
		}
	}

	void DeicideSpawning()
	{
		//Choose wich spawn point will be use
		Vector2 pos = spawnPoint[Random.Range(0,2)];
		//Get the sum amount all spawn final rarity
		float sum = 0; for (int s = 0; s < spawns.Length; s++)
		{
			//If this enemy haven't able to appear yet then skip if
			if(!spawns[s].allowToAppear) continue;
			//Sum of final rarity has been increase
			sum += spawns[s].finalRarity;
		}
		//Randomize inside the sum
		sum = Random.Range(0, sum);
		//Go through all the enemy could spawn
		for (int s = 0; s < spawns.Length; s++)
		{
			//If this enemy haven't able to appear yet then skip if
			if(!spawns[s].allowToAppear) continue;
			//If this enemy final rarity take all the sum
			if(sum - spawns[s].finalRarity <= 0)
			{
				//Spawn this enemy
				SpawnEnemy(spawns[s].enemy, pos); break;
			}
			//If there sill sum left over
			else
			{
				//Reduce the sum with this enemy final rarity
				sum -= spawns[s].finalRarity;
			}
		}
	}

	void SpawnEnemy(EnemySpawn spawn, Vector2 pos)
	{
		//Get the min/max off set of given spawn
		MinMaxVector2 offset = spawn.spawnOffset;
		//Randmoize offset with given position to decide true spawn position
		Vector2 spawnPos = pos + new Vector2(Random.Range(offset.min.x, offset.max.x), Random.Range(offset.min.y, offset.max.y));
		//Pooling the given spawn enemy
		Pooler.i.Create(spawn.prefab, spawnPos, Quaternion.identity);
	}

	void OnDrawGizmos() 
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(spawnPoint[0], 0.25f);
		Gizmos.DrawWireSphere(spawnPoint[1], 0.25f);
	}

	void OnDisable()
	{
		ground.onExpand -= RefreshPoint;
	}
}
