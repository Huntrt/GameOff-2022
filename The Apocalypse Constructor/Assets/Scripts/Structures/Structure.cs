using UnityEngine;
using System;

public class Structure : Entity
{
	[SerializeField] Vector2[] extends;
	[HideInInspector] public bool flipped;
	public enum Function {filler, tower, dynamo}; public Function function;
	public Action onDie;
	protected StructureManager manager;
	public StructureStashData stash; [HideInInspector] public class StructureStashData
	{
		public Stash.Ingredients leftovered;
		public Stash.Occupation occupied;
	}

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
		//Call die action
		onDie?.Invoke();
		Retracting();
		//Erased track of this structure and as filler
		manager.structures.Remove(this);
		manager.fills.Add(this);
		base.Die();
	}
}
