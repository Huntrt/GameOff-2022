using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
	#region Set this class to singleton
	public static Inventory i {get{if(_i==null){_i = GameObject.FindObjectOfType<Inventory>();}return _i;}} static Inventory _i;
	#endregion

	public Materials materials;
	public int selected; int scrollSelect; public Stash selectedStash;
	public delegate void OnSelect(Stash selected); public OnSelect onSelect;
	bool trashMode;
	
	[Header("GUI")]
	[SerializeField] Image selectIndicator;
	[SerializeField] Color indicatorDefaultColor, indicatorTrashColor;
	[SerializeField] TextMeshProUGUI selectNameText;
	[SerializeField] GameObject nightDisableWarning;
	public Slot[] slots;
	Camera cam;

	[System.Serializable] public class Slot 
	{
		public Stash stashed;
		public int stack;
		public bool select;
		public TextMeshProUGUI stackText;
		public Image iconImage;
	}
	[System.Serializable] public class Materials 
	{
		public int wood, steel, gunpowder, energy, maxEnergy;
		[Header("GUI")]
		public TextMeshProUGUI woodText;
		public TextMeshProUGUI steelText, gunpowderText, capacityText;

		public void Gain(int wood, int steel, int gunpowder, int energy, int maxEnergy)
		{
			//@ Gain the given material
			this.wood      += wood;
			this.steel     += steel;
			this.gunpowder += gunpowder;
			this.energy    -= energy;
			this.maxEnergy += maxEnergy;
			Refresh();
		}

		public bool Consume(int wood, int steel, int gunpowder, int energy, bool check = false)
		{
			//@ Checking if there still enough material to consume
			if(this.wood - wood < 0)              {print("Out of Wood"); return false;}
			if(this.steel - steel < 0)            {print("Out of Steel"); return false;}
			if(this.gunpowder - gunpowder < 0)    {print("Out of Gunpowder"); return false;}
			if(this.energy + energy > maxEnergy)  {print("Energy Maxxed"); return false;}
			//Dont consume if this just an check
			if(check) return true;
			//@ If has enough material then consume them
			this.wood      -= wood;
			this.steel     -= steel;
			this.gunpowder -= gunpowder;
			this.energy    += energy;
			Refresh();
			return true;
		}

		public void Refresh()
		{
			//Checking if still has enough energy for tower
			StructureManager.i.EnergySufficientCheck();
			//@ Make sure material dont go to negative
			if(wood < 0) wood = 0;
			if(steel < 0) steel = 0;
			if(gunpowder < 0) gunpowder = 0;
			if(energy < 0) energy = 0;
			if(maxEnergy < 0) maxEnergy = 0;
			//@ Display material value to it text
			woodText.text = wood.ToString();
			steelText.text = steel.ToString();
			gunpowderText.text = gunpowder.ToString();
			capacityText.text = energy + "/" + maxEnergy;
		}
	}

	#region Automaticly Get Inventory GUI (deactive)
	// [SerializeField] Transform slotLayout;
	// private void OnValidate() 
	// {
	// 	//Go through all the child of layout
	// 	for (int c = 0; c < slotLayout.childCount; c++)
	// 	{
	// 		//Save this child
	// 		Transform panel = slotLayout.GetChild(c);
	// 		//The first child will be display the icon
	// 		slots[c].iconImage = panel.GetChild(0).GetComponent<Image>();
	// 		//The second child first will be display the stack
	// 		slots[c].stackText = panel.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
	// 	}
	// }
	#endregion

	void Start()
	{
		cam = Camera.main;
		materials.Refresh();
	}
	
	void Update()
	{
		ChoosingSlot();
	}

	void ChoosingSlot()
	{
		//todo: Keybinds Inventory
		#region Inventory keycode
		if(Input.GetKeyDown(KeyCode.Alpha1)) Select(0);
		if(Input.GetKeyDown(KeyCode.Alpha2)) Select(1);
		if(Input.GetKeyDown(KeyCode.Alpha3)) Select(2);
		if(Input.GetKeyDown(KeyCode.Alpha4)) Select(3);
		if(Input.GetKeyDown(KeyCode.Alpha5)) Select(4);
		if(Input.GetKeyDown(KeyCode.Alpha6)) Select(5);
		if(Input.GetKeyDown(KeyCode.Alpha7)) Select(6);
		if(Input.GetKeyDown(KeyCode.Alpha8)) Select(7);
		if(Input.GetKeyDown(KeyCode.Alpha9)) Select(8);
		if(Input.GetKeyDown(KeyCode.Alpha0)) Select(9);
		#endregion
		//If the mouse are scrolling only when crafting GUI are closed
		if(Input.mouseScrollDelta.y != 0 && !Crafts.Crafting.i.craftingGUI.activeInHierarchy)
		{
			//Increase or decrease the scroll select selected index when scroll mouse
			scrollSelect = selected - Mathf.Clamp(Mathf.CeilToInt(Input.mouseScrollDelta.y),-1,1);
			//Cycle through the scroll select if it go over or under the slot amount
			if(scrollSelect > 9) scrollSelect = 0; if(scrollSelect < 0) scrollSelect = 9;
			//Choosed slot at selected using scroll
			Select(scrollSelect);
		}
	}

	void Select(int slot, bool refresh = false)
	{
		//Havent get any stash
		Stash stashed = null;
		//Deselect the previous slot if select and new slot 
		if(selected != slot) slots[selected].select = false;
		//If havent select the given slot or select is an refresh
		if(slots[slot].select == false || refresh)
		{
			//Select the given slot index
			selected = slot;
			//Get the stash at selected slot 
			stashed = slots[selected].stashed;
			//Show the select indicator
			selectIndicator.gameObject.SetActive(true);
			//Has select an new slot
			slots[slot].select = true;
		}
		//If select an already select slot
		else
		{
			//Hide the select indicator
			selectIndicator.gameObject.SetActive(false);
			//No longer select the same slot
			slots[slot].select = false;
		}
		//Call has select an slot 
		onSelect?.Invoke(stashed);
		//If the stash of selected slot is empty
		if(stashed == null)
		{
			//No longer select any stash
			selectedStash = null;
			//Hide the select name panel
			selectNameText.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			//Now select the stash has get
			selectedStash = stashed;
			//Display select name text as selected stash name
			selectNameText.text = stashed.name;
			//Show the select name panel
			selectNameText.transform.parent.gameObject.SetActive(true);
		}
		//Move indicator to selected slot position
		selectIndicator.transform.position = slots[selected].iconImage.transform.position;
		//Refresh transhing mode indicator
		RefreshTrashIndicator();
	}
	
	public void Use(Vector2 position, bool flip)
	{
		//Dont allow to use if crafting gui are still open
		if(Crafts.Crafting.i.craftingGUI.activeInHierarchy) return;
		//Shorting the selected stash
		Stash select = selectedStash;
		//Dont use if there is no selected stash at slot
		if(select == null) return;
		//If currently night time then show warning and stop use
		if(DaysManager.i.isNight) {nightDisableWarning.SetActive(true); return;}
		//Will select deplete any energy?
		int depleting = 0;
		//If using select are an tower tower
		if(select.prefab.CompareTag("Tower"))
		{	
			//Get the amount of energy this tower gonna deplete 
			depleting = select.prefab.GetComponent<Tower>().depleted;
			//Check to stop when deplete has go over max energy
			if(!materials.Consume(0,0,0,depleting,true)) return;
		}
		//Placing the select buildings at mouse coordinate with occupian has decided
		GameObject placed = Map.Placing(select.prefab, position, (int)select.occupation);
		//If sucessfully placed an structure
		if(placed != null)
		{
			//Trash the select stash that been use without refund
			Trash(selected, false);
			//Get the structure component of structure has place
			Structure structCmp = placed.GetComponent<Structure>();
			//Flip the structure base on given flip
			structCmp.FlipStructure(flip);
			//Set the placed structure stash as selected stash
			structCmp.stashed = select;
		}
	}

	public static bool Add(Stash stashing)
	{
		Slot[] slots = i.slots;
		///Go through all the slot of inventory to CHECKING STACK
		for (int s = 0; s < slots.Length; s++)
		{
			//If this slot already has the given item gonna stash 
			if(slots[s].stashed != null) if(stashing.name == slots[s].stashed.name)
			{
				//Skip if this stash has reached max stack
				if(slots[s].stack >= slots[s].stashed.maxStack) continue;
				//Stack stash then refresh display and successfully add item
				slots[s].stack++; Refresh(s); return true;
			}
		}
		///Go through all the slot of inventory to FIND AN EMPTY SLOT
		for (int s = 0; s < slots.Length; s++)
		{
			//If this stash has are empty
			if(slots[s].stashed == null)
			{
				//This stash will be the given stash
				slots[s].stashed = stashing;
				//Stack stash then refresh display and successfully add item
				slots[s].stack++; Refresh(s); return true;
			}
		}
		//There are no slot left
		return false;
	}

	public void TrashSelected() {Trash(selected, true);}

	public void ToggleTrashMode(bool enable) {trashMode = enable; RefreshTrashIndicator();}

	void RefreshTrashIndicator()
	{
		//Color that will be choose base on transh mode
		Color choose = Color.black;
		//Change select color to trash color if enable trash mode or default if nit
		if(trashMode) {choose = indicatorTrashColor;} else {choose =  indicatorDefaultColor;}
		//Overwrite indicator and select name color but not it alpha value
		selectIndicator.color = new Color(choose.r, choose.g, choose.b, selectIndicator.color.a);
		selectNameText.color = new Color(choose.r, choose.g, choose.b, selectNameText.color.a);;
	}

	public static void Trash(int slot, bool refund)
	{
		//Get the slot gonna get trash
		Slot[] slots = i.slots; Slot trashing = slots[slot];
		//Stop if try to trash nothing
		if(trashing.stashed == null) return;
		//Refund the trashed left over of stash if needed
		if(refund) Refund(trashing.stashed.Leftovering());
		//Remove the stash stack
		trashing.stack--;
		//This slot no longer has any stash if out of stack
		if(trashing.stack == 0) trashing.stashed = null;
		//Refresh at slot trashed
		Refresh(slot);
	}

	public static void Refund(Stash.Ingredients ingredients)
	{
		//Gain the leftover ingredients of structure being delete
		i.materials.Gain(ingredients.wood, ingredients.steel, ingredients.gunpowder,0,0);
	}

	static void Refresh(int index)
	{
		Slot slot = i.slots[index];
		//Reselect the slot currently select
		i.Select(i.selected, true);
		//Clear the slot display if it dont has stash
		if(slot.stashed == null) {slot.stackText.text = ""; slot.iconImage.enabled = false; return;}
		//Enable slot icon and set it to be stash icon
		slot.iconImage.enabled = true; slot.iconImage.sprite = slot.stashed.icon;
		//Set stack text to be how many stack has stash
		slot.stackText.text = slot.stack.ToString();
	}
}