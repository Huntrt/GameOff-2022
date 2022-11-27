using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other) 
	{
		Entity entity = other.collider.GetComponent<Entity>();
		if(entity != null) entity.Die();
	}
}