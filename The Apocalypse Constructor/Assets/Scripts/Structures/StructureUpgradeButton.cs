using UnityEngine.EventSystems;
using UnityEngine;

public class StructureUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
	[SerializeField] StructureDetails details;
	Structure structure;
	[SerializeField] Structure.Function function;
	bool pointerOver;

	void Update()
	{
		//Also selling if press the sell key
		if(Input.GetKeyDown(KeyOperator.i.UpgradeStructure)) Upgrading();
	}

	//? When the details panel of this sell button get active
	void OnEnable()
	{
		//Go through all the structure currently detail
		for (int d = 0; d < details.detailings.Length; d++) 
		{
			//Set button's structure to be details structure has the same function of this button
			if(details.detailings[d].function == function) {structure = details.detailings[d]; break;}
		}
	}

    public void Upgrading()
	{
		//Get the cost of upgrading button's structure
		Stash.Ingredients cost = structure.stashed.upgrading;
		//Consume the cost to upgrade structure
		Inventory.i.materials.Consume(cost.wood, cost.steel, cost.gunpowder, 0);
		//Button's structure level up
		structure.LevelUp();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		//Show the modifier that will display upgrade cost ingredients
		Inventory.i.materials.gameGui.ShowModifier(structure.stashed.upgrading, false);
		//Being pointer over
		pointerOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Close the modifier
		Inventory.i.materials.gameGui.ShowModifier(null);
		//No longer pointer over
		pointerOver = true;
	}
	
	void OnDisable()
	{
		//If get disable while still point over this panel
		if(pointerOver)
		{
			//Close the materials modifier
			Inventory.i.materials.gameGui.ShowModifier(null);
			//No longer being pointer over
			pointerOver = false;
		}
	}
}
