using UnityEngine;

public class Entity : MonoBehaviour
{
	public float maxHealth;
	[SerializeField] float initialMaxHP; public float InitialMaxHP {get => initialMaxHP;}
	[SerializeField] float growthMaxHP; public float GrowthMaxHP {get => growthMaxHP;}
	public float finalMaxHP;
	[SerializeField] float health; public float Health {get => health;}

	public OnHealth onHurt, onHeal, onDeath;

	public delegate void OnHealth(float amount);

	protected virtual void OnEnable()
	{
		
	}
	
	//? Since entity auto levelup from 0 when enable so this will also heal full hp since final start from 0 
	public void GrowingHealth(int level)
	{
		//Save the final max hp brfore grow it
		float preFinal = finalMaxHP;
		//Growth the initial max hp to get the final max hp
		finalMaxHP = initialMaxHP + (((growthMaxHP / 100) * initialMaxHP) * level);
		//Heal for the amount of max hp got increase
		Heal(finalMaxHP - preFinal);
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
		//Reset the final max hp to 0
		finalMaxHP -= finalMaxHP;
	}
}
