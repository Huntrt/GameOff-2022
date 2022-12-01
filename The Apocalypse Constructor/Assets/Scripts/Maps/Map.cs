using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Map : MonoBehaviour
{
    #region Set this class to singleton
	public static Map i {get{if(_i==null){_i = GameObject.FindObjectOfType<Map>();}return _i;}} static Map _i;
	#endregion
	
	public float spacing;
	public Ground ground;
	///List of all the plot has create
	public List<Plot> plots = new List<Plot>();
	[SerializeField] GameObject occupyPopup;
	public System.Action onRextend;
	
	//Function to make any value take into account of spacing
	public static float Spaced(float value) {return (value) * i.spacing;}

	public static Vector2 SnapPosition(Vector2 position)
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
		//? If the given coordnate divide with spacing are not whole number
		if((coordinate.x / i.spacing) % 1 != 0 || (coordinate.y / i.spacing) % 1 != 0)
		{
			//Print an warning
			Debug.LogWarning("The plot try to find at " + coordinate + " might has incorrect coordinate");
		}
		//Go through all the plot has create
		for (int p = 0; p < i.plots.Count; p++)
		{
			//Return the plot at given coordinate
			if(i.plots[p].coordinate == coordinate) return i.plots[p];
		}
		//Return nothing if no plot exist in given coordinate
		return null;
	}
	
	/// Extending an new plot
	public static void ExtendPlot(Vector2 coordinate, int occupying)
	{
		//Find the plot at given coordinate to extend at it
		Plot plot = FindPlot(coordinate);
		//If the extending plot don't exist yet
		if(plot == null)
		{
			//Create an new plot ar extend coordinate with given occupying
			plot = new Plot(coordinate, occupying);
			//The new plot has been extend by 1
			plot.extended++;
			//Add newly create plot to list
			i.plots.Add(plot);
		}
		//If the extending plot already exist
		else
		{
			//The plot has been extend one more time
			plot.extended++;
		}
	}

	public static void RetractPlot(Vector2 coordinate)
	{
		//Find the plot at given coordinate to retract at it
		Plot plot = FindPlot(coordinate);
		//If the plot gonna retract does exist
		if(plot != null)
		{
			//The plot lost an extend
			plot.extended--;
			//Remove the plot from map list if plot no longer has any extend and it is empty
			if(plot.extended <= 0 && plot.occupation == 0) plot.Remove(i.plots);
		}
		//If the plot gonna retract dont exist
		else
		{
			//Print error to make sure when try to retract plot haven't exist
			Debug.LogWarning("Cant retract an plot at '" + coordinate + "'since it don't exist");
		}
	}
	
	/// Placing structure onto an plot
	public static GameObject Placing(GameObject structure, Vector2 coordinate, int occupy)
	{
		//Find the plot at given coordinate
		Plot plot = FindPlot(coordinate);
		//Exit if the plot dont exist (air)
		if(plot == null) return null;

		//% Print an error if try to occupy out of range
		if(occupy < 0 || occupy > 3) {Debug.LogWarning("Cant occupying at ("+occupy+")"); return null;}
		
		/// If this plot has been blocked
		if(plot.occupation == 3) 
		{
			i.OccupyWarningPopup("This plot has been block");
			return null;
		}
		/// If gonna get occupy by an TOWER
		if(occupy == 1)
		{
			//But already occupy by another TOWER
			if(plot.occupation == 1)
			{
				i.OccupyWarningPopup("Cant place tower on top of one");
				return null;
			}
			//But already occupy by an PLATFORM
			if(plot.occupation == 2)
			{
				//? Successful place an tower in platform
				//This plot are now blocked
				plot.occupation = 3;
			}
			//The plot are now occupy by an tower if available
			if(plot.occupation == 0) plot.occupation = occupy;
		}
		/// If gonna get occupy by an PLATFORM
		else if(occupy == 2)
		{
			//But already occupy by another PLATFORM
			if(plot.occupation == 2)
			{
				i.OccupyWarningPopup("Cant place platform on top of one");
				return null;
			}
			//But already occupy by an TOWER
			if(plot.occupation == 1)
			{
				//? Successful place an platform in tower
				//This plot are now blocked
				plot.occupation = 3;
			}
			//The plot are now occupy by an platform if available
			if(plot.occupation == 0) plot.occupation = occupy;
		}
		/// If gonna get occupy by an STRUCTURE 
		else if(occupy == 3)
		{
			//But plot is not available
			if(plot.occupation > 0)
			{
				Debug.LogWarning("This plot are not available for structure");
				return null;
			}
			//Plot are now locked by structure
			plot.occupation = 3;
		}
		//Create the given object at given coordinates then return it if need to create any
		return Instantiate(structure, coordinate, Quaternion.identity);
	}

	public static void Deleting(Structure structure)
	{
		//Find the plot of structure gonna delete 
		Plot plot = FindPlot(structure.transform.position);
		//If the plot try to delete does not exist
		if(plot == null)
		{
			//Print an error if try to delete an structure without plot
			Debug.LogError("Fail to delete '" + structure.gameObject.name + "' since no plot exist there");
			//Stop deleteing
			return;
		}
		//Reduce the plot occupation with deleted structure occupation
		plot.occupation -= (int)structure.stashed.occupation;
		//Remove the plot from map list if plot no longer has any extend and it is empty
		if(plot.extended <= 0 && plot.occupation == 0) plot.Remove(i.plots);
	}

	void OccupyWarningPopup(string popText)
	{
		//Pool the occupy popup at mouse coordinate
		GameObject popUp = Pooler.i.Create(occupyPopup, PlayerCursor.i.mouseCoord, Quaternion.identity);
		//Set the popup text as popup text has get
		popUp.GetComponentInChildren<TextMeshProUGUI>().text = popText;
	}
	
	//% Map Grid Gizmo
	// void OnDrawGizmos()
	// {
	// 	if(plots.Count > 0) foreach (Plot plot in plots) 
	// 	{
	// 		if(plot.occupation == 0) Gizmos.color = Color.green;
	// 		if(plot.occupation == 1) Gizmos.color = Color.cyan;
	// 		if(plot.occupation == 2) Gizmos.color = Color.yellow;
	// 		if(plot.occupation == 3) Gizmos.color = Color.red;
	// 		Gizmos.DrawWireCube(plot.coordinate, Vector2.one * (spacing - (spacing/10)));
	// 	}
	// }
}

[System.Serializable] public class Plot
{
	public Vector2 coordinate;
	//note: 0 = empty | 1 = tower | 2 = platform | 3 = blocked 
	public int occupation;
	public int extended;
	[HideInInspector] public SpriteRenderer visual;
	[HideInInspector] public bool grounded;

	public Plot(Vector2 coordinate, int occupied) 
	{
		this.coordinate = coordinate;
		this.occupation = occupied;
		//This plot are belong to ground if it coordinate match ground Y
		grounded = coordinate.y == Map.i.ground.initalSize.y;
	}

	public void Remove(List<Plot> plots)
	{
		//Dont allow removal of ground plot
		if(grounded) return;
		//Destroy it visual if it does has one
		if(visual != null) Object.Destroy(visual.gameObject);
		//Remove itself from given list
		plots.Remove(this);
	}
}