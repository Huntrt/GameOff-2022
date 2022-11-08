using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
	#region Set this class to singleton
	public static Inventory i {get{if(_i==null){_i = GameObject.FindObjectOfType<Inventory>();}return _i;}} static Inventory _i;
	#endregion

	public Slot[] slots;

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

	void Update()
	{
		//test: Remove stash from inventory one by one
		if(Input.GetKeyDown(KeyCode.X)) for (int i = 0; i < 10; i++) {if(Remove(slots[i].stash)) return;}
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