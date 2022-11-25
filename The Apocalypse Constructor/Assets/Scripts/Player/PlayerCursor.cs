using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
	Camera cam;
    Vector2 mousePos, mouseCoord, preCoord;
	Tower hoverTower; 
	public Structure[] structureHovered = new Structure[0];
	public StructurePreview structurePreview; [System.Serializable] public class StructurePreview
	{
		public SpriteRenderer render;
		public Color emptyColor;
		public Sprite defaultSprite;
		public Color defaultColor;
		public Combat_Aiming previewAim;
		public Tower previewTower;
	}
	[SerializeField] Transform circleRange, rectangleRange;
	bool selectFlip;

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
		FlipStructure();
		//todo: When press delete keybind then delete structure currently hover
		if(Input.GetKeyDown(KeyCode.A)) DeleteStructure();
		//todo: Use Slot Keybind to use inventory
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			//Use the stash in inventory at mouse coordinate and flip if not in trash mode
			if(!Inventory.i.trashMode) Inventory.i.Use(mouseCoord, selectFlip);
			//Refresh the structure hovering to hover over structure just use
			StructureHovering();
		}
	}
	
	void MousePositioning()
	{
		//Get the the mouse position
		mousePos = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
		//Snap the current mouse position to map
		mouseCoord = Map.SnapPosition(mousePos);
		//If the mouse has move to an new coordinate
		if(preCoord != mouseCoord)
		{
			//Make the preview follow current mouse coordinates
			structurePreview.render.transform.position = mouseCoord;
			//Refresh preview range when coordinate change
			RefreshPreviewRange();
			//Begin hover over structure
			StructureHovering();
			//Has move to an new coordinate
			preCoord = mouseCoord;
		}
	}

	void FlipStructure()
	{
		//Does inventory select any stash
		Stash select = Inventory.i.selectedStash;
		//todo: When press flip keybind and inventory has select something
		if(Input.GetKeyDown(KeyCode.R) && select != null) 
		{
			//Toggle between flip
			selectFlip = !selectFlip;
			//Flip the preview render to be currently flipped
			structurePreview.render.transform.rotation = Quaternion.Euler(0,(selectFlip)? 180 : 0,0);
			//Refresh pewview range when flip while not hover over any structure
			if(structureHovered.Length <= 0) RefreshPreviewRange();
		}
	}

	void StructureHovering()
	{
		//Cast an ray at mouse coordinate on structure layer
		RaycastHit2D[] hovers = Physics2D.RaycastAll(mouseCoord,Vector2.zero,0,StructureManager.i.structureLayer);
		///If hover over any structure
		if(hovers.Length > 0)
		{
			//Hide the tower range
			HideTowerRange();
			//Stop if hover over the house
			if(hovers[0].collider.CompareTag("House")) return;
			//Renew how many structure being hover
			structureHovered = new Structure[hovers.Length];
			//Go through all the structure being hover
			for (int h = 0; h < hovers.Length; h++)
			{
				//Get this hover
				RaycastHit2D hovered = hovers[h];
				//Get component of this hover structure
				Structure structure = hovered.collider.GetComponent<Structure>();
				//Save this hover structure component
				structureHovered[h] = structure;
				//If hover over an filler
				if(structure.function == Structure.Function.filler)
				{
					///... Do something
				}
				//If hover over an tower
				if(structure.function == Structure.Function.tower)
				{
					//Get the tower currently being hover 
					hoverTower = hovered.collider.GetComponent<Tower>();
					//Get the aim of tower hovered
					Combat_Aiming hoverAim = hoverTower.GetComponent<Combat_Aiming>();
					//Show range of the tower hover over with it flip
					ShowTowerRange(hoverAim, hoverTower, hoverTower.flipped);
				}
				//If hover over an dynamo
				if(structure.function == Structure.Function.dynamo)
				{
					///... Do something
				}
			}
		}
		//No longer hover over any structure 
		else
		{
			structureHovered = new Structure[0];
			hoverTower = null;
		}
	}

	void ChangePreview(Stash selected)
	{
		//No longer preview any tower aim
		structurePreview.previewAim = null;
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
				//Get the aiming and tower component at tower currently previwing
				structurePreview.previewAim = selected.prefab.GetComponent<Combat_Aiming>();
				structurePreview.previewTower = selected.prefab.GetComponent<Tower>();
			}
		}
		//If inventory select no stash
		else
		{
			//Reset preview render and color to default
			structurePreview.render.sprite = structurePreview.defaultSprite;
			structurePreview.render.color = structurePreview.defaultColor;
		}
		//Refresh preview range when not hover over any structure while preview change
		if(structureHovered.Length <= 0) RefreshPreviewRange();
	}

	void RefreshPreviewRange()
	{
		//Show tower range with preview tower and aim with current select flip
		ShowTowerRange(structurePreview.previewAim, structurePreview.previewTower, selectFlip);
	}

 	void ShowTowerRange(Combat_Aiming aimed, Tower towered, bool isFlip)
	{
		//Hide tower range and stop showing if aim dont exist
		HideTowerRange(); if(aimed == null) return;
		//Center point of range will alway be mouse coordinate
		Vector2 pos = mouseCoord;
		//If aim mode of given tower are direct
		if(aimed.mode == Combat_Aiming.Mode.Direct)
		{
			//Value for adjust the X given position
			float adjust = pos.x;
			//Decrease with half tower range and block if flipeed
			if(isFlip) {adjust -= ((towered.stats.range/2) + (Map.i.spacing/2));}
			//Increase with half tower range and block if not flipeed
			else {adjust += ((towered.stats.range/2) + (Map.i.spacing/2));}
			//Set range position X to be adjusted and Y to be given position
			rectangleRange.position = new Vector2(adjust, pos.y);
			//Set range scale width to be tower range and height to be an spacing
			rectangleRange.localScale = new Vector2(towered.stats.range, Map.i.spacing);

		}
		//If aim mode of given tower are rotate and aimless mode
		else if(aimed.mode == Combat_Aiming.Mode.Rotate || aimed.mode == Combat_Aiming.Mode.Aimless)
		{
			//Move circle range to given tower position
			circleRange.position = pos;
			//Circle range size will be double size of tower range
			circleRange.localScale = new Vector2(towered.stats.range*2, towered.stats.range*2);
		}
	}

	void HideTowerRange()
	{
		//Reset both range size to zero
		rectangleRange.localScale = Vector2.zero;
		circleRange.localScale = Vector2.zero;
	}

	void DeleteStructure()
	{
		//Go through all the structure currently hover
		for (int s = 0; s < structureHovered.Length; s++)
		{
			//Refund the leftover ingredients of structure being delete
			Inventory.Refund(structureHovered[s].stashed.Leftovering());
			//Instantly kill the structure wanted to delete
			structureHovered[s].Die();
		}
		
		//Refresh the structure hovering
		StructureHovering();
	}

	void OnDisable()
	{
		Inventory.i.onSelect -= ChangePreview;
	}
}