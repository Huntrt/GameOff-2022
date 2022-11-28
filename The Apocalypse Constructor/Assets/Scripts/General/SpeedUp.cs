using UnityEngine;

public class SpeedUp : MonoBehaviour
{
	bool speeded;

    public void SpeedingUp()
	{
		//Set time scale by 4x base on speeded or not
		speeded = !speeded; Time.timeScale = (speeded)? 4 : 1;
	}
}
