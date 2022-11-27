using UnityEngine.UI;
using UnityEngine;

public class FullscreenSetter : MonoBehaviour
{
	//! FULLSCREEN SETTING ARE SAVED ACROSS GAME INSTANCE
	[SerializeField] Toggle fullscreenToggeler;
	
    void Start()
    {
		//Set full screen toggle status base on full screen state
		fullscreenToggeler.SetIsOnWithoutNotify(Screen.fullScreen);
    }

	//Toggle between full screen upon call
	public void ToggleFullscreen() {Screen.fullScreen = !Screen.fullScreen;}
}
