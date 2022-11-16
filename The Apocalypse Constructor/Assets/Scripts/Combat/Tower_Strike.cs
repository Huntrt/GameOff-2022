using UnityEngine;
using System;

public class Tower_Strike : MonoBehaviour
{
	public Tower_Caster caster;
	public Action onOver;
    public float damage;

	//Hurt the given entity with strike damage
	public virtual void Hurting(GameObject entity)
	{
		entity.GetComponent<Entity>().Hurt(damage);
	}

	//Heal the given entity with strike damage
	public virtual void Healing(GameObject entity)
	{
		entity.GetComponent<Entity>().Heal(damage);
	}
	
	//When the strike are over
	public virtual void Over()
	{
		//Call over event
		onOver?.Invoke();
		//Destroy the strike if it caster no longer exist
		if(caster == null) {Destroy(gameObject); return;}
		//Deactive the strike
		gameObject.SetActive(false);
		//The strike of caster are now over
		caster.StrikeOver(this);
	}
}
