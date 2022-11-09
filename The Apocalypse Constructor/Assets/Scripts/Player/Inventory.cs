using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
	#region Set this class to singleton
	public static Inventory i {get{if(_i==null){_i = GameObject.FindObjectOfType<Inventory>();}return _i;}} static Inventory _i;
	#endregion

	public Slot[] slots;
	public int selected;
	[SerializeField] Transform buildSnap;
	[SerializeField] Transform selectIndicator;
	[SerializeField] TextMeshProUGUI selectNameText;
	Vector2 mouseCoord;
	Camera cam;

	[System.Serializable] public class Slot 
	{
		public Stash stash;
		public TextMeshProUGUI nameText; //! Replace with image later
		public TextMeshProUGUI stackText;
	}

	#region Automaticly Get Inventory GUI (deactive)
	// private void OnValidate() 
	// {
	// 	//Go through all the child of layout
	// 	for (int c = 0; c < slotLayout.childCount; c++)
	// 	{
	// 		//Save this child
	// 		Transform panel = slotLayout.GetChild(c);
	// 		//The first child will be display the name of it panel
	// 		slots[c].nameText = panel.GetChild(0).GetComponent<TextMeshProUGUI>();
	// 		//The second child will be display the stack of it panel
	// 		slots[c].stackText = panel.GetChild(1).GetComponent<TextMeshProUGUI>();
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
		if(Input.GetKeyDown(KeyCode.X)) for (int i = 0; i < 10; i++) {if(Remove(slots[i].stash)) return;}
		ChoosingSlot();
		UsingSelectedSlot();
	}

	void ChoosingSlot()
	{
		//todo: Keybinds Inventory
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
		Stash stash = slots[selected].stash;
		//If the stash of selected slot is empty
		if(stash.name == "" || stash == null)
		{
			//Hide the select name panel
			selectNameText.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			//Display select name text as selected stash name
			selectNameText.text = stash.name;
			//Show the select name panel
			selectNameText.transform.parent.gameObject.SetActive(true);
		}
		//Move indicator to selected slot position
		selectIndicator.position = slots[selected].nameText.transform.position;
	}

	void UsingSelectedSlot()
	{
		//Get the coordinate in map of mouse position
		mouseCoord = Map.PositionToCoordinate(cam.ScreenToWorldPoint((Vector2)Input.mousePosition));
		//Snap the build to mouse coordinate
		buildSnap.transform.position = mouseCoord;
		//todo: Use Slot Keybind
		if(Input.GetKeyDown(KeyCode.Mouse0)) Use();
	}

	void Use()
	{
		//Get the selected stash
		Stash select = slots[selected].stash;
		//Dont use if there is no object selected at stash
		if(select.obj == null) return;
		//@ Switch the occupy layer depend what category of select building
		int occupy = 0; switch(select.category)
		{
			case "tower": occupy = 1; break;
			case "platform": occupy = 2; break;
			case "structure": occupy = 3; break;
		}
		//Placing the select buildings at mouse coordinate with occupian has decided
		Map.Placing(select.obj, mouseCoord, occupy);
	}
	
	public static bool Add(Stash stashing)
	{
		Slot[] slots = i.slots;
		///Go through all the slot of inventory to CHECKING STACK
		for (int s = 0; s < slots.Length; s++)
		{
			//If the given stash already stash 
			if(slots[s].stash != null) if(stashing.name == slots[s].stash.name)
			{
				//Check next slot if this stash has reached max stack
				if(slots[s].stash.stack >= slots[s].stash.maxStack) continue;
				//Stack stash then refresh display and successfully add item
				slots[s].stash.stack++; RefreshDisplay(s); return true;
			}
		}
		///Go through all the slot of inventory to FIND AN EMPTY SLOT
		for (int s = 0; s < slots.Length; s++)
		{
			//Add the given stash to this stash if it empty
			if(slots[s].stash == null) slots[s].stash = stashing;
			//If this stash has no stack
			if(slots[s].stash.stack == 0)
			{
				//This stash will be the given stash
				slots[s].stash = stashing;
				//Stack stash then refresh display and successfully add item
				slots[s].stash.stack++; RefreshDisplay(s); return true;
			}
		}
		//There are no slot left
		return false;
	}

	public static bool Remove(Stash stashing)
	{
		Slot[] slots = i.slots;
		///Go through all the slot of inventory to CHECKING STACK
		for (int s = 0; s < slots.Length; s++)
		{
			//Skip if this slot don't has any stash
			if(slots[s].stash.name == null) continue;
			if(slots[s].stash.name == "") continue;
			//If there is stash of the given stash and it is exist
			if(stashing.name == slots[s].stash.name)
			{
				//Remove the stash stack
				slots[s].stash.stack--;
				//Remove the stash if stash are out of stack
				if(slots[s].stash.stack == 0) slots[s].stash = null;
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
		if(slot.stash == null) {slot.stackText.text = ""; slot.nameText.text = ""; return;}
		//Update the slot display according to what it stash
		slot.nameText.text = slot.stash.name;
		slot.stackText.text = slot.stash.stack.ToString();
	}
}