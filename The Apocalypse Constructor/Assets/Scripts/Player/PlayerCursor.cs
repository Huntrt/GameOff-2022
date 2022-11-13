using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    Vector2 mousePos, mouseCoord;
	Camera cam;
	public StructurePreview structurePreview; [System.Serializable] public class StructurePreview
	{
		public SpriteRenderer render;
		public Color emptyColor;
		public Sprite defaultSprite;
		public Color defaultColor;
		public Aiming previewAim;
	}
	[SerializeField] Transform circleRange, rectangleRange;
	Transform hoverTower;
	bool selectFlip = true;
	Stash invSelect;

	void Start()
	{
		//Get the main camera
		cam = Camera.main;
		//Preview no structure when start
		ChangePreview(null);
		//Preview structure when ever inventory select change
		Inventory.i.onSelect += ChangePreview;
	}

	void Update()
	{
		MousePositioning();
		StructureInteract();
		StructurePreviewing();
		//todo: When press flip keybind then toggle select flip only when inventory select something
		if(Input.GetKeyDown(KeyCode.R) && invSelect != null) selectFlip = !selectFlip;
		//todo: Use Slot Keybind to use inventory at current mouse coordinate and select flip
		if(Input.GetKeyDown(KeyCode.Mouse0)) Inventory.i.Use(mouseCoord, selectFlip);
	}
	
	void MousePositioning()
	{
		//Get the the mouse position
		mousePos = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
		//Snap the current mouse position to map
		mouseCoord = Map.SnapPosition(mousePos);
		//Make the preview follow current mouse coordinates
		structurePreview.render.transform.position = mouseCoord;
	}

	void StructureInteract()
	{
		//Cast an ray at mouse position on tower layer
		RaycastHit2D tower = Physics2D.Raycast(mousePos, Vector2.zero, 0, StructureManager.i.towerLayer);
		///If hover over an tower
		if(tower) 
		{
			//Hide the structure preview
			structurePreview.render.enabled = false;
			//If the tower hover over are an new one
			if(tower.collider.transform != hoverTower)
			{
				ResetTowerRange();
				//Are now hover over this tower
				hoverTower = tower.collider.transform;
				//Get the aim of tower hovered
				Aiming hoverAim = hoverTower.GetComponent<Aiming>();
				//Show tower range at tower got hover with aiming and flip of tower hover
				ShowTowerRange(hoverTower.position, hoverAim, hoverAim.tower.flipped);
			}
		}
		//No longer hover over any tower 
		else hoverTower = null;
	}

	void StructurePreviewing()
	{
		//todo: optimize this to only pewview when mouse coordinate change
		/// Dont review anything if still hover over structure
		if(hoverTower != null) return;
		//Reset tower range
		ResetTowerRange();
		//Show the structure preview
		structurePreview.render.enabled = true;
		//No longer hover over any tower
		hoverTower = null;
		//Flip the preview render to be currently flipped
		structurePreview.render.transform.rotation = Quaternion.Euler(0,(selectFlip)? 180 : 0,0);
		//But currently preview an tower
		if(structurePreview.previewAim != null)
		{
			//Show the tower range at mouse coordinates with preview aim with select flip
			ShowTowerRange(mouseCoord, structurePreview.previewAim, selectFlip);
		}
	}

	void ChangePreview(Stash selected)
	{
		//No longer preview any tower aim
		structurePreview.previewAim = null;
		//Save the inventory selected stash
		invSelect = selected;
		//If inventory has select an stash
		if(selected != null)
		{
			//Preview render the selected satsh icon 
			structurePreview.render.sprite = selected.icon;
			//temp: empty color
			structurePreview.render.color = structurePreview.emptyColor;
			//If select an tower
			if(selected.prefab.CompareTag("Tower"))
			{
				//Get the aiming component at tower currently previwing
				structurePreview.previewAim = selected.prefab.GetComponent<Aiming>();
			}
		}
		//If inventory select no stash
		else
		{
			//Reset preview render and color to default
			structurePreview.render.sprite = structurePreview.defaultSprite;
			structurePreview.render.color = structurePreview.defaultColor;
		}
	}

	void ShowTowerRange(Vector2 pos, Aiming aimed, bool isFlip)
	{
		//If aim mode of given tower are direct
		if(aimed.mode == Aiming.Mode.Direct)
		{
			//Value for adjust the X given position
			float adjust = pos.x;
			//Decrease with half tower range and block if flipeed
			if(isFlip) {adjust -= ((aimed.tower.range/2) + (Map.i.spacing/2));}
			//Increase with half tower range and block if not flipeed
			else {adjust += ((aimed.tower.range/2) + (Map.i.spacing/2));}
			//Set range position X to be adjusted and Y to be given position
			rectangleRange.position = new Vector2(adjust, pos.y);
			//Set range scale width to be tower range and height to be an spacing
			rectangleRange.localScale = new Vector2(aimed.tower.range, Map.i.spacing);

		}
		//If aim mode of given tower are rotate and aimless mode
		else if(aimed.mode == Aiming.Mode.Rotate || aimed.mode == Aiming.Mode.Aimless)
		{
			//Move circle range to given tower position
			circleRange.position = pos;
			//Circle range size will be double size of tower range
			circleRange.localScale = new Vector2(aimed.tower.range*2, aimed.tower.range*2);
		}
	}

	void ResetTowerRange()
	{
		//Reset both range size to zero
		rectangleRange.localScale = Vector2.zero;
		circleRange.localScale = Vector2.zero;
	}

	void OnDisable()
	{
		Inventory.i.onSelect -= ChangePreview;
	}
}