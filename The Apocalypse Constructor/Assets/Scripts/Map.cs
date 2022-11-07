using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Set this class to singleton
	public static Map i {get{if(_i==null){_i = GameObject.FindObjectOfType<Map>();}return _i;}} static Map _i;
	#endregion
	
	public float spacing;
	///List of all the plot has create
	public List<Plot> plots = new List<Plot>();
	
	//Function to make any value take into account of spacing
	public static float Spaced(float value) {return (value) * i.spacing;}

	public static Vector2 PositionToCoordinate(Vector2 position)
	{
		//Make position take in account of spacing
		position /= i.spacing;
		//Snap the given position with spacing to get coordinate
		Vector2 coord = new Vector2
		(
			Map.Spaced(Mathf.RoundToInt(position.x)),
			Map.Spaced(Mathf.RoundToInt(position.y))
		);
		return coord;
	}

	public static Plot FindPlot(Vector2 coordinate)
	{
		//Go through all the plot has create
		for (int p = 0; p < i.plots.Count; p++)
		{
			//Return the plot at given coordinate
			if(i.plots[p].coordinate == coordinate) return i.plots[p];
		}
		//Return nothing if no plot exist in given coordinate
		return null;
	}
	
	/// Placing building onto an plot
	public static GameObject Placing(GameObject building, Vector2 coordinate, int occupy)
	{
		//Find the plot at given coordinate
		Plot plot = FindPlot(coordinate);
		//Exit if the plot dont exist
		if(plot == null) return null;

		//% Print an error if try to occupy out of range
		if(occupy < 0 || occupy > 3) {Debug.LogError("Cant occupying at ("+occupy+")"); return null;}
		
		/// If this plot has been blocked
		if(plot.occupian == 3) 
		{
			Debug.LogError("This plot has been block");
			return null;
		}
		/// If gonna get occupy by an TOWER
		if(occupy == 1)
		{
			//But already occupy by another TOWER
			if(plot.occupian == 1)
			{
				Debug.LogError("Cant place another tower on top of one");
				return null;
			}
			//But already occupy by an PLATFORM
			if(plot.occupian == 2)
			{
				//? Successful place an tower in platform
				//This plot are now blocked
				plot.occupian = 3;
			}
			//The plot are now occupy by an tower if available
			if(plot.occupian == 0) plot.occupian = occupy;
		}
		/// If gonna get occupy by an PLATFORM
		else if(occupy == 2)
		{
			//But already occupy by another PLATFORM
			if(plot.occupian == 2)
			{
				Debug.LogError("Cant place another platform on top of one");
				return null;
			}
			//But already occupy by an TOWER
			if(plot.occupian == 1)
			{
				//? Successful place an platform in tower
				//This plot are now blocked
				plot.occupian = 3;
			}
			//The plot are now occupy by an platform if available
			if(plot.occupian == 0) plot.occupian = occupy;
		}
		/// If gonna get occupy by an STRUCTURE 
		else if(occupy == 3)
		{
			//But plot is not available
			if(plot.occupian > 0)
			{
				Debug.LogError("This plot are not available for structure");
				return null;
			}
			//Plot are now locked by structure
			plot.occupian = 3;
		}

		//Create the given object at given coordinates then return it if need to create any
		return Instantiate(building, coordinate, Quaternion.identity);
	}

	/// Creating an new plot
	public static GameObject Creating(GameObject building, Vector2 coordinate, int occupying)
	{
		//Find the plot at given coordinate
		Plot plot = FindPlot(coordinate);
		//Create an new plot with given coordinate and occupian
		plot = new Plot(coordinate, occupying); 
		//Add newly create plot to list
		i.plots.Add(plot);
		//Create the given object at given coordinates then return it if need to create any
		if(building != null) return Instantiate(building, coordinate, Quaternion.identity); return null;
	}
	
	void OnDrawGizmos()
	{
		if(plots.Count > 0) foreach (Plot plot in plots) if(plot.occupian == 0)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(plot.coordinate, Vector2.one * (spacing - (spacing/10)));
		}
	}
}

[System.Serializable] public class Plot
{
	public Vector2 coordinate;
	//? 0 = nothing | 1 = tower | 2 = platform | 3 = blocked 
	public int occupian;

	public Plot(Vector2 coordinate, int occupied)
	{
		this.coordinate = coordinate;
		this.occupian = occupied;
	}
}