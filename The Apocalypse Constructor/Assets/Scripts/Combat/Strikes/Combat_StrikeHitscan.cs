using UnityEngine;

public class Combat_StrikeHitscan : Combat_Strike
{
	public float length, width;
	public int piercing; int pierced;
	public bool phasing;
	public LineRenderer render;
	public Animation lineAnimation;
	public LineConfig lineConfig; [System.Serializable] public class LineConfig
	{
		public float duration;
		public Color start, end;
		public float expand;
		[HideInInspector] public float fadeCount;
	}
	Vector2 endPoint;
	[SerializeField] bool hitHouse;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Stop if caster havent been assign
		if(caster == null) return;
		//Reset pierced count
		pierced = piercing;
		//Reset line drawer value
		lineConfig.fadeCount = 0;
		//Play the animtion for line if has any
		if(lineAnimation != null) lineAnimation.Play();
		//Begin hit scan
		Scan();
		//@ Set the line start and end position
		render.SetPosition(0, transform.position);
		render.SetPosition(1, endPoint);
	}

	//Only need to draw line when it dont has animation
	void Update() {if(lineAnimation == null) DrawLine();}

	void Scan()
	{
		//Create an circle cast at this object with width toward it right direction with set length
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, width, transform.right, length);
		//Go through all the object got hit
		if(hits.Length > 0) for (int h = 0; h < hits.Length; h++)
		{
			//Save this object hit
			RaycastHit2D hit = hits[h];
			//Will his hit end the scan
			bool isEnded = false;
			//Hit the enemy when combat layer are on enemy
			if(caster.combatLayer == LayerMask.GetMask("Enemy")) {ScanEnemy(hit, out isEnded);}
			//Hit the structure when combat layer are on enemy
			if(caster.combatLayer == LayerMask.GetMask("Structure")) {ScanStructure(hit, out isEnded);}
			//Start to hit house if needed
			if(hitHouse) ScanHouse(hit, out isEnded);
			//Stop if scan has ended
			if(isEnded) return;
		}
		//? If still able to pierce but out of hit
		//Set end point at the end of length when still able to hit
		endPoint = transform.TransformPoint(Vector2.right * length);
		//Over at end point with drawer duration
		Over(endPoint, lineConfig.duration);
	}

	void ScanEnemy(RaycastHit2D hit, out bool ended)
	{
		//This hit havent end
		ended = false; 
		//If collide with either an fill or dynamo structure without phasing
		if((hit.collider.CompareTag("Filler") || hit.collider.CompareTag("Dynamo")) && !phasing)
		{
			//End scan at this contact point
			endPoint = hit.point;
			//Despawn at this contact point with drawer duration
			Despawn(hit.point, lineConfig.duration);
			//This hit will ended scan
			ended = true; return;
		}
		//If collide with an enemy
		if(hit.collider.CompareTag("Enemy"))
		{
			//Hit scanning to check will it end
			ended = HitScaning(hit);
			//Stop if has end scan
			if(ended) return;
		}
	}

	void ScanStructure(RaycastHit2D hit, out bool ended)
	{
		//This hit havent end
		ended = false; 
		//If collide with any function structure
		if(hit.collider.CompareTag("Filler") || hit.collider.CompareTag("Dynamo") || hit.collider.CompareTag("Tower"))
		{
			//Hit scanning to check will it end
			ended = HitScaning(hit);
		}
	}

	void ScanHouse(RaycastHit2D hit, out bool ended)
	{
		//This hit havent end
		ended = false; 
		//If collide with an house
		if(hit.collider.CompareTag("House"))
		{
			//Hit scanning to check will it end
			ended = HitScaning(hit);
			//Stop if has end scan
			if(ended) return;
		}
	}

	bool HitScaning(RaycastHit2D hit)
	{
		//Hurt the hit given with scan point with raw damage
		Hurting(damage, hit.collider.gameObject, hit.point);
		//Lose an pierce and when out of it
		pierced--; if(pierced <= 0) 
		{
			//End scan at this contact point of structure
			endPoint = hit.point;
			//Over at this contact point with drawer duration
			Over(hit.point, lineConfig.duration);
			//This hit will ended scan
			return true;
		}
		//Can keep to scan
		return false;
	}

	void DrawLine()
	{
		//Increase fade count
		lineConfig.fadeCount += Time.deltaTime;
		//Get how many percent has fade of total duration allow
		float faded = lineConfig.fadeCount / lineConfig.duration;
		//Lerp between color and width has faded
		Color colorLerp = Color.Lerp(lineConfig.start, lineConfig.end, faded);
		float widthLerp = Mathf.Lerp(width, lineConfig.expand, faded);
		//Set color and side according to the lerped amount
		render.startColor = colorLerp; render.endColor = colorLerp;
		render.startWidth = widthLerp; render.endWidth = widthLerp;
	}
}