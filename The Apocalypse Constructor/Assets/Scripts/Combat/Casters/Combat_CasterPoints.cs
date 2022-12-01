using UnityEngine;

public class Combat_CasterPoints : Combat_Caster
{
	public GameObject strikePrefab;
	public Point[] points;
	[System.Serializable] public class Point 
	{
		public Transform transform; 
		public float delay;
		public bool disableAdjust;
	}
	int repeated;

	public override void InvertingCaster(bool isInvert, bool dontAdjustPoint = false)
	{
		base.InvertingCaster(isInvert);
		//Go through all the point when get invert ONLY WHEN NEEDED
		if(!dontAdjustPoint) for (int p = 0; p < points.Length; p++)
		{
			//Skip if this point dont need adjust
			if(points[p].disableAdjust) continue;
			//Get this point local position
			Vector2 pPos = points[p].transform.localPosition;
			//Adjust this point Y to be opposite of it current 
			float flipAdjustY = -pPos.y;
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