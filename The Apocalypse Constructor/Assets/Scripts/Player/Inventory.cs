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
	[Header("GUI")]
	public int selected;
	[SerializeField] Transform buildSnap;
	[SerializeField] Transform selectIndicator;
	[SerializeField] TextMeshProUGUI selectNameText;
	Vector2 mouseCoord;
	Camera cam;

	[System.Serializable] public class Slot 
	{
		public SO_Item stashed;
		public TextMeshProUGUI stackText;
		public Image iconImage;
	}
	
	[System.Serializable] public class Materials 
	{
		public int wood, steel, gunpowder, energy, maxEnergy;

		public void Gain(int wood, int steel, int gunpowder, int maxEnergy)
		{
			//@ Gain the given material
			this.wood      += wood;
			this.steel     += steel;
			this.gunpowder += gunpowder;
			this.maxEnergy += maxEnergy;
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
			return true;
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
	}
	
	void Update()
	{
		//test: Remove stash from inventory one by one
		if(Input.GetKeyDown(KeyCode.X)) for (int i = 0; i < 10; i++) {if(Remove(slots[i].stashed)) return;}
		ChoosingSlot();
		Use();
	}

	void ChoosingSlot()
	{
		//todo: Keybinds Inventory
		#region Inventory keycode
		if(Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
		if(Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
		if(Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
		if(Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
		if(Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
		if(Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
		if(Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);
		if(Input.GetKeyDown(KeyCode.Alpha8)) SelectSlot(7);
		if(Input.GetKeyDown(KeyCode.Alpha9)) SelectSlot(8);
		if(Input.GetKeyDown(KeyCode.Alpha0)) SelectSlot(9);
		#endregion
		//If the mouse are scrolling
		if(Input.mouseScrollDelta.y != 0)
		{
			//Increase or decrease the selected index when scroll mouse
			selected += Mathf.Clamp(Mathf.CeilToInt(Input.mouseScrollDelta.y),-1,1);
			//Choosed slot at selected
			SelectSlot(selected);
		}
	}

	void SelectSlot(int index)
	{
		//Select the given index
		selected = index;
		//Clamp the selected
		selected = Mathf.Clamp(selected, 0, slots.Length-1);
		//Get the stash at selected slot 
		SO_Item stashed = slots[selected].stashed;
		//If the stash of selected slot is empty
		if(stashed == null)
		{
			//Hide the select name panel
			selectNameText.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			//Display select name text as selected stash name
			selectNameText.text = stashed.name;
			//Show the select name panel
			selectNameText.transform.parent.gameObject.SetActive(true);
		}
		//Move indicator to selected slot position
		selectIndicator.position = slots[selected].iconImage.transform.position;
	}

	void Use()
	{
		//Get the coordinate in map of mouse position
		mouseCoord = Map.PositionToCoordinate(cam.ScreenToWorldPoint((Vector2)Input.mousePosition));
		//Snap the build to mouse coordinate
		buildSnap.transform.position = mouseCoord;
		//todo: Use Slot Keybind
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{	
			//Get the selected stash
			SO_Item select = slots[selected].stashed;
			//Dont use if there is no selected stash at slot
			if(select == null) return;
			//Placing the select buildings at mouse coordinate with occupian has decided
			Map.Placing(select.prefab, mouseCoord, select.occupation);
		}
	}

	public static bool Add(SO_Item stashing)
	{
		Slot[] slots = i.slots;
		///Go through all the slot of inventory to CHECKING STACK
		for (int s = 0; s < slots.Length; s++)
		{
			//If this slot already has the given item gonna stash 
			if(slots[s].stashed != null) if(stashing.name == slots[s].stashed.name)
			{
				//Skip if this stash has reached max stack
				if(slots[s].stashed.stack.cur >= slots[s].stashed.stack.max) continue;
				//Stack stash then refresh display and successfully add item
				slots[s].stashed.stack.cur++; RefreshDisplay(s); return true;
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
				slots[s].stashed.stack.cur++; RefreshDisplay(s); return true;
			}
		}
		//There are no slot left
		return false;
	}

	public static bool Remove(SO_Item stashing)
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
				slots[s].stashed.stack.cur--;
				//Remove the stash if stash are out of stack
				if(slots[s].stashed.stack.cur == 0) slots[s].stashed = null;
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
		slot.stackText.text = slot.stashed.stack.cur.ToString();
		//Refresh slot selection
		i.SelectSlot(i.selected);
	}
}