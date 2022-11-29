using UnityEngine;

public class Entity : MonoBehaviour
{
	[SerializeField] float initialMaxHP; public float InitialMaxHP {get => initialMaxHP;}
	[SerializeField] float growthMaxHP; public float GrowthMaxHP {get => growthMaxHP;}
	public float finalMaxHP;
	[SerializeField] float health; public float Health {get => health;}
	[SerializeField] AudioClip spawnSound, hurtSound, healSound, dieSound;

	public OnHealth onHurt, onHeal, onDeath;

	public delegate void OnHealth(float amount);

	protected virtual void OnEnable()
	{
		//Play the spawn sound when this entity enable if needed
		if(spawnSound != null) SessionOperator.i.audios.soundSource.PlayOneShot(spawnSound);
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
		//Play the hurt sound when this entity take damage if needed
		if(hurtSound != null) SessionOperator.i.audios.soundSource.PlayOneShot(hurtSound);
	}

	public virtual void Heal(float amount)
	{
		//Increase health with amount got heal
		health += amount;
		//Call heal event with damage has take
		onHeal?.Invoke(amount);
		//Cap health from going beyond final max health
		health = Mathf.Clamp(health, 0, finalMaxHP);
		//Play the heal sound when this entity get heal if needed
		if(healSound != null) SessionOperator.i.audios.soundSource.PlayOneShot(healSound);
	}

	public virtual void Die()
	{
		//Play the die sound upon entity die if needed
		if(dieSound != null) SessionOperator.i.audios.soundSource.PlayOneShot(dieSound);
		//Reset the final max hp to 0
		finalMaxHP -= finalMaxHP;
	}
}
