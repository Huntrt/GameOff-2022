using System.Collections.Generic;
using UnityEngine;
using System;

public class Combat_Caster : MonoBehaviour
{
	[SerializeField] AttackAnimation attackAnimation; [Serializable] class AttackAnimation
	{
		public Animator animator;
		public float scaleWindupWithRate;
		public bool allowSlowdown;
	}
	[Header("General")]
	public LayerMask combatLayer;
	public bool detected;
	public Combats.Stats stats, growth;
	public Action onStrike;
	[HideInInspector] public bool flipped;
	[HideInInspector] public List<Combat_Strike> strikes = new List<Combat_Strike>();

	void OnEnable()
	{
		//Get the stats for caster on this structure
		if(gameObject.CompareTag("Tower"))stats = GetComponent<Tower>().stats;
		//Get the stats for caster on this structure
		if(gameObject.CompareTag("Enemy"))stats = GetComponent<Enemy>().stats;
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
			//If there is attack animator
			if(attackAnimation.animator != null)
			{
				//Get scaled value by multiply stats rate with set windup scale
				float scaled = stats.rate * attackAnimation.scaleWindupWithRate;
				//Lock scale to 1 if not allow it to slow down windup speed
				if(scaled < 1 && !attackAnimation.allowSlowdown) {scaled = 1;}
				//Set windup float as scaled value
				attackAnimation.animator.SetFloat("Windup", scaled);
				//STart attack trigger in animator
				attackAnimation.animator.SetTrigger("Attack");
			}
			//If there no attack animator
			else
			{
				//Manually attack
				Attack();
			}
			//Reset speed counter
			countRate -= countRate;
		}
	}

	protected virtual void Attack() {}

	protected virtual Combat_Strike Striking(GameObject strikeObj, Vector2 pos, Quaternion rot)
	{
		//Call on strike when create one
		onStrike?.Invoke();
		//Go through all the strike has cache
		for (int s = 0; s < strikes.Count; s++)
		{
			//Get this strike has cache
			Combat_Strike strike = strikes[s];
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
		Combat_Strike newStrike = Instantiate(strikeObj).GetComponent<Combat_Strike>();
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

	void SetupStrike(Combat_Strike strike, Vector2 pos, Quaternion rot)
	{
		//Set the strike damage to be the caster damage
		strike.damage = stats.damage;
		//Set the strike to be given position and rotation
		strike.transform.position = pos;
		strike.transform.rotation = rot;
	}

	public void StrikeOver(Combat_Strike strike)
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