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

	void Start()
	{
		//Get the main camera
		cam = Camera.main;
		//Preview no structure when start
		StructurePreviewing(null);
		//Preview structure when ever inventory select change
		Inventory.i.onSelect += StructurePreviewing;
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

	void Update()
	{
		MousePositioning();
		TowerInteract();
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
				ResetTowerRange();
				//Are now hover over this tower
				hoverTower = hover.collider.transform;
				//Show tower range at tower got hover with aiming of tower hover
				ShowTowerRange(hoverTower.position, hoverTower.GetComponent<Aiming>());
			}
		}
		//If not hover over any tower
		else
		{
			ResetTowerRange();
			//No longer hover over any tower
			hoverTower = null;
			//But currently preview an tower
			if(structurePreview.previewAim != null)
			{
				//Show the tower range at mouse coordinates with preview aim
				ShowTowerRange(mouseCoord, structurePreview.previewAim);
			}
		}
	}

	void StructurePreviewing(Stash selected)
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

	void ShowTowerRange(Vector2 pos, Aiming aimed)
	{
		//If aim mode of given tower are direct
		if(aimed.mode == Aiming.Mode.Direct)
		{
			//Adjust given X poisition by increase it with half of tower range and half spacing size
			float adjust = pos.x + ((aimed.tower.range/2) + (Map.i.spacing/2));
			//Flip the adjust position if tower are flipped
			if(aimed.tower.flipped) adjust = -adjust;
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
		Inventory.i.onSelect -= StructurePreviewing;
	}
}