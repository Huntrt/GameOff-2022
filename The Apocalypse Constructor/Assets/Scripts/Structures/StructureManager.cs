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
}
