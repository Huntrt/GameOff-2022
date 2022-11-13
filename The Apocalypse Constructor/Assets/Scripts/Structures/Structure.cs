using UnityEngine;

public class Structure : Entity
{
	[SerializeField] Vector2[] extend;
	[SerializeField] public bool flipped;
	public enum Function {none, tower, dynamo}; public Function function;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Keep track of this structure
		StructureManager.i.structures.Add(this);
		//Keep track of this structure as filler
		StructureManager.i.fills.Add(this);
		Extending();
	}

	public void FlipStructure(bool isFlip)
	{
		//Set structure flipped as given flip
		flipped = isFlip;
		//Flip he tower Y rotation by 180 if it is flipped
		if(flipped) transform.rotation = Quaternion.Euler(0,180,0);
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

	public override void Die()
	{
		//Erased track of this structure and as filler
		StructureManager.i.structures.Remove(this);
		StructureManager.i.fills.Add(this);
		base.Die();
	}
}
