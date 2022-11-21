using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
	public Rigidbody2D rb;
	public bool turnover;
	public bool encounter;
	public float vision;
	public float speed;

	protected virtual void OnEnable()
	{
		//Turn over if spawn on the right side of map
		if(transform.position.x > 0) TurnoverMover(true);
	}

	public void TurnoverMover(bool turn)
	{
		print(transform.eulerAngles.z);
		//Set turn over as given turn
		turnover = turn;
		//If turn over then rotate the enemy in X and Y axis 180
		if(turn) transform.rotation = Quaternion.Euler(0,180,transform.eulerAngles.z);
	}
}