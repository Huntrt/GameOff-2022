using UnityEngine;
using System;
using UnityEngine.Serialization;

public class Tower_Strike : MonoBehaviour
{
	public Tower_Caster caster;
	public OnHit onHit;
	public delegate void OnHit(Entity entity, Vector2 pos);
	public OnEnd onOver, onDespawn;
	public delegate void OnEnd(Vector2 pos);
	[SerializeField] GameObject hitEffect, despawnEffect;
    [HideInInspector] public float damage; 
	[Header("Stats")] public float accuracy;

	protected virtual void OnEnable()
	{
		//The ange has get from randomize accuracy
		float accurate = UnityEngine.Random.Range(-accuracy, accuracy);
		//Rotate the strike initial rotation with accurate has get
		transform.localRotation = Quaternion.Euler(0,0, accurate + transform.localEulerAngles.z);
	}

	public virtual void Hurting(float amount, GameObject entity, Vector2 contact, bool callHit = true)
	{
		//Heal the entity got hurt
		Entity hurted = entity.GetComponent<Entity>();
		//Hurt the enemy with damage has
		hurted.Hurt(amount);
		//Called hitting if needed
		if(callHit) Hitting(hurted, contact);
	}

	public virtual void Healing(float amount, GameObject entity, Vector2 contact, bool callHit = true)
	{
		Entity healed = entity.GetComponent<Entity>();
		//Heal the enemy with damage has
		healed.Heal(amount);
		//Called hitting if needed
		if(callHit) Hitting(healed, contact);
	}

	void Hitting(Entity hit, Vector2 contact)
	{
		onHit?.Invoke(hit, contact);
		//Create hit effect at contact pos with the parent as pooler itself
		if(hitEffect != null) Pooler.i.Create(hitEffect, contact, Quaternion.identity, true, Pooler.i.transform);
	}
	
	/// The strike end cause by out of interaction
	public virtual void Over(Vector2 overPos, float delay = 0)
	{
		onOver?.Invoke(overPos);
		Invoke("Ended", delay);
	}

	/// The strike end while still having left over interaction
	public virtual void Despawn(Vector2 despawnPos, float delay = 0) 
	{
		onDespawn?.Invoke(despawnPos);
		//Create despawn effect at despawn pos with the parent as pooler itself
		if(despawnEffect != null) Pooler.i.Create(despawnEffect, despawnPos, Quaternion.identity, true, Pooler.i.transform);
		Invoke("Ended", delay);
	}

	void Ended()
	{
		//Destroy the strike if it caster no longer exist
		if(caster == null) {Destroy(gameObject); return;}
		//Deactive the strike
		gameObject.SetActive(false);
		//The strike of caster are now over
		caster.StrikeOver(this);
	}
}
