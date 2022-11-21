using UnityEngine;

public class Sighting : MonoBehaviour
{
	[HideInInspector] public Enemy_Move mover;
	public Mode mode; public enum Mode {Direct, Rotate, Aimless}
	[HideInInspector] public Vector2 direction;
	[HideInInspector] public Transform rotationAnchor;
	[HideInInspector] public float rotateSpeed;
	Transform indicator;
    
	void Reset()
	{
		//Get the move component the moment sight get added
		mover = GetComponent<Enemy_Move>();
		//Print error if the object dont has move for 
		if(mover == null) Debug.LogError(gameObject.name + " sighting need to be an move");
	}

	void OnEnable()
	{
		SetupSightIndicator();
	}

	void SetupSightIndicator()
	{
		//@ Choose the indicator base on sighting mode
		if(mode == Mode.Direct) indicator = EnemyManager.i.directSightIndicator.transform;
		else if(mode == Mode.Rotate)  indicator = EnemyManager.i.rotateSightIndicator.transform;
		//Create the indicator has choose at this enemy position
		indicator = Instantiate(indicator, transform.position, Quaternion.identity).transform;
		//Parent the indicator onto enemy
		indicator.SetParent(transform);
		//If indicator are direct mode
		if(mode == Mode.Direct)
		{
			//Set the indicator scale as mover vision
			indicator.localScale = new Vector2(mover.vision, Map.i.spacing);
			//Adjust the indicator vision by moving it half of vision and block size
			indicator.localPosition = new Vector2(((mover.vision/2) + (Map.i.spacing/2)), 0);
		}
		//If indicator are rotate node then indicator radius are double mover vision
		else if(mode == Mode.Rotate) indicator.transform.localScale = new Vector2(mover.vision*2, mover.vision*2);
	}
	
	void Update()
	{
		//Tower havent encounter anything
		mover.encounter = false;
		//@ Deicide which aim mode gonna base on what has choose
		if(mode == Mode.Direct) DirectSight();
		else if(mode == Mode.Rotate) RotateSight();
	}

	void DirectSight()
	{
		//Flip the aim direction if the mover are turn over
		Vector2 dir = direction; if(mover.turnover) dir = -dir;
		//Create an pillar at this move vision with radius of an spacing toward set direction on structure layer
		RaycastHit2D hit = Physics2D.CircleCast(transform.position, Map.i.spacing/2, dir, mover.vision, StructureManager.i.structureLayer);
		//Encounter if sight saw an structure
		if(hit) mover.encounter = true;
	}

	void RotateSight()
	{
		//Create an circle cast at this mover with vision as it range and only cast on structure layer
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, mover.vision, Vector2.zero,0, StructureManager.i.structureLayer);
		//If cast hit anything
		if(hits.Length > 0)
		{
			//Mover has encounter tower
			mover.encounter = true;
			//Dont need to rotate if using aimless mode
			if(mode == Mode.Aimless) return;
			//Get the closest structure that got hit by cast
			GameObject detect = GetCloset.Ray(transform.position, hits);
			//Make the anchor rotate toward closest structure detected
			rotationAnchor.right = (detect.transform.position - transform.position).normalized;
		}
	}
}