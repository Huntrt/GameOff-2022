using UnityEngine.UI;
using UnityEngine;

public class HealthOverlay : MonoBehaviour
{
    public Entity entity;
    [SerializeField] Image healthbar;

	void OnEnable()
	{
		//Stop has no entity to display
		if(entity == null) return;
		//Overlay will alway follow set entity
		transform.position = entity.transform.position;
	}

	void Update()
	{
		//Deactive the overlay if it has no entity to display
		if(entity == null) {gameObject.SetActive(false); return;}
		//Fill health bar with set entity health
		healthbar.fillAmount = entity.health / entity.maxHealth;
	}
}