using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Name", menuName = "Scriptable Object/Enemy Spawn", order = 1)]
public class EnemySpawn : ScriptableObject
{
	public GameObject prefab;
	public SpawnOffset spawnOffset;
    [System.Serializable] public struct SpawnOffset {public Vector2 min, max;}
}
