using UnityEngine;

public class Tower_StrikeHitscan : Tower_Strike
{
	public float length, width;
	public int piercing; int pierced;
	public bool phasing;
	public LineRenderer render;
	public Animation lineAnimation;
	public LineDrawer lineDrawer; [System.Serializable] public class LineDrawer
	{
		public float duration;
		public Color start, end;
		public float expand;
		[HideInInspector] public float fadeCount;
	}
	Vector2 endPoint;

	protected override void OnEnable()
	{
		base.OnEnable();
		pierced = piercing;
		Scan();
		//@ Set the line start and end position
		render.SetPosition(0, transform.position);
		render.SetPosition(1, endPoint);
		//Play the animtion for line if has any
		if(lineAnimation != null) lineAnimation.Play();
		//Reset line drawer value
		lineDrawer.fadeCount = 0;
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
			//If collide with either an fill or dynamo structure without phasing
			if((hit.collider.CompareTag("Filler") || hit.collider.CompareTag("Dynamo")) && !phasing)
			{
				//End scan at this contact point
				endPoint = hit.point;
				//Despawn at this contact point with drawer duration
				Despawn(hit.point, lineDrawer.duration);
				return;
			}
			//If collide with an enemy
			if(hit.collider.CompareTag("Enemy"))
			{
				//Hurt the enemy that got hit with scan point
				Hurting(hit.collider.gameObject, hit.point);
				//Lose an pierce and when out of it
				pierced--; if(pierced <= 0) 
				{
					//End scan at this contact point of enemy
					endPoint = hit.point;
					//Despawn at this contact point with drawer duration
					Despawn(hit.point, lineDrawer.duration);
					return;
				}
			}
		}
		//Set end point at the end of length when still able to hit
		endPoint = transform.TransformPoint(Vector2.right * length);
	}

	void DrawLine()
	{
		//Increase fade count
		lineDrawer.fadeCount += Time.deltaTime;
		//Get how many percent has fade of total duration allow
		float faded = lineDrawer.fadeCount / lineDrawer.duration;
		//Lerp between color and width has faded
		Color colorLerp = Color.Lerp(lineDrawer.start, lineDrawer.end, faded);
		float widthLerp = Mathf.Lerp(width, lineDrawer.expand, faded);
		//Set color and side according to the lerped amount
		render.startColor = colorLerp; render.endColor = colorLerp;
		render.startWidth = widthLerp; render.endWidth = widthLerp;
	}
}