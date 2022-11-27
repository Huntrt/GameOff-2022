using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSetter : MonoBehaviour
{
	//! RESOLUTION ARE SAVED ACROSS GAME INSTANCE
	[SerializeField] TMP_Dropdown dropdown;
	[SerializeField] List<string> resolutions;

	void Start()
	{
		//The index of screen size currently use
		int use = 0; 
		//Get the resolution currently use
		Resolution cur = Screen.currentResolution;
		//Go through all the available resolution
		for (int r = 0; r < Screen.resolutions.Length; r++)
		{
			//Save this available resolution as string
			resolutions.Add(Res(r).width + " x " + Res(r).height);
			/// Get this index if this resolution match with RESOLUTION currently use in full screen
			if(Screen.fullScreen && Res(r).width == cur.width && Res(r).height == cur.height) use = r;
			/// Get this index if this resolution match with WINDOW SIZE currently use in windowed
			if(!Screen.fullScreen && Res(r).width == Screen.width && Res(r).height == Screen.height) use = r;
		}
		//Add all the resolution string to drop down
		dropdown.AddOptions(resolutions);
		//Set dropdown default value as screen size currently use
		dropdown.value = use;
	}

	public void ChangeResolution(int c)
	{
		//Set resolution base on choosed index and use currently full screen state
		Screen.SetResolution(Res(c).width, Res(c).height, Screen.fullScreen, Res(c).refreshRate);
	}

	//Get resolution at index
	Resolution Res(int i) {return Screen.resolutions[i];}
}