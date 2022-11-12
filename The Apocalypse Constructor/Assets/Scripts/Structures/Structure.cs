using UnityEngine;

public class Structure : Entity
{
	[SerializeField] Vector2[] extend;
	public enum Function {none, tower, dynamo}; public Function function;

	void Start()
	{
		Extending();
	}

	void Extending()
	{
		//Go through all the plot needed to extand
		for (int p = 0; p < extend.Length; p++)
		{
			//Get this coordinate using current position increase with this extend that got spaced
			Vector2 coord = Map.SnapPosition((Vector2)transform.position + (extend[p]*Map.i.spacing));
			//Extend an new empty plot at cordinate has get if that coordinate dont has plot
			if(Map.FindPlot(coord) == null) Map.Creating(null, coord, 0);
		}
	}
}
