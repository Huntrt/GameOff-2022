using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
	#region Set this class to singleton
	public static Inventory i {get{if(_i==null){_i = GameObject.FindObjectOfType<Inventory>();}return _i;}} static Inventory _i;
	#endregion

	public Materials materials;
	public Slot[] slots;
	public int selected; int scrollSelect;
	public Stash selectedStash;
	public delegate void OnSelect(Stash selected); public OnSelect onSelect;
	
	[Header("GUI")]
	[SerializeField] Transform selectIndicator;
	[SerializeField] TextMeshProUGUI selectNameText;
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

		public void Gain(int wood, int steel, int gunpowder, int maxEnergy)
		{
			//@ Gain the given material
			this.wood      += wood;
			this.steel     += steel;
			this.gunpowder += gunpowder;
			this.maxEnergy += maxEnergy;
			UpdateCounter();
		}

		public bool Consume(int wood, int steel, int gunpowder, int energy = 0)
		{
			//@ Checking if there still enough material to consume
			if(this.wood - wood < 0)              {print("Out of Wood"); return false;}
			if(this.steel - steel < 0)            {print("Out of Steel"); return false;}
			if(this.gunpowder - gunpowder < 0)    {print("Out of Gunpowder"); return false;}
			if(this.energy + energy > maxEnergy)  {print("Energy Maxxed"); return false;}
			//@ If has enough material then consume them
			this.wood      -= wood;
			this.steel     -= steel;
			this.gunpowder -= gunpowder;
			this.energy    += energy;
			UpdateCounter();
			return true;
		}

		public void UpdateCounter()
		{
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
	// 		//The first child will be display the stack
	// 		slots[c].stackText = panel.GetChild(0).GetComponent<TextMeshProUGUI>();
	// 		//The second child will be display the icon
	// 		slots[c].iconImage = panel.GetChild(1).GetComponent<Image>();
	// 	}
	// }
	#endregion

	void Start()
	{
		cam = Camera.main;
		materials.UpdateCounter();
	}
	
	void Update()
	{
		//test: Remove stash from inventory one by one
		if(Input.GetKeyDown(KeyCode.X)) for (int i = 0; i < 10; i++) {if(Remove(slots[i].stashed)) return;}
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
			scrollSelect = selected + Mathf.Clamp(Mathf.CeilToInt(Input.mouseScrollDelta.y),-1,1);
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
		selectIndicator.position = slots[selected].iconImage.transform.position;
	}
	
	public void Use(Vector2 position, bool flip)
	{
		//Shorting the selected stash
		Stash select = selectedStash;
		//Dont use if there is no selected stash at slot
		if(select == null) return;
		//If use the select tower but dont has enough energy for it to be depleted
		if(select.prefab.CompareTag("Tower") && !materials.Consume(0,0,0,select.prefab.GetComponent<Tower>().depleted))
		{
			//Dont allow use
			return;
		}
		//Placing the select buildings at mouse coordinate with occupian has decided
		GameObject placed = Map.Placing(select.prefab, position, (int)select.occupation);
		//If sucessfully placed an structure
		if(placed != null)
		{
			//Flip the the placed structure with given flip
			placed.GetComponent<Structure>().FlipStructure(flip);
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

	public static bool Remove(Stash stashing)
	{
		Slot[] slots = i.slots;
		//False if try to remove nothing
		if(stashing == null) return false;
		///Go through all the slot of inventory to CHECKING STACK
		for (int s = 0; s < slots.Length; s++)
		{
			//If the stash want to remove does exist and it in this slot
			if(slots[s].stashed != null) if(stashing.name == slots[s].stashed.name)
			{
				//Remove the stash stack
				slots[s].stack--;
				////This slot no longer has any stash if out of stash
				if(slots[s].stack == 0) slots[s].stashed = null;
				//Refresh display and successfully remove item
				Refresh(s); return true;
			}
		}
		return false;
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