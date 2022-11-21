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
			if(caster.combatLayer == LayerMask.GetMask("Enemy")) {HitEnemy(hit, out isEnded);}
			//Hit the structure when combat layer are on enemy
			if(caster.combatLayer == LayerMask.GetMask("Structure")) {HitStructure(hit, out isEnded);}
			//Start to hit house if needed
			if(hitHouse) HitHouse(hit, out isEnded);
			//Stop if scan has ended
			if(isEnded) return;
		}
		SlashOver();
	}

	void HitEnemy(RaycastHit2D hit, out bool ended)
	{
		//If collide with an enemy
		if(hit.collider.CompareTag("Enemy"))
		{
			//Hurt the enemy that got hit with scan point with raw damage
			Hurting(damage, hit.collider.gameObject, hit.point);
			//Lose an pierce and when out of it
			pierced--; if(pierced <= 0) 
			{
				SlashOver();
				//This hit will ended scan
				ended = true; return;
			}
		}
		//This hit havent end scan
		ended = false; 
	}

	void HitStructure(RaycastHit2D hit, out bool ended)
	{
		//If collide with any function structure
		if(hit.collider.CompareTag("Filler") || hit.collider.CompareTag("Dynamo") || hit.collider.CompareTag("Tower"))
		{
			//Hurt the structure that got hit with scan point with raw damage
			Hurting(damage, hit.collider.gameObject, hit.point);
			//Lose an pierce and when out of it
			pierced--; if(pierced <= 0) 
			{
				SlashOver();
				//This hit will ended scan
				ended = true; return;
			}
		}
		//This hit havent end scan
		ended = false; 
	}

	void HitHouse(RaycastHit2D hit, out bool ended)
	{
		//If collide with an house
		if(hit.collider.CompareTag("House"))
		{
			//Hurt the house that got hit with scan point with raw damage
			Hurting(damage, hit.collider.gameObject, hit.point);
			//Lose an pierce and when out of it
			pierced--; if(pierced <= 0) 
			{
				SlashOver();
				//This hit will ended scan
				ended = true; return;
			}
		}
		//This hit havent end scan
		ended = false; 
	}

	public void SlashOver()
	{
		//Over at current position with set duration
		Over(slashPos, duration);
	}
}