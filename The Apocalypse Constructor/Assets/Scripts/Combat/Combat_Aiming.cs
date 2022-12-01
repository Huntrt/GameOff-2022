using UnityEngine;

public class Combat_Aiming : MonoBehaviour
{
	public Combat_Caster caster;
	public Mode mode; public enum Mode {Direct, Rotate, Aimless}
	[HideInInspector] public Vector2 direction;
	[HideInInspector] public Transform rotationAnchor;
	[HideInInspector] public float rotateSpeed;
	[HideInInspector] public SpriteRenderer shooterRender;
	[HideInInspector] public bool getFirstAsShooter;

	void OnValidate()
	{
		//If needed to get the first child as shooter wild in rotate mode
		if(getFirstAsShooter && mode == Mode.Rotate)
		{	
			//Get first child sprite render as shooter renderer
			shooterRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
		}
		if(caster == null)
		{
			//Get the caster component the moment aim get added
			caster = GetComponent<Combat_Caster>();
			//Print error if the object dont has caster ti aim
			if(caster == null) Debug.LogError(gameObject.name + " aiming need to be an caster");
		}
	}

	void Update()
	{
		//Tower havent detect anything
		caster.detected = false;
		//Dont aim when caster is deactived
		if(!caster.isActiveAndEnabled) return;
		//@ Deicide which aim mode gonna base on what has choose
		if(mode == Mode.Direct) DirectAim();
		else if(mode == Mode.Rotate) RotateAim();
	}

	void DirectAim()
	{
		//Flip the aim direction if the caster been flip
		Vector2 dir = direction; if(caster.Inverted) dir = -dir;
		//Create an pillar at this caster to range with radius of an spacing toward set direction on combat layer
		RaycastHit2D hit = Physics2D.CircleCast(transform.position, Map.i.spacing/2, dir, caster.finalStats.range, caster.combatLayer);
		//Detect if cast hit an enemy
		if(hit) caster.detected = true;
	}

	void RotateAim()
	{
		//Create an circle cast at this caster with radius as it range and only cast on the combat layer
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, caster.finalStats.range, Vector2.zero,0, caster.combatLayer);
		//If cast hit anything
		if(hits.Length > 0)
		{
			//Caster has detect an enemy
			caster.detected = true;
			//Dont need to rotate if using aimless mode
			if(mode == Mode.Aimless) return;
			//Get the closest enemy that got hit by cast
			GameObject detect = GetCloset.Ray(transform.position, hits);
			//Make the anchor rotate toward closest enemy detected
			rotationAnchor.right = (detect.transform.position - transform.position).normalized;
			//Check does detected enemy are from behind of enemy to be aim backward or not
			bool aimBackward = detect.transform.position.x < transform.position.x;
			//Flip the caster to base on aim backward only once
			if(caster.Inverted != aimBackward) caster.InvertingCaster(aimBackward);
			//Flip the shooter render sprite Y if aim backward 
			if(shooterRender != null) shooterRender.flipY = aimBackward;
		}
	}
}