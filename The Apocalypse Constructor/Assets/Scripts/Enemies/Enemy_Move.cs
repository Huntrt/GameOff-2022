using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
	[SerializeField] protected Combat_Caster caster;
	public Rigidbody2D rb;
	public float movementSpeed;
	public bool turnover;

	void Reset() 
	{
		caster = GetComponent<Combat_Caster>();
	}

	protected virtual void OnEnable()
	{
		//Turn over if spawn on the right side of map
		if(transform.position.x > 0) TurnoverMover(true);
		//Flip the caster if move been turnover
		caster.flipped = turnover;
	}

	public void TurnoverMover(bool turn)
	{
		//Set turn over as given turn
		turnover = turn;
		//If turn over then rotate the enemy in X and Y axis 180
		if(turn) transform.rotation = Quaternion.Euler(0,180,transform.eulerAngles.z);
	}
}