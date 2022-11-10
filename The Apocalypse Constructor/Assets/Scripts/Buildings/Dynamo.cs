public class Dynamo : Building
{
	public new SO_Dynamo so;
    public int provide;

	void Start()
	{
		SetupStats();
		//Provide max energy on being build
		Inventory.i.materials.Gain(0,0,0,+provide);
	}
	
	void SetupStats()
	{
		maxHealth = so.maxHealth;
		consumption = so.consumption;
		provide = so.energyProvide;
	}


	public override void Die()
	{
		//Lost max energy on being destroy
		Inventory.i.materials.Gain(0,0,0,-provide);
		base.Die();
	}
}