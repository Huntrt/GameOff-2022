using UnityEngine;

public class Entity : MonoBehaviour
{
	
	public float maxHealth;
	public float health;
	public OnHealth onHurt, onHeal, onDeath;
	public delegate void OnHealth(float amount);

	protected virtual void OnEnable()
	{
		health = maxHealth;
	}

	public virtual void Hurt(float amount)
	{
		//Reduce health with amount got hurt
		health -= amount;
		//If out of health then call event with hurt amount then die
		if(health <= 0) {onDeath?.Invoke(amount); Die(); return;}
		//Call hurt event with damage has take
		onHurt?.Invoke(amount);
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
		
	}
}
