using UnityEngine;

public class Structure : Entity
{
	public Stash stash;
	[SerializeField] Vector2[] extends;
	public bool flipped;
	public enum Function {filler, tower, dynamo}; public Function function;

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
		//Go through all the plot needed to extend
		for (int e = 0; e < extends.Length; e++)
		{
			//Get an coordinate using structure position increase with this extend that got spaced
			Vector2 coord = Map.SnapPosition((Vector2)transform.position + (extends[e]*Map.i.spacing));
			//Extend an new empty plot at coordinate has get
			Map.ExtendPlot(null, coord, 0);
		}
	}

	void Retracting()
	{
		//Go through all the plot has been extend
		for (int e = 0; e < extends.Length; e++)
		{
			//Get an coordinate using structure position increase with this extend that got spaced
			Vector2 coord = Map.SnapPosition((Vector2)transform.position + (extends[e]*Map.i.spacing));
			//Retract the plot at coordinate has get
			Map.RetractPlot(coord);
		}
	}

	public override void Die()
	{
		Retracting();
		//Erased track of this structure and as filler
		StructureManager.i.structures.Remove(this);
		StructureManager.i.fills.Add(this);
		base.Die();
	}
}
