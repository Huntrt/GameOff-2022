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
		float length = Random.Range(-barrier.localScale.y,barrier.localScale.y)/6;
		//Get position inside barrier with length has randomize
		return barrier.TransformPoint(Vector2.up * length);
	}
}