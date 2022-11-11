using UnityEngine;

public class Structure : MonoBehaviour
{
    public float maxHealth;
	[SerializeField] protected float health;
	[SerializeField] Vector2[] extend;
	public enum Function {none, tower, dynamo}; public Function function;

	void Start()
	{
		//Reset health
		health = maxHealth;
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

	public virtual void Hurt(float amount)
	{
		//Reduce health with amount got hurt
		health -= amount;
		//If out of health
		if(health <= 0)
		{
			Die();
		}
	}

	public virtual void Heal(float amount)
	{
		//Increase health with amount got heal
		health += amount;
		//Cap health from going beyond max health
		health = Mathf.Clamp(health, 0, maxHealth);
	}

	public virtual void Die()
	{
		print(gameObject.name + " Destroyed");
	}
}
