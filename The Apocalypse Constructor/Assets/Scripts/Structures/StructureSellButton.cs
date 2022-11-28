using UnityEngine.EventSystems;
using UnityEngine;
public class StructureSellButton : MonoBehaviour
{
	[SerializeField] StructureDetails details;
	Structure structure;
	[SerializeField] Structure.Function function;

	void Update()
	{
		//Also selling if press the sell key
		if(Input.GetKeyDown(KeyOperator.i.SellStructure)) Selling();
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

    public void Selling()
	{
		//Refund the love over of this button's structure
		Inventory.Refund(structure.stashed.Leftovering());
		//Instanly kill this button's structure
		structure.Die();
	}
}
