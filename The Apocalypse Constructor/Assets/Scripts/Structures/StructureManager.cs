using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
	#region Set this class to singleton
	public static StructureManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<StructureManager>();}return _i;}} static StructureManager _i;
	#endregion

    public List<Structure> structures;
    public List<Structure> fills;
    public List<Dynamo> dynamos;
	public List<Tower> towers;

	public LayerMask structureLayer;
	public GameObject insufficientIndiPrefab;

	public void EnergySufficientCheck()
	{
		//The total depleted of all tower
		int totalDepleted = 0;
		//Go through all the tower
		for (int t = 0; t < towers.Count; t++)
		{
			//Count each depleted of each tower
			totalDepleted += towers[t].depleted;
			//Thus tower havent insufficient of energy yet
			towers[t].insufficient = false;
			//This tower are now insufficient of energy when total deplete has go over the max energy
			if(totalDepleted > Inventory.i.materials.maxEnergy) towers[t].insufficient = true;
			//Refesh this tower for an state of insufficient
			towers[t].RefreshInsufficient();
		}
	}
}
