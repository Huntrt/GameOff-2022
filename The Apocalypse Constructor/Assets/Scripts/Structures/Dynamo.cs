public class Dynamo : Structure
{
    public int provide;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Keep track of this structure as dynamo
		StructureManager.i.dynamos.Add(this);
		//Erased track of this structure as filler
		StructureManager.i.fills.Remove(this);
		//Provide max energy on being build
		Inventory.i.materials.Gain(0,0,0,+provide);
	}
	
	public override void Die()
	{
		//Lost max energy on being destroy
		Inventory.i.materials.Gain(0,0,0,-provide);
		//Erased track of this structure as dynamo
		StructureManager.i.dynamos.Remove(this);
		base.Die();
	}
}