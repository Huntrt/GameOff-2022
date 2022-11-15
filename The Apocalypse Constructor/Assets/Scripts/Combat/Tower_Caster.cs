using UnityEngine;

public class Tower_Caster : MonoBehaviour
{
	Tower tower;

	void Reset() 
	{
		//Get the tower component the moment caster get added
		tower = GetComponent<Tower>();
		//Print error if the object dont has tower for caster
		if(tower == null) Debug.LogError(gameObject.name + " aiming need to be an tower");
	}

	void OnEnable() {tower.onAttack += Attack;}

	protected virtual void Attack() {}

	void OnDisable() {tower.onAttack += Attack;}
}