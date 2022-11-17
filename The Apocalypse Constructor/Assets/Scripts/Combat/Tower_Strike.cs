using UnityEngine;
using System;

public class Tower_Strike : MonoBehaviour
{
	public Tower_Caster caster;
	public Action onOver;
    [HideInInspector] public float damage; 
	public float accuracy;

	protected virtual void OnEnable()
	{
		//The ange has get from randomize accuracy
		float accurate = UnityEngine.Random.Range(-accuracy, accuracy);
		//Rotate the strike initial rotation with accurate has get
		transform.localRotation = Quaternion.Euler(0,0, accurate + transform.localEulerAngles.z);
	}

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
