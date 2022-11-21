using UnityEngine;

public class Combat_CasterPoints : Combat_Caster
{
	public GameObject strikePrefab;
	public Point[] points;
	[System.Serializable] public class Point {public Transform transform; public float delay;}
	int repeated;

	protected override void Attack()
	{
		//Reset the amount has repeat
		repeated -= repeated;
		//Begin repeating amount gonna strike
		Invoke("RepeatPoints", 0);
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