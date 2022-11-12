using UnityEngine;

public class Aiming : MonoBehaviour
{
	Tower tower;
	public Mode mode; public enum Mode {Direct, Dynamic, Rotate, Aimless}
	[HideInInspector] public int direction;
	[HideInInspector] public Transform rotationAnchor;
	[HideInInspector] public float rotateSpeed;

	void Start()
	{
		//Getting tower from object it on
		tower = GetComponent<Tower>();
		//Print null if the object is not an tower
		if(tower == null) Debug.LogError(gameObject.name + " aiming need to be an tower");
	}

	void Update()
	{
		//Tower havenlt detect anything
		tower.detected = false;
		//@ Deicide which aim mode gonna base on what has choose
		if(mode == Mode.Direct) DirectAim();
		else if(mode == Mode.Dynamic) DynamicAim();
		else if(mode == Mode.Rotate) RotateAim();
		else if(mode == Mode.Aimless) AimlessAim();
	}

	void DirectAim()
	{

	}

	void DynamicAim() {}

	void RotateAim()
	{
		//Create an circle cast at this tower with radius as it range and only cast on enemy layer
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, tower.range, Vector2.zero,0, EnemyManager.i.enemyLayer);
		//If cast hit anything
		if(hits.Length > 0)
		{
			//Getet the closest enemy that got hit by cast
			GameObject detect = EnemyManager.Closest(transform.position, hits);
			//Makt the anchor rotate toward closest enemy detected
			rotationAnchor.right = (detect.transform.position - transform.position).normalized;
			//Tower has detect an enemy
			tower.detected = true;
		}
		
	}

	void AimlessAim() {}
}