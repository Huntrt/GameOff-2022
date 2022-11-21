using System.Collections.Generic;
using UnityEngine;
using System;

public class Tower_Caster : MonoBehaviour
{
	public LayerMask combatLayer;
	public Combats.Stats stats;
	public bool detected;
	public Action onStrike;
	[HideInInspector] public bool flipped;

	//Cache all the strike this caster has create
	public List<Tower_Strike> strikes = new List<Tower_Strike>();

	void OnEnable()
	{
		//Get the stats of caster on this caster
		stats = GetComponent<Tower>().stats;
	}

	void Update()
	{
		//Attacking when detect an enemy
		if(detected) Attacking();
	}

	float countRate;
	void Attacking()
	{
		//Counting speed for attack
		countRate += Time.deltaTime;
		//If has count enough rate
		if(countRate >= (stats.rateTimer))
		{
			//Begin attack
			Attack();
			//Reset speed counter
			countRate -= countRate;
		}
	}

	protected virtual void Attack() {}

	protected virtual Tower_Strike Striking(GameObject strikeObj, Vector2 pos, Quaternion rot)
	{
		//Call on strike when create one
		onStrike?.Invoke();
		//Go through all the strike has cache
		for (int s = 0; s < strikes.Count; s++)
		{
			//Get this strike has cache
			Tower_Strike strike = strikes[s];
			//If this cached strike is unactive
			if(!strike.gameObject.activeInHierarchy)
			{
				//Set up the inactive strike
				SetupStrike(strike, pos, rot);
				//Active the strike then return it
				strike.gameObject.SetActive(true); return strike;
			}
		}

		///When need more strike
		//Create an new strike at given transform then cache the strike component
		Tower_Strike newStrike = Instantiate(strikeObj).GetComponent<Tower_Strike>();
		//Deactive the new strike for waiting to assign it stats
		newStrike.gameObject.SetActive(false);
		//Set new strike caster to be this caster
		newStrike.caster = this;
		//Setup the newly created strike
		SetupStrike(newStrike, pos, rot);
		//Cache the newly create strike
		strikes.Add(newStrike);
		//Active the new strike then return it
		newStrike.gameObject.SetActive(true); return newStrike;
	}

	void SetupStrike(Tower_Strike strike, Vector2 pos, Quaternion rot)
	{
		//Set the strike damage to be the caster damage
		strike.damage = stats.damage;
		//Set the strike to be given position and rotation
		strike.transform.position = pos;
		strike.transform.rotation = rot;
	}

	public void StrikeOver(Tower_Strike strike)
	{
		//Move the strike over to be first cache since it now available
		strikes.Insert(0, strike); strikes.Remove(strike);
	}

	void OnDestroy()
	{
		//Go through all the strike has cast
		for (int s = 0; s < strikes.Count; s++)
		{
			//Skip if the strike already null
			if(strikes[s] == null) return;
			//Destroy any strike the currently inactive
			if(!strikes[s].gameObject.activeInHierarchy) Destroy(strikes[s].gameObject); 
		}
		strikes.Clear();
	}
}