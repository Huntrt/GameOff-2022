using UnityEngine;

public class Building : MonoBehaviour
{
	public SO_Building so;
    public float maxHealth;
	public int consumption;
	[SerializeField] protected float health;
	public enum Function {none, attack, energize}; public Function function;

	void Start()
	{
		SetupStats();
		//Reset health
		health = maxHealth;
	}

	void SetupStats()
	{
		maxHealth = so.maxHealth;
		consumption = so.consumption;
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
