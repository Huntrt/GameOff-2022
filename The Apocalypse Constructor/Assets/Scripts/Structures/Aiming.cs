using UnityEngine;

public class Aiming : MonoBehaviour
{
	Tower tower;
	public Mode mode; public enum Mode {Direct, Dynamic, Rotate, Aimless}
	[HideInInspector] public int direction;
	[HideInInspector] public Transform rotationObject;
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
		//@ Deicide which aim mode gonna base on what has choose
		if(mode == Mode.Direct) DirectAim();
		else if(mode == Mode.Dynamic) DynamicAim();
		else if(mode == Mode.Rotate) RotateAim();
		else if(mode == Mode.Aimless) AimlessAim();
	}

	void DirectAim()
	{

	}

	void DynamicAim()
	{
		
	}

	void RotateAim()
	{
		
	}

	void AimlessAim()
	{
		
	}
}