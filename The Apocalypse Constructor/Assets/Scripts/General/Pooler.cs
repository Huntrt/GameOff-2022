using System.Collections.Generic;
using UnityEngine;
using System;

public class Pooler : MonoBehaviour
{
	#region Set this class to singleton
	public static Pooler i 
	{
		//Create an instance of the pooler once if it haven't exist
		get {if(_i == null) {_i = GameObject.FindObjectOfType<Pooler>();} return _i; }
	}
	static Pooler _i;
	#endregion
	
	[Serializable] public class Pool {public List<GameObject> pool = new List<GameObject>();} 
	public List<Pool> pools = new List<Pool>(); 
	public List<string> names;

	//Create the object needed with wanted position, rotation, does it auto active upon create? and do it need to has parent?
	public GameObject Create(GameObject need, Vector3 position, Quaternion rotation, bool autoActive = true, Transform parent = null)
	{
		//Get the name of needed object
		string name = need.name + "(Clone)";
		//If haven't save the name
		if(!names.Contains(name))
		{
			//Save it
			names.Add(name);
			//Make an pool for that name
			pools.Add(new Pool());
		}
		//Use the pool at index of the needed name
		List<GameObject> use = pools[names.IndexOf(name)].pool;

		//If the use pool has object then go through all of it object
		if(use.Count > 0) {for (int o = 0; o < use.Count; o++)
		{
			//Get the current object will use
			GameObject obj = use[o];
			//Remove any using object that is null
			if(obj == null) {use.RemoveAt(o); continue;}
			//If this object is unactive 
			if(!obj.activeInHierarchy)
			{
				//Move this object to last
				use.Add(obj); use.RemoveAt(o);
				//Using the last object since it got move
				obj = use[use.Count-1];
				//Set it position
				obj.transform.position = position;
				//Set it rotation
				obj.transform.rotation = rotation;
				//Set it to given parent
				obj.transform.SetParent(parent);
				//Active it if need to
				obj.SetActive(autoActive);
				//Return it and no need to create new
				return obj;
			}
		}}

		///If there is no unactive object left in pool
		//Create an new needed object at given position and rotation
		GameObject newObj = Instantiate(need, position, rotation);
		//Set the new object to given parent
		newObj.transform.SetParent(parent);
		//Add new object into the use list
		use.Add(newObj);
		//Active the new object if need to
		newObj.SetActive(autoActive);
		//Return the new object
		return newObj;
	}

	//Return the game object list of pool of given name
	public List<GameObject> GetPool(string name) {return pools[names.IndexOf(name)].pool;}
}