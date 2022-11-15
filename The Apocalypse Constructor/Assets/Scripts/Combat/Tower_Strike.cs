using UnityEngine;

public class Tower_Strike : MonoBehaviour
{
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
}
