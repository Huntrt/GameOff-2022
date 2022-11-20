using UnityEngine;

public class Aiming : MonoBehaviour
{
	[HideInInspector] public Tower tower;
	public Mode mode; public enum Mode {Direct, Rotate, Aimless}
	[HideInInspector] public Vector2 direction;
	[HideInInspector] public Transform rotationAnchor;
	[HideInInspector] public float rotateSpeed;
	[HideInInspector] public SpriteRenderer shooterRender;
	[HideInInspector] public bool getFirstAsShooter;

	void Reset() 
	{
		//Get the tower component the moment aim get added
		tower = GetComponent<Tower>();
		//Print error if the object dont has tower for aim
		if(tower == null) Debug.LogError(gameObject.name + " aiming need to be an tower");
	}

	void OnValidate()
	{
		//If needed to get the first child as shooter wild in rotate mode
		if(getFirstAsShooter && mode == Mode.Rotate)
		{	
			//Get first child sprite render as shooter renderer
			shooterRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
		}
	}

	void Update()
	{
		//Tower havent detect anything
		tower.detected = false;
		//Dont aim when tower insufficient of energy
		if(tower.insufficient) return;
		//@ Deicide which aim mode gonna base on what has choose
		if(mode == Mode.Direct) DirectAim();
		else if(mode == Mode.Rotate) RotateAim();
	}

	void DirectAim()
	{
		//Flip the aim direction if the tower been flip
		Vector2 dir = direction; if(tower.flipped) dir = -dir;
		//Create an pillar at this tower to range with radius of an spacing toward set direction on enemy layer
		RaycastHit2D hit = Physics2D.CircleCast(transform.position, Map.i.spacing/2, dir, tower.stats.range, EnemyManager.i.enemyLayer);
		//Detect if cast hit an enemy
		if(hit) tower.detected = true;
	}

	void RotateAim()
	{
		//Create an circle cast at this tower with radius as it range and only cast on enemy layer
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, tower.stats.range, Vector2.zero,0, EnemyManager.i.enemyLayer);
		//If cast hit anything
		if(hits.Length > 0)
		{
			//Tower has detect an enemy
			tower.detected = true;
			//Dont need to rotate if using aimless mode
			if(mode == Mode.Aimless) return;
			//Getet the closest enemy that got hit by cast
			GameObject detect = EnemyManager.Closest(transform.position, hits);
			//Makt the anchor rotate toward closest enemy detected
			rotationAnchor.right = (detect.transform.position - transform.position).normalized;
			//Stop if has no shooter render to invert
			if(shooterRender == null) return;
			//If the detect enemy are infront tower
			if(detect.transform.position.x > transform.position.x)
			{
				//Dont invenrt
				shooterRender.flipY = false;
			}
			//If the detect enemy are behind tower
			else
			{
				//Invert
				shooterRender.flipY = true;
			}
		}
		
	}
}