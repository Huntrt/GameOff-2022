using System.Collections.Generic;
using UnityEngine;

public class Tower_Strike_Explode : MonoBehaviour
{
    [HideInInspector] public Tower_Strike strike;
	public Explosion hitExplosion, despawnExplosion;
	[System.Serializable] public class Explosion
	{	
		public bool enable;
		public float damageScaling;
		public float radius;
	}

	void Reset() 
	{
		strike = GetComponent<Tower_Strike>();
	}

	void OnEnable()
	{
		//Print error if the object dont has strike for explode
		if(strike == null) Debug.LogError(gameObject.name + " explode mod need to be an strike");
		//Adding explode to on hit and on despawn event
		if(hitExplosion.enable) strike.onHit += ExplodeOnHit; 
		if(despawnExplosion.enable) strike.onDespawn += ExplodeOnDespawn;
	}

	void ExplodeOnHit(Entity hitted, Vector2 pos) => Exploding(hitExplosion, pos);

	void ExplodeOnDespawn(Vector2 pos) => Exploding(hitExplosion, pos);

	void Exploding(Explosion explosion, Vector2 pos)
	{
		//Create circle cast at given pos with given explosion radius to hit enemy only
		RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, explosion.radius/2, Vector2.zero, 0, EnemyManager.i.enemyLayer);
		//Go through all the enemy has hit
		if(hits.Length > 0) for (int h = 0; h < hits.Length; h++)
		{
			//Scaling explosion damage for hurting this enemy with given contact point AND dont recall hit
			strike.Hurting(Tower.Stats.Scale(strike.damage, explosion.damageScaling), hits[h].collider.gameObject, hits[h].point, false);
		}
	}

	void OnDisable()
	{
		//Remove explode from on hit and on despawn event
		if(hitExplosion.enable) strike.onHit -= ExplodeOnHit; 
		if(despawnExplosion.enable) strike.onDespawn -= ExplodeOnDespawn;
	}
}