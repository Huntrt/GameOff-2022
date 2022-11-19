using UnityEngine;

public class Entity : MonoBehaviour
{ 
	public float maxHealth;
	public float health;
	public OnHealth onHurt, onHeal;
	public delegate float OnHealth(float amount);

	protected virtual void OnEnable()
	{
		health = maxHealth;
	}

	public virtual void Hurt(float amount)
	{
		//Reduce health with amount got hurt
		health -= amount;
		//Call hurt event with damage has take
		onHurt?.Invoke(amount);
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
		//Call heal event with damage has take
		onHeal?.Invoke(amount);
		//Cap health from going beyond max health
		health = Mathf.Clamp(health, 0, maxHealth);
	}

	public virtual void Die()
	{
		Destroy(gameObject);
	}
}
