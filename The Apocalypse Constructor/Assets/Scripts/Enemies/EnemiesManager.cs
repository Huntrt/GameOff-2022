using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
	#region Set this class to singleton
	public static EnemiesManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<EnemiesManager>();}return _i;}} static EnemiesManager _i;
	#endregion

    public List<Enemy> enemies = new List<Enemy>();
	public LayerMask enemyLayer;
	public GameObject directSightIndicator, rotateSightIndicator;

	public static void Spawn(GameObject enemy, Vector2 position)
	{
		//If the enemy got spawn at the left side of map then flip it
		float facing = 0; if(position.x < 0) facing = 180;
		//Create the given enemy at given position and facing has decided
		Instantiate(enemy, position, Quaternion.Euler(0,facing,0));
	}

	//Record given enemy to total list
	public static void Record(Enemy enemy) {i.enemies.Add(enemy);}
	//Erased given enemy from total list
	public static void Erased(Enemy enemy) {i.enemies.Remove(enemy);}
}