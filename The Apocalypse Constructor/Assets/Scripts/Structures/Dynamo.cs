public class Dynamo : Structure
{
    public int provide;

	void Start()
	{
		//Provide max energy on being build
		Inventory.i.materials.Gain(0,0,0,+provide);
	}
	
	public override void Die()
	{
		//Lost max energy on being destroy
		Inventory.i.materials.Gain(0,0,0,-provide);
		base.Die();
	}
}