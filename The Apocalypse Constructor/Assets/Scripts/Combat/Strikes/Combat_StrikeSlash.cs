using UnityEngine;

public class Combat_StrikeSlash : Combat_Strike
{
    
	public float length, width, forward;
	public int piercing; int pierced;
	public float duration;
	[SerializeField] bool hitHouse;
	Vector2 slashPos;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Reset pierced count
		pierced = piercing;
		//Begin scan if caster has been assign
		if(caster != null) Scan();
		//Move forward from current position to get slash pos
		slashPos = transform.position + (transform.right * forward);
	}

	void Scan()
	{
		//Create an circle cast at this object with width toward it right direction with set length
		RaycastHit2D[] hits = Physics2D.CircleCastAll(slashPos, width/2, transform.right, length);
		//Go through all the object got hit
		if(hits.Length > 0) for (int h = 0; h < hits.Length; h++)
		{
			//Save this object hit
			RaycastHit2D hit = hits[h];
			//Will his hit end the scan
			bool isEnded = false;
			//Hit the enemy when combat layer are on enemy
			if(caster.combatLayer == LayerMask.GetMask("Enemy")) {SlashEnemy(hit, out isEnded);}
			//Hit the structure when combat layer are on enemy
			if(caster.combatLayer == LayerMask.GetMask("Structure")) {SlashStructure(hit, out isEnded);}
			//Start to hit house if needed
			if(hitHouse) SlashHouse(hit, out isEnded);
			//Stop if scan has ended
			if(isEnded) return;
		}
		//Over at slash position with set duration
		Over(slashPos, duration);
	}

	void SlashEnemy(RaycastHit2D hit, out bool ended)
	{
		//If collide with an enemy
		if(hit.collider.CompareTag("Enemy"))
		{
			//Slash to check if has ended
			ended = Slashing(hit);
		}
		//This hit havent end scan
		ended = false; 
	}

	void SlashStructure(RaycastHit2D hit, out bool ended)
	{
		//If collide with any function structure
		if(hit.collider.CompareTag("Filler") || hit.collider.CompareTag("Dynamo") || hit.collider.CompareTag("Tower"))
		{
			//Slash to check if has ended
			ended = Slashing(hit);
		}
		//This hit havent end scan
		ended = false; 
	}

	void SlashHouse(RaycastHit2D hit, out bool ended)
	{
		//If collide with an house
		if(hit.collider.CompareTag("House"))
		{
			//Slash to check if has ended
			ended = Slashing(hit);
		}
		//This hit havent end scan
		ended = false; 
	}

	
	bool Slashing(RaycastHit2D hit)
	{
		//Hurt the hit given with scan point with raw damage
		Hurting(damage, hit.collider.gameObject, hit.point);
		//Lose an pierce and when out of it
		pierced--; if(pierced <= 0) 
		{
			//Over at slash position with set duration
			Over(slashPos, duration);
			//This hit will ended scan
			return true;
		}
		//Can keep to scan
		return false;
	}
}