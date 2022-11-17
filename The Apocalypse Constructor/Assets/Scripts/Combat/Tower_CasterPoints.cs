using System.Collections;
using UnityEngine;

public class Tower_CasterPoints : Tower_Caster
{
	public GameObject strikePrefab;
	public Point[] points;
	[System.Serializable] public class Point {public Transform transform; public float delay;}

	protected override void Attack()
	{
		StartCoroutine("LoopPoints");
	}

	IEnumerator LoopPoints()
	{
		//Go through all the points to strike
		for (int p = 0; p < points.Length; p++)
		{
			//Strike at this point transform and using point rotation
			Striking(strikePrefab, points[p].transform.position, points[p].transform.rotation);
			//Wait for the delay of this point
			yield return new WaitForSeconds(points[p].delay);
		}
	}
}