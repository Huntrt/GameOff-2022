using UnityEngine;

public class Entity : MonoBehaviour
{ 
	public float maxHealth;
	[SerializeField] protected float health;

	void OnEnable()
	{
		health = maxHealth;
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
