using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    Vector2 mousePos, mouseCoord;
	Camera cam;
	[Header("Building")]
	[SerializeField] Transform hoverSnap;
	[Header("Tower")]
	[SerializeField] Transform hoverTower; Aiming hoverAim;
	[SerializeField] Transform circleRange, rectangleRange;

	void Start()
	{
		//Get the main camera
		cam = Camera.main;
	}

	void MousePositioning()
	{
		//Get the the mouse position
		mousePos = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
		//Snap the current mouse position to map
		mouseCoord = Map.SnapPosition(mousePos);
	}

	void Update()
	{
		MousePositioning();
		TowerInteract();
		//Make the hover snap follow current mouse coordinates
		hoverSnap.position = mouseCoord;
		//todo: Use Slot Keybind to use inventory at current mouse coordinate
		if(Input.GetKeyDown(KeyCode.Mouse0)) Inventory.i.Use(mouseCoord);
	}

	void TowerInteract()
	{
		//Cast an ray at mouse position on tower layer
		RaycastHit2D hover = Physics2D.Raycast(mousePos, Vector2.zero, 0, StructureManager.i.towerLayer);
		//If hover over an tower
		if(hover) 
		{
			//If the tower hover over are an new one
			if(hover.collider.transform != hoverTower)
			{
				//Are now hover over this tower
				hoverTower = hover.collider.transform;
				//Get the aiming component of new tower got hover
				hoverAim = hoverTower.GetComponent<Aiming>();
			}
			//If aim mode of hover tower are direct
			if(hoverAim.mode == Aiming.Mode.Direct)
			{
				//Adjust hover X poisition by increase it with half of tower range and half spacing size
				float adjust = hoverTower.position.x + ((hoverAim.tower.range/2) + (Map.i.spacing/2));
				//Flip the adjust position if tower are flipped
				if(hoverAim.tower.flipped) adjust = -adjust;
				//Set range position X to be adjusted and Y to be hover tower position
				rectangleRange.position = new Vector2(adjust, hoverTower.position.y);
				//Set range scale width to be tower range and height to be an spacing
				rectangleRange.localScale = new Vector2(hoverAim.tower.range, Map.i.spacing);

			}
			//If aim mode of hover tower are rotate and aimless mode
			else if(hoverAim.mode == Aiming.Mode.Rotate || hoverAim.mode == Aiming.Mode.Aimless)
			{
				//Move circle range to hover tower position
				circleRange.position = hoverTower.position;
				//Circle range size will be double size of tower range
				circleRange.localScale = new Vector2(hoverAim.tower.range*2, hoverAim.tower.range*2);
			}
		}
		//If not hover over any tower
		else
		{
			//Set both range size to zero
			rectangleRange.localScale = Vector2.zero;
			circleRange.localScale = Vector2.zero;
		}
	}
}