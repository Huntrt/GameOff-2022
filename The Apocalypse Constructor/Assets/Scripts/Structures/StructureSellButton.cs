using UnityEngine;

public class StructureSellButton : MonoBehaviour
{
	[SerializeField] StructureDetails details;
	[SerializeField] Structure.Function function;

	void Update()
	{
		//Also selling if press the sell key
		if(Input.GetKeyDown(KeyOperator.i.SellStructure)) Selling();
	}

    public void Selling()
	{
		//Go through all the structure currently detail
		for (int d = 0; d < details.detailings.Length; d++)
		{
			//Get this details structure
			Structure detail = details.detailings[d];
			//If this structure has same function has set while still exist
			if(detail.function == function && detail != null)
			{
				//Refund the loverover of this details structure
				Inventory.Refund(detail.stashed.Leftovering());
				//Instanly kill this details structure
				detail.Die();
				break;
			}
		}
	}
}
