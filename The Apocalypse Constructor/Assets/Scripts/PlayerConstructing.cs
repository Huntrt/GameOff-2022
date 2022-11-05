using UnityEngine;

public class PlayerConstructing : MonoBehaviour
{
    public GameObject crate, turret, roof, snap;
	Camera cam; Vector2 mouseCoord;

	void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		mouseCoord = Map.PositionToCoordinate(cam.ScreenToWorldPoint((Vector2)Input.mousePosition));
		snap.transform.position = mouseCoord;
		if(Input.GetKeyDown(KeyCode.Alpha1)) Map.Placing(turret, mouseCoord, 1);
		if(Input.GetKeyDown(KeyCode.Alpha2)) Map.Placing(roof, mouseCoord, 2);
		if(Input.GetKeyDown(KeyCode.Alpha3)) Map.Placing(crate, mouseCoord, 3);
	}
}
