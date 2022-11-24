using System.Collections;
using UnityEngine;

public class DayVisual : MonoBehaviour
{
    DaysManager day;
	[Header("Skybox")]
	Camera skybox;
	[SerializeField] float cycleSpeed, cycleDuration; [SerializeField]float cycleTimer;
	[SerializeField] Color morningColor, nightColor;

	void OnEnable()
	{
		day = DaysManager.i;
		skybox = Camera.main;
		day.onCycle += WhenCycleChange;
	}

	
	void WhenCycleChange(bool night)
	{
		//Begin cycle skybox toward night/morning color base on given state
		StartCoroutine(SkyboxCycle((night) ? nightColor : morningColor));
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
		print("**");
	}

	
	void OnDisable()
	{
		day.onCycle -= WhenCycleChange;
	}
}