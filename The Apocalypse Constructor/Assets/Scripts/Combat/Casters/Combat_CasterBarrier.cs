using UnityEngine;

public class Combat_CasterBarrier : Combat_Caster
{
	public GameObject strikePrefab;
	[SerializeField] Transform barrier;
	public Amount[] amount;
	[System.Serializable] public class Amount {public float delay;}
	int repeated;

	public override void FlipCaster(bool isFlip)
	{
		base.FlipCaster(isFlip);
		//Adjust the barrier Y to the negative if caster is flipped
		float flipAdjust = barrier.localPosition.y * ((isFlip) ? -1f : 1f); 
		//Move this barrier position to be adjust
		barrier.localPosition = new Vector2(barrier.localPosition.x, flipAdjust);
	}

	protected override void Attack()
	{
		base.Attack();
		//Reset the amount has repeat
		repeated -= repeated;
		//Begin repeating amount gonna strike
		RepeatAmount();
	}

	void RepeatAmount()
	{
		//Strike randomly at barrier length along with it rotation;
		Striking(strikePrefab, GetBarrierLength(), barrier.rotation);
		//Has repeat and count it, exit if repeat enough amount 
		repeated++; if(repeated >= amount.Length) return;
		//Repat again with the current repeat delay
		Invoke("RepeatAmount", amount[repeated].delay);
	}
	
	Vector2 GetBarrierLength()
	{
		//Randomize from the top to bottom of barrier scale height
		float length = Random.Range(-barrier.localScale.y, barrier.localScale.y);
		//Get position upward from barrier with length has randomize
		return barrier.TransformPoint(Vector2.up * length);
	}
}