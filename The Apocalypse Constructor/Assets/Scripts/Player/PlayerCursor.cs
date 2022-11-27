using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
	Camera cam;
    Vector2 mousePos, mouseCoord, preCoord;
	Tower hoverTower; 
	public Structure[] structureHovered = new Structure[0];
	public System.Action onHover;
	public StructurePreview structurePreview; [System.Serializable] public class StructurePreview
	{
		public PlayerCursor pCursor;
		public SpriteRenderer render;
		public Color emptyColor;
		public Sprite defaultSprite;
		public Color defaultColor;
		public Tower previewTower;
		public Combat_Aiming previewAim;

		//Refresh the show tower range of preview tower
		public void RefreshRange() {pCursor.ShowTowerRange(previewAim, previewTower, pCursor.selectFlip);}
		public void PreviewingRange()
		{
			//Get how many structure currently hover
			int hL = pCursor.structureHovered.Length;
			//Refresh range if hover over nothing OR only hover over an platform
			if(hL <= 0 || (hL == 1 && pCursor.HoverPlatfrom(pCursor.structureHovered[0]))) RefreshRange();
		}
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
		//When press use item key
		if(Input.GetKeyDown(KeyOperator.i.UseItem))
		{
			//Use the stash in inventory at mouse coordinate and flip
			Inventory.i.Use(mouseCoord, selectFlip);
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
			structurePreview.RefreshRange();
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
		//When press flip key and inventory has select something
		if(Input.GetKeyDown(KeyOperator.i.FlipStructure) && select != null) 
		{
			//Toggle between flip
			selectFlip = !selectFlip;
			//Flip the preview render to be currently flipped
			structurePreview.render.transform.rotation = Quaternion.Euler(0,(selectFlip)? 180 : 0,0);
			//Begin previewing range
			structurePreview.PreviewingRange();
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
				///If hover over an filler
				if(structure.function == Structure.Function.filler)
				{
					//But the filler are an platform while preview an tower aim
					if(HoverPlatfrom(structure) && structurePreview.previewAim != null)
					{
						//Refresh preview tower's range
						structurePreview.RefreshRange();
					}
				}
				/// If hover over an tower
				if(structure.function == Structure.Function.tower)
				{
					//Get the tower currently being hover 
					hoverTower = hovered.collider.GetComponent<Tower>();
					//Get the aim of tower hovered
					Combat_Aiming hoverAim = hoverTower.GetComponent<Combat_Aiming>();
					//Show range of the tower hover over with it flip
					ShowTowerRange(hoverAim, hoverTower, hoverTower.flipped);
				}
			}
		}
		//No longer hover over any structure 
		else
		{
			structureHovered = new Structure[0];
			hoverTower = null;
		}
		//Call on hover event
		onHover?.Invoke();
	}

	//Function to check if hover over an platform
	bool HoverPlatfrom(Structure s) {return s.stashed.occupation == Stash.Occupation.platform;}

	void ChangePreview(Stash selected)
	{
		//No longer preview any tower aim
		structurePreview.previewAim = null;
		//If inventory has select an stash
		if(selected != null)
		{
			//Preview render the selected stas icon 
			structurePreview.render.sprite = selected.icon;
			//todo: change color below base on hover occupation
			//Change preview color to empty color
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
		//Begin previewing range
		structurePreview.PreviewingRange();
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

	void OnDisable()
	{
		Inventory.i.onSelect -= ChangePreview;
	}
}