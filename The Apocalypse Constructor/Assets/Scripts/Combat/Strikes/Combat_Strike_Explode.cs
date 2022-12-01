using System.Collections.Generic;
using UnityEngine;

public class Combat_Strike_Explode : MonoBehaviour
{
    [HideInInspector] public Combat_Strike strike;
	public Explosion hitExplosion, despawnExplosion;
	[SerializeField] AudioClip explodeSound;
	[System.Serializable] public class Explosion
	{	
		public bool enable;
		public float damageScaling;
		public float radius;
	}

	void Reset() 
	{
		strike = GetComponent<Combat_Strike>();
	}

	void OnEnable()
	{
		//Print error if the object dont has strike for explode
		if(strike == null) Debug.LogError(gameObject.name + " explode mod need to be an strike");
		//Adding explode to on hit and on despawn event
		if(hitExplosion.enable) strike.onHit += ExplodeOnHit; 
		if(despawnExplosion.enable) strike.onDespawn += ExplodeOnDespawn;
	}

	void ExplodeOnHit(Entity hitted, Vector2 pos) {Exploding(hitExplosion, pos);}
	//Only explode when depsawn if havent explode on strike
	void ExplodeOnDespawn(Vector2 pos){Exploding(despawnExplosion, pos);}

	void Exploding(Explosion explosion, Vector2 pos)
	{
		//Play the exploding sound upon exploding
		SessionOperator.i.audios.soundSource.PlayOneShot(explodeSound);
		//Create circle cast at given pos with given explosion radius to hit entity in combat layer only
		RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, explosion.radius/2, Vector2.zero, 0, strike.caster.combatLayer);
		//Go through all the enemy has hit
		if(hits.Length > 0) for (int h = 0; h < hits.Length; h++)
		{
			//Scaling explosion damage to hurting this entity with given contact point AND dont recall hit
			strike.Hurting(Combats.Stats.Scale(strike.damage, explosion.damageScaling), hits[h].collider.gameObject, hits[h].point, false);
		}
	}

	void OnDisable()
	{
		//Remove explode from on hit and on despawn event
		if(hitExplosion.enable) strike.onHit -= ExplodeOnHit; 
		if(despawnExplosion.enable) strike.onDespawn -= ExplodeOnDespawn;
	}
}