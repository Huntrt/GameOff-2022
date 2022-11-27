using UnityEngine;

public class MainMenuSpawner : MonoBehaviour
{   
	public float spawnRate; float spawnTimer;
	public EnemySpawn[] spawns;
	[SerializeField] Transform[] points;

	void Update()
	{
		//Timing spawn timer
		spawnTimer += Time.deltaTime;
		//If timer has reach spawn rate then begin spawing and reset the timer
		if(spawnTimer >= (1/spawnRate)) {DeicideSpawning(); spawnTimer -= spawnTimer;}
	}

	void DeicideSpawning()
	{
		//Randomly choose an enemy spawn
		EnemySpawn spawn = spawns[Random.Range(0,spawns.Length)];
		//Spawn enemy has get with random between spawn point position
		SpawnEnemy(spawn, points[Random.Range(0,2)].position);
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
}
