using System.Collections.Generic;
using UnityEngine;

public class Tower_Caster : MonoBehaviour
{
	Tower tower;
	//Cache all the strike this caster has create
	public List<Tower_Strike> strikes;

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

	protected virtual Tower_Strike CreateStrike(GameObject striking, Transform at)
	{
		//Go through all the strike has cache
		for (int s = 0; s < strikes.Count; s++)
		{
			//Get this strike has cache
			Tower_Strike strike = strikes[s];
			//If this cached strike is unactive
			if(!strike.gameObject.activeInHierarchy)
			{
				//Set the strike damage to be the tower damage
				strike.damage = tower.damage;
				//Set the strike position and rotation to be given transform
				strike.transform.position = at.position;
				strike.transform.rotation = at.rotation;
				//Active the strike then return it
				strike.gameObject.SetActive(true); return strike;
			}
		}

		///When need more strike
		//Create an new strike at given transform then cache the strike component
		Tower_Strike newStrike = Instantiate(striking).GetComponent<Tower_Strike>();
		//Deactive the new strike for waiting to assign it stats
		newStrike.gameObject.SetActive(false);
		//Set new strike caster to be this caster
		newStrike.caster = this;
		//Set the new strike damage to be the tower damage
		newStrike.damage = tower.damage;
		//Set the new strike position and rotation to be given transform
		newStrike.transform.position = at.position;
		newStrike.transform.rotation = at.rotation;
		//Cache the newly create strike
		strikes.Add(newStrike);
		//Active the new strike then return it
		newStrike.gameObject.SetActive(true); return newStrike;
	}

	public void StrikeOver(Tower_Strike strike)
	{
		//Move the strike over to be first cache since it now available
		strikes.Insert(0, strike); strikes.Remove(strike);
	}
}