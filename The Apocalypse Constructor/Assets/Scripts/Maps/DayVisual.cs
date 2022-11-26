using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DayVisual : MonoBehaviour
{
    DaysManager days;
	[SerializeField] Ground ground;
	[Header("Skybox")]
	Camera skybox;
	[SerializeField] float cycleSpeed, cycleDuration; [SerializeField]float cycleTimer;
	[SerializeField] Color morningColor, nightColor;
	[Header("Background")]
	[SerializeField] ParticleSystem[] mountains;
	[SerializeField] Transform clouds;
	[SerializeField] ParticleSystem stars;
	[Header("GUI")]
	[SerializeField] TextMeshProUGUI dayCounterText;
	[SerializeField] Image cycleProgressBar;

	void OnEnable()
	{
		days = DaysManager.i;
		skybox = Camera.main;
		days.onCycle += WhenCycleChange;
		ground.onExpand += WhenGroundExpand;
	}

	void Update()
	{
		UpdateProgressBar();
	}
	
	void WhenCycleChange(bool night)
	{
		//Get the given cycle color
		Color cycleColor = (night) ? nightColor : morningColor;
		//Begin transition skybox color toward cycle color
		StartCoroutine(SkyboxCycle(cycleColor));
		//Update the day counter when it morning
		if(!night) dayCounterText.text = "" + days.passes;
	}

	IEnumerator SkyboxCycle(Color cycleColor)
	{
		//Get current color of skybox
		Color curColor = skybox.backgroundColor;
		//Reset the cycle timer
		cycleTimer -= cycleTimer;
		//While cycle timer havent reached duration
		while(cycleTimer <= cycleDuration)
		{
			//Counting cycle timer with speed
			cycleTimer += Time.deltaTime * cycleSpeed;
			//Lerp skybox color from current color to color will be cycle
			skybox.backgroundColor = Color.Lerp(curColor, cycleColor, cycleTimer/cycleDuration);
			yield return null;
		}
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