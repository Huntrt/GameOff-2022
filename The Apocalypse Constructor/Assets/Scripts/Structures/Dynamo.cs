public class Dynamo : Structure
{
    public int provide;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Keep track of this structure as dynamo
		manager.dynamos.Add(this);
		//Erased track of this structure as filler
		manager.fills.Remove(this);
		//Provide max energy on being build
		Inventory.i.materials.Gain(0,0,0,0,+provide);
	}
	
	public override void Die()
	{
		//Erased track of this structure as dynamo
		manager.dynamos.Remove(this);
		//Lost max energy on being destroy
		Inventory.i.materials.Gain(0,0,0,0,-provide);
		base.Die();
	}
}