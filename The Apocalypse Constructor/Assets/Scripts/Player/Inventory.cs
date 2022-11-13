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
	public int selected; public OnSelect onSelect;
	[HideInInspector] public Stash selectedStash;
	[Header("GUI")]
	[SerializeField] Transform selectIndicator;
	[SerializeField] TextMeshProUGUI selectNameText;
	Camera cam;

	public delegate void OnSelect(Stash selected);
	[System.Serializable] public class Slot 
	{
		public Stash stashed;
		public int stack;
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
			//Increase or decrease the selected index when scroll mouse
			selected += Mathf.Clamp(Mathf.CeilToInt(Input.mouseScrollDelta.y),-1,1);
			//Choosed slot at selected
			Select(selected);
		}
	}

	void Select(int slot)
	{
		//Select the given slot index
		selected = slot;
		//Clamp the selected slot
		selected = Mathf.Clamp(selected, 0, slots.Length-1);
		//Get the stash at selected slot 
		Stash stashed = slots[selected].stashed;
		//Call has select an new slot 
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
			//Save the stash at selected slot
			selectedStash = slots[selected].stashed;
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
		//Get the selected stash
		Stash select = slots[selected].stashed;
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
				slots[s].stack++; RefreshDisplay(s); return true;
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
				slots[s].stack++; RefreshDisplay(s); return true;
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
			//If there is stash of the given stash and it is exist
			if(slots[s].stashed != null) if(stashing.name == slots[s].stashed.name)
			{
				//Remove the stash stack
				slots[s].stack--;
				//Remove the stash if stash are out of stack
				if(slots[s].stack == 0) slots[s].stashed = null;
				//Refresh display and successfully remove item
				RefreshDisplay(s); return true;
			}
		}
		return false;
	}

	public static void RefreshDisplay(int index)
	{
		Slot slot = i.slots[index];
		//Clear the slot display if it dont has stash
		if(slot.stashed == null) {slot.stackText.text = ""; slot.iconImage.enabled = false; return;}
		//Enable slot icon and set it to be stash icon
		slot.iconImage.enabled = true; slot.iconImage.sprite = slot.stashed.icon;
		//Set stack text to be how many stack has stash
		slot.stackText.text = slot.stack.ToString();
		//Refresh slot selection
		i.Select(i.selected);
	}
}