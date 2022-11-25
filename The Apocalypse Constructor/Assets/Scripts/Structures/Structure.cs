using UnityEngine;
using System;

public class Structure : Entity
{
	[SerializeField] Vector2[] extends;
	[HideInInspector] public bool flipped;
	public enum Function {filler, tower, dynamo}; public Function function;
	protected StructureManager manager;
	[HideInInspector] public Stash stashed;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Get the structure
		manager = StructureManager.i;
		//Keep track of this structure
		manager.structures.Add(this);
		//Keep track of this structure as filler
		manager.fills.Add(this);
		Extending();
	}

	public virtual void FlipStructure(bool isFlip)
	{
		//Set structure flipped as given flip
		flipped = isFlip;
		//If the tower is flipped
		if(flipped)
		{
			//Flip it X and Y rotation
			transform.rotation = Quaternion.Euler(180,180,0);
			//Flip it Y of sprite render
			GetComponent<SpriteRenderer>().flipY = true;
		}
	}

	void Extending()
	{
		//Go through all the plot needed to extend
		for (int e = 0; e < extends.Length; e++)
		{
			//Get an coordinate using structure position increase with this extend that got spaced
			Vector2 coord = Map.SnapPosition((Vector2)transform.position + (extends[e]*Map.i.spacing));
			//Extend an empty plot at this coordinate
			Map.ExtendPlot(coord, 0);
		}
	}

	void Retracting()
	{
		//Go through all the plot has been extend
		for (int e = 0; e < extends.Length; e++)
		{
			//Get an coordinate using structure position increase with this extend that got spaced
			Vector2 coord = Map.SnapPosition((Vector2)transform.position + (extends[e]*Map.i.spacing));
			//Retract the plot at this coordinate
			Map.RetractPlot(coord);
		}
	}

	public override void Die()
	{
		//Retract then delete itself off the map
		Retracting(); Map.Deleting(this);
		//Erased track of this structure and as filler
		manager.structures.Remove(this);
		manager.fills.Add(this);
		base.Die();
	}
}
