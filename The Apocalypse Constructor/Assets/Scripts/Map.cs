using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Set this class to singleton
	public static Map i {get{if(_i==null){_i = GameObject.FindObjectOfType<Map>();}return _i;}} static Map _i;
	#endregion
	
	public float spacing;
	///List of all the plot has create
	public static List<Vector2> plots = new List<Vector2>();
	///List of all the plot as been blocked
	public static List<Vector2> blocked = new List<Vector2>();

	public static Vector2 PositionToCoordinate(Vector2 position)
	{
		//Make position take in account of spacing
		position /= i.spacing;
		//Snap the given position with spacin to get coordinate
		Vector2 coord = new Vector2
		(
			Mathf.RoundToInt(position.x) * i.spacing,
			Mathf.RoundToInt(position.y) * i.spacing
		);
		return coord;
	}

	public static void Placing(GameObject create, Vector2 coordinate, string placeStatus)
	{
		//Added snapped if there no plot been snap there yet
		if(!plots.Contains(coordinate)) plots.Add(coordinate);
		//If placing also need to an status
		switch(placeStatus)
		{
			//Blocked at sanpped if needed
			case "blocked": if(!blocked.Contains(coordinate)) blocked.Add(coordinate); break;
		}
		//Create the given object at given coordinates
		Instantiate(create, coordinate, Quaternion.identity);
	}
}