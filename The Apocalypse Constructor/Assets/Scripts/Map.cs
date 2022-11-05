using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Set this class to singleton
	public static Map i {get{if(_i==null){_i = GameObject.FindObjectOfType<Map>();}return _i;}} static Map _i;
	#endregion
	
	public float spacing;
	///List of all the coordinate has create
	public static List<Vector2> coordinates = new List<Vector2>();
	///List of all the coordinates as been blocked
	public static List<Vector2> blocked = new List<Vector2>();

	//Function to make any value take into account of spacing
	public static float Spaced(float value) {return (value) * Map.i.spacing;}

	public static Vector2 PositionToCoordinate(Vector2 position)
	{
		//Make position take in account of spacing
		position /= i.spacing;
		//Snap the given position with spacin to get coordinate
		Vector2 coord = new Vector2
		(
			Mathf.RoundToInt(Map.Spaced(position.x)),
			Mathf.RoundToInt(Map.Spaced(position.y))
		);
		return coord;
	}

	public static GameObject Placing(GameObject create, Vector2 coordinate, string placeStatus)
	{
		//Added snapped coordinate if there no coordinate been snap there yet
		if(!coordinates.Contains(coordinate)) coordinates.Add(coordinate);
		//Has placing status been set
		bool setStatus = false;
		//If placing also need an status
		switch(placeStatus)
		{
			//Blocked at sanpped if needed
			case "blocked": if(!blocked.Contains(coordinate)) blocked.Add(coordinate); setStatus = true; break;
		}
		//Print an error if status need to place does not exist
		if(!setStatus) Debug.LogError("There are no placing status named [" + placeStatus + "]");
		//Create the given object at given coordinates then return it
		return Instantiate(create, coordinate, Quaternion.identity);
	}
}