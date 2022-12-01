using UnityEngine;

public class Combat_CasterPoints : Combat_Caster
{
	public GameObject strikePrefab;
	public Point[] points;
	[System.Serializable] public class Point {public Transform transform; public float delay;}
	int repeated;

	public override void FlipCaster(bool isFlip)
	{
		base.FlipCaster(isFlip);
		//If caster has been flip then go through all the point
		if(isFlip) for (int p = 0; p < points.Length; p++)
		{
			//Get this point local position
			Vector2 pPos = points[p].transform.localPosition;
			//Adjust this point Y to the negative if caster is flipped
			float flipAdjust = pPos.y * ((isFlip) ? -1f : 1f); 
			//Move this point position to be adjust
			points[p].transform.localPosition = new Vector2(pPos.x, flipAdjust);
		}
	}

	protected override void Attack()
	{
		base.Attack();
		//Reset the amount has repeat
		repeated -= repeated;
		//Begin repeating point gonna strike
		RepeatPoints();
	}

	void RepeatPoints()
	{
		//Strike at this point transform and using point rotation
		Striking(strikePrefab, points[repeated].transform.position, points[repeated].transform.rotation);
		//Has repeat and count it, exit if repeat enough amount 
		repeated++; if(repeated >= points.Length) return;
		//Repat again with the current repeat delay
		Invoke("RepeatPoints", points[repeated].delay);
	}
}