using UnityEngine;

public class Combat_CasterPoints : Combat_Caster
{
	public GameObject strikePrefab;
	public Point[] points;
	[System.Serializable] public class Point {public Transform transform; public float delay;}
	int repeated;

	public override void InvertingCaster(bool isInvert)
	{
		base.InvertingCaster(isInvert);
		//If caster has been flip then go through all the point
		if(isInvert) for (int p = 0; p < points.Length; p++)
		{
			//Get this point local position
			Vector2 pPos = points[p].transform.localPosition;
			//Adjust this point Y to the absolute negative if caster is flipped
			float flipAdjustY = (isInvert) ? -Mathf.Abs(pPos.y) : pPos.y; 
			//Move this point position to be adjust
			points[p].transform.localPosition = new Vector2(pPos.x, flipAdjustY);
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