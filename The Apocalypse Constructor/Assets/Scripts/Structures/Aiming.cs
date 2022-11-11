using UnityEngine;

public class Aiming : MonoBehaviour
{
	Tower tower;
	public Mode mode; public enum Mode {Directional, Dynamic, Pivot, Aimless}
	[HideInInspector] public Transform rotationObject;
	[HideInInspector] public float rotateSpeed;
	[HideInInspector] public int direction;

	void Start()
	{
		//Getting tower from object it on
		tower = GetComponent<Tower>();
		//Print null if the object is not an tower
		if(tower == null) Debug.LogError(gameObject.name + " aiming need to be an tower");
	}

	void Update()
	{
		//@ Deicide which aim mode gonna base on what has choose
		if(mode == Mode.Directional)  DirectionalAim();
		else if(mode == Mode.Dynamic) DynamicAim();
		else if(mode == Mode.Pivot)	  PivotAim();
		else if(mode == Mode.Aimless) AimlessAim();
	}

	void DirectionalAim()
	{

	}

	void DynamicAim()
	{
		
	}

	void PivotAim()
	{
		
	}

	void AimlessAim()
	{
		
	}
}