using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	#region Set this class to singleton
	public static EnemyManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<EnemyManager>();}return _i;}} static EnemyManager _i;
	#endregion

	//temp: replace with enemy component
    public List<GameObject> enemies = new List<GameObject>();
	public LayerMask enemyLayer;

	public static void Spawn(GameObject enemy, Vector2 position)
	{
		//If the enemy got spawn at the left side of map then flip it
		float facing = 0; if(position.x < 0) facing = 180;
		//Create the given enemy at given position and facing has decided
		Instantiate(enemy, position, Quaternion.Euler(0,facing,0));
	}

	//Record given enemy to total list
	public static void Record(GameObject enemy) {i.enemies.Add(enemy);}
	//Erased given enemy from total list
	public static void Erased(GameObject enemy) {i.enemies.Remove(enemy);}

	public static GameObject Closest(Vector2 point, RaycastHit2D[] detects)
	{
		//The most closet distance
		float most = Mathf.Infinity;
		//The potential closet enemy
		GameObject potential = null;
		//Go through all the enemy got detect
		for (int d = 0; d < detects.Length; d++)
		{
			//Get the distance by sqr magnitude the direction from point to this detect object
			float dist = ((Vector2)detects[d].collider.transform.position - point).sqrMagnitude;
			//If this ditance are closer than the most closest
			if(dist < most)
			{
				//This distance are now the most closest
				most = dist;
				//Potential enemy are now this enemy
				potential = detects[d].collider.gameObject;
			}
		}
		//Return the final potential enemy
		return potential;
	}
}