using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    #region Set this class to singleton
	public static Map i {get{if(_i==null){_i = GameObject.FindObjectOfType<Map>();}return _i;}} static Map _i;
	#endregion
	
	public List<Plot> plots = new List<Plot>();
	public float spacing;
	public Vector2Int size, initialSize;
	//Getting true size of the map
	public Vector2Int trueSize {get{return size*2;}}

	void Start()
	{
		//Det the size Y only
		size.y = initialSize.y;
		//Fill all the size with collum
		CreateCollum(initialSize.x);
	}

	void CreateCollum(int amount)
	{
		//If there is no plots created
		if(plots.Count == 0)
		{
			//Create an new plot middle plot at 0,0
			plots.Add(new Plot(new Vector2(0,0), Plot.Status.air));
			//Create all the plot for y axis at the middle
			for (int y = 1; y <= size.y; y++) CreateTwoSidePlot(new Vector2(0,y));
		}
		//Go through all the amount of collum need to create
		for (int a = 0; a < amount; a++)
		{
			//Map size increased
			size.x++;
			//Create an middle point on each side of current size x
			CreateTwoSidePlot(new Vector2(size.x, 0));
			//Create each side of plot in Y axis for each middle plot created
			for (int y = 1; y <= size.y; y++)
			{
				plots.Add(new Plot(new Vector2(size.x, y), Plot.Status.air));
				plots.Add(new Plot(new Vector2(size.x, -y), Plot.Status.air));
				plots.Add(new Plot(new Vector2(-size.x, y), Plot.Status.air));
				plots.Add(new Plot(new Vector2(-size.x, -y), Plot.Status.air));
			}
		}
	}

	//Function the create 2 node on at given coordinate and another at opposite
	void CreateTwoSidePlot(Vector2 coordinate)
	{
		plots.Add(new Plot(coordinate, Plot.Status.air));
		plots.Add(new Plot(-coordinate, Plot.Status.air));
	}


	void OnDrawGizmos()
	{
		//Colors
			Color plotColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.3f);
		//

		if(plots.Count > 0)
		{
			foreach (Plot plot in plots)
			{
				Gizmos.color = plotColor;
				Gizmos.DrawCube(plot.position, Vector2.one * (spacing - spacing/7));
			}
		}
	}
}

[System.Serializable] public class Plot
{
	public Vector2 coordinate;
	//Position are alway coordinate that multiply with spacing
	public Vector2 position {get {return coordinate * Map.i.spacing;}}
	public enum Status {air, blocked, standable, occupied} public Status status;

	//Assign new plot it needed variable
	public Plot(Vector2 coordinate, Status status)
	{
		this.coordinate = coordinate;
		this.status = status;
	}
}
