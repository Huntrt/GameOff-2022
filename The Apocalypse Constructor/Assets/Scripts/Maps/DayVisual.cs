using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DayVisual : MonoBehaviour
{
    DaysManager days;
	[SerializeField] Ground ground;
	[Header("Background")]
	[SerializeField] Animator dayAnimator;
	[SerializeField] ParticleSystem[] mountains;
	[SerializeField] Transform clouds;
	[SerializeField] ParticleSystem stars;
	[Header("GUI")]
	[SerializeField] TextMeshProUGUI dayCounterText;
	[SerializeField] Image cycleProgressBar;
	[SerializeField] GameObject skipMorningButton;

	void OnEnable()
	{
		days = DaysManager.i;
		days.onCycle += WhenCycleChange;
		ground.onExpand += WhenGroundExpand;
	}

	void Update()
	{
		UpdateProgressBar();
	}
	
	void WhenCycleChange(bool night)
	{
		//Animated day to be the given morning/night cycle
		dayAnimator.SetBool("To Night", night);
		//Update the day counter when it morning
		if(!night) dayCounterText.text = "" + days.passes;
		//Swwitch skip morning button active base on cycle
		skipMorningButton.SetActive(!night);
	}

	void WhenGroundExpand()
	{
		int longestGround = ground.LongestGroundWay();
		UpdateMoutains(longestGround);
		UpdateClouds();
		UpdateStars(longestGround);
	}

	void UpdateMoutains(float longestGround)
	{
		//Go through all the mountains background
		for (int b = 0; b < mountains.Length; b++)
		{
			//Scale this mountains size as the longest ground
			mountains[b].transform.localScale = new Vector2(longestGround, 1);
			//Get the burst of particle system in this mountain
			ParticleSystem.Burst burst = mountains[b].emission.GetBurst(0);
			//Set the amount of burst the same as longest ground
			burst.count = longestGround;
			//@ Refresh particle system
			mountains[b].emission.SetBurst(0, burst);
			mountains[b].Clear();
			mountains[b].Play();
		}
	}

	void UpdateClouds()
	{
		//Move the cloud to the alway be right side of the map
		clouds.position = new Vector2(Map.Spaced(ground.groundRight), clouds.position.y);
	}

	void UpdateStars(float longestGround)
	{
		//Scale this stars size X as longest ground
		stars.transform.localScale = new Vector2(longestGround, stars.transform.localScale.y);
		//Get the buremissionst of particle system in stars
		ParticleSystem.EmissionModule emission = stars.emission;
		//Set the emission rate the same as longest ground
		emission.rateOverTime = longestGround;
	}

	void UpdateProgressBar()
	{
		//Get progress of an cycle
		float cycleProgress = days.progress * 2;
		//Set fill amount as cycle progress that shifted base on whole progress
		cycleProgressBar.fillAmount = (cycleProgress >= 1) ? cycleProgress-1 : cycleProgress;;
	}

	void OnDisable()
	{
		days.onCycle -= WhenCycleChange;
		ground.onExpand -= WhenGroundExpand;
	}
}