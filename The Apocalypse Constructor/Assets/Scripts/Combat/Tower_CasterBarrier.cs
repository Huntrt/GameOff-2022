using System.Collections;
using UnityEngine;

public class Tower_CasterBarrier : Tower_Caster
{
	public GameObject strikePrefab;
	[SerializeField] Transform barrier;
	public Amount[] amount;
	[System.Serializable] public class Amount {public float delay;}

	protected override void Attack()
	{
		StartCoroutine("LoopAmount");
	}

	IEnumerator LoopAmount()
	{
		//Go through all the points to strike
		for (int p = 0; p < amount.Length; p++)
		{
			//Strike randomly at barrier length along with it rotation;
			Striking(strikePrefab, GetBarrierLength(), barrier.rotation);
			//Wait for the delay of this point
			yield return new WaitForSeconds(amount[p].delay);
		}
	}
	
	Vector2 GetBarrierLength()
	{
		//Randomize from the top to bottom of barrier scale height
		float length = Random.Range(-barrier.localScale.y,barrier.localScale.y)/4;
		//Rotate barrier's anchor angle to emulate X rotation then convert it to radians
		float radians = (barrier.parent.localEulerAngles.z + 90) * Mathf.Deg2Rad;
		//Increase current position with radomize length at rad has get then return it
		return (Vector2)barrier.position + (new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * length);
	}
}