using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayEnemyRange : MonoBehaviour
{
	[SerializeField] Combat_Aiming aim;
	[SerializeField] Combat_Caster caster;
	Transform indicator;

	void Reset() 
	{
		aim = GetComponent<Combat_Aiming>();
		caster = GetComponent<Combat_Caster>();
	}

    void OnEnable()
	{
		//@ Choose the indicator base on sighting mode
		if(aim.mode == Combat_Aiming.Mode.Direct) indicator = EnemiesManager.i.directSightIndicator.transform;
		else if(aim.mode == Combat_Aiming.Mode.Rotate) indicator = EnemiesManager.i.rotateSightIndicator.transform;
		//Create the indicator has choose at this enemy position
		indicator = Instantiate(indicator, transform.position, Quaternion.identity).transform;
		//Parent the indicator onto enemy
		indicator.SetParent(transform);
		//If indicator are direct mode
		if(aim.mode == Combat_Aiming.Mode.Direct)
		{
			//Set the indicator scale as mover vision
			indicator.localScale = new Vector2(caster.finalStats.range, Map.i.spacing);
			//Adjust the indicator vision by moving it half of vision and block size
			indicator.localPosition = new Vector2(((caster.finalStats.range/2) + (Map.i.spacing/2)), 0);
		}
		//If indicator are rotate node then indicator radius are double mover vision
		else if(aim.mode == Combat_Aiming.Mode.Rotate)
		{
			indicator.transform.localScale = new Vector2(caster.finalStats.range*2, caster.finalStats.range*2);
		}
	}
}
