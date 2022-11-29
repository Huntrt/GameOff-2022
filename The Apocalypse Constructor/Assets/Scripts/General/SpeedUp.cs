using UnityEngine;

public class SpeedUp : MonoBehaviour
{
	bool speeded;
	[SerializeField] ToggleGraphicColor graphicToggler;

	void Update()
	{
		//If press speed up key
		if(Input.GetKeyDown(KeyOperator.i.SpeedUp))
		{
			//Speed the game up
			SpeedingUp();
			//Toggle the graphic
			graphicToggler.Toggling();
		}
		
	}

    public void SpeedingUp()
	{
		//Set time scale by 4x base on speeded or not
		speeded = !speeded; Time.timeScale = (speeded)? 4 : 1;
	}
}