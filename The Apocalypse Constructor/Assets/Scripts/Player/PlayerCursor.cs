using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    Vector2 mousePos, mouseCoord, preCoord;
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
	Structure[] structureHovered = new Structure[0];
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
		//todo: When press flip keybind then toggle select flip only when inventory select something
		if(Input.GetKeyDown(KeyCode.R) && invSelect != null) selectFlip = !selectFlip;
		//todo: When press delete keybind then delete structure currently hover
		if(Input.GetKeyDown(KeyCode.A)) DeleteStructure();
		//todo: Use Slot Keybind to use inventory at current mouse coordinate and select flip
		if(Input.GetKeyDown(KeyCode.Mouse0)) Inventory.i.Use(mouseCoord, selectFlip);
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
			//Begin hover and previewing structure
			StructureHovering();
			StructurePreviewing();
			//Has move to an new coordinate
			preCoord = mouseCoord;
		}
	}

	void DeleteStructure()
	{
		//Go through all the structure currently hover and delete each of them off the map
		for (int s = 0; s < structureHovered.Length; s++) Map.Deleting(structureHovered[s]);
	}

	void StructureHovering()
	{
		//Cast an ray at mouse position on structure layer
		RaycastHit2D[] hovers = Physics2D.RaycastAll(mouseCoord,Vector2.zero,0,StructureManager.i.structureLayer);
		///If hover over any structure
		if(hovers.Length > 0)
		{
			//Hide the tower range
			HideTowerRange();
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
					//Are now hover over this tower
					hoverTower = hovered.collider.transform;
					//Get the aim of tower hovered
					Aiming hoverAim = hoverTower.GetComponent<Aiming>();
					//Show tower range at tower got hover with aiming and flip of tower hover
					ShowTowerRange(hoverTower.position, hoverAim, hoverAim.tower.flipped);
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

	void StructurePreviewing()
	{
		/// Dont review anything if still hover over structure
		if(structureHovered.Length > 0) return;
		//Hide the tower range
		HideTowerRange();
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
		//Instantly preview structure
		StructurePreviewing();
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

	void HideTowerRange()
	{
		//Reset both range size to zero
		rectangleRange.localScale = Vector2.zero;
		circleRange.localScale = Vector2.zero;
	}

	void DeleteStructure()
	{
		//Go through all the structure currently hover and delete each of them off the map
		for (int s = 0; s < structureHovered.Length; s++) Map.Deleting(structureHovered[s]);
		//Refresh the structure hovering
		StructureHovering();
	}

	void OnDisable()
	{
		Inventory.i.onSelect -= ChangePreview;
	}
}