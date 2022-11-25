using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Name", menuName = "Scriptable Object/Enemy Spawn", order = 1)]
public class EnemySpawn : ScriptableObject
{
	public GameObject prefab;
	public MinMaxVector2 spawnOffset;
}