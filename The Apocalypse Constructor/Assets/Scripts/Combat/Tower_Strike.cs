using UnityEngine;
using System;

public class Tower_Strike : MonoBehaviour
{
	public Tower_Caster caster;
	public Action onOver;
	public OnHit onHit;
	public delegate void OnHit(Entity entity, Vector2 pos);
    [HideInInspector] public float damage; 
	public float accuracy;

	protected virtual void OnEnable()
	{
		//The ange has get from randomize accuracy
		float accurate = UnityEngine.Random.Range(-accuracy, accuracy);
		//Rotate the strike initial rotation with accurate has get
		transform.localRotation = Quaternion.Euler(0,0, accurate + transform.localEulerAngles.z);
	}

	public virtual void Hurting(GameObject entity, Vector2 contact, bool silent = false)
	{
		//Heal the entity got hurt
		Entity hurted = entity.GetComponent<Entity>();
		//Hurt the enemy with damage has
		hurted.Hurt(damage);
		//Called onhit event if needed
		if(!silent) onHit?.Invoke(hurted, contact);
	}

	public virtual void Healing(GameObject entity, Vector2 contact, bool silent = false)
	{
		Entity healed = entity.GetComponent<Entity>();
		//Heal the enemy with damage has
		healed.Heal(damage);
		//Called onhit event if needed
		if(!silent) onHit?.Invoke(healed, contact);
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
