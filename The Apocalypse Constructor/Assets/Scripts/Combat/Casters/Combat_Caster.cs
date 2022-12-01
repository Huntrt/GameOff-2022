using System.Collections.Generic;
using UnityEngine;
using System;

public class Combat_Caster : MonoBehaviour
{
	[SerializeField] Combats.Stats initialStats; public Combats.Stats InitialStats {get => initialStats;}
	[SerializeField] Combats.Stats growthStats; public Combats.Stats GrowthStats {get => growthStats;}
	public Combats.Stats finalStats; float countRate;
	[SerializeField] AttackAnimation attackAnimation;
	[Serializable] class AttackAnimation
	{
		public Animator animator;
		public float scaleWindupWithRate;
		public bool allowSlowdown, currentAnimating;
	}
	[SerializeField] AudioClip attackSound;
	[Tooltip("Player each strike instead of single attack")] [SerializeField] bool soundOnStrike;
	public LayerMask combatLayer;
	public bool detected;
	bool inverted; public bool Inverted {get {return inverted;}}
	public Action onStrike;
	[HideInInspector] public List<Combat_Strike> strikes = new List<Combat_Strike>();

	public virtual void InvertingCaster(bool isInvert, bool dontAdjustPoint = false)
	{
		//Caster has been flip like given
		inverted = isInvert;
	}

	void Update()
	{
		//Attacking when detect an enemy and while attack animtion not running
		if(detected && !attackAnimation.currentAnimating) Attacking();
	}

	void Attacking()
	{
		//Counting speed for attack
		countRate += Time.deltaTime;
		//If has count enough rate
		if(countRate >= (1/finalStats.rate))
		{
			//If there is attack animator
			if(attackAnimation.animator != null)
			{
				//Get scaled value by multiply stats rate with set windup scale
				float scaled = finalStats.rate * attackAnimation.scaleWindupWithRate;
				//Lock scale to 1 if not allow it to slow down windup speed
				if(scaled < 1 && !attackAnimation.allowSlowdown) {scaled = 1;}
				//Set windup float as scaled value
				attackAnimation.animator.SetFloat("Windup", scaled);
				//STart attack trigger in animator
				attackAnimation.animator.SetTrigger("Attack");
				//Has begin animating attack
				attackAnimation.currentAnimating = true;
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

	protected virtual void Attack() 
	{
		//No longer animating attack
		attackAnimation.currentAnimating = false;
		//Player the caster's attack sound if needed and strike not playing sound
		if(attackSound != null && !soundOnStrike) SessionOperator.i.audios.soundSource.PlayOneShot(attackSound);
	}

	protected virtual Combat_Strike Striking(GameObject strikeObj, Vector2 pos, Quaternion rot)
	{
		//Play attack sound when strike if wanted
		if(soundOnStrike) SessionOperator.i.audios.soundSource.PlayOneShot(attackSound);
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
		strike.damage = finalStats.damage;
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