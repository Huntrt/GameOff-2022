using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AudioControl : MonoBehaviour
{
	public TextMeshProUGUI display;
	public Slider slider;
	enum VolumeType {master, sound, music} [SerializeField] VolumeType type;
	AudioOperator manager;

	void Start()
	{
		//Get the audio manager
		manager = SessionOperator.i.audios;
		UpdateDisplay();
	}

	void OnEnable() {UpdateDisplay();}

	public void ModifyVolume(int value)
	{
		//Don't modify if there no manager
		if(manager == null) return;
		//If this control are master type
		if(type == VolumeType.master)
		{
			//Increase the master volume with value that been clamp as 0 -> 100
			manager.SetMaster(Mathf.Clamp(manager.masterVolume + value,0,100));
			//Set master volume as rounded slider value if value are 0
			if(value == 0) {manager.SetMaster(Mathf.Round(slider.value * 100));}
		}
		//If this control are sound type
		else if(type == VolumeType.sound)
		{
			//Increase the sound volume with value that been clamp as 0 -> 100
			manager.SetSound(Mathf.Clamp(manager.soundVolume + value,0,100));
			//Set sound volume as rounded slider value if value are 0
			if(value == 0) {manager.SetSound(Mathf.Round(slider.value * 100));}
		}
		//If this control are music type
		else if(type == VolumeType.music)
		{
			//Increase the music volume with value that been clamp as 0 -> 100
			manager.SetMusic(Mathf.Clamp(manager.musicVolume + value,0,100));
			//Set music volume as rounded slider value if value are 0
			if(value == 0) {manager.SetMusic(Mathf.Round(slider.value * 100));}
		}
		//Update both display and slider
		UpdateDisplay(); UpdateSlider();
	}

	void UpdateDisplay()
	{
		//Don't update if there no manager
		if(manager == null) return;
		//@ Set the display text as manager volume then update slider
		if(type == VolumeType.master) {display.text = "Master: "+manager.masterVolume; UpdateSlider();}
		if(type == VolumeType.sound) {display.text = "Sound: "+manager.soundVolume; UpdateSlider();}
		if(type == VolumeType.music) {display.text = "Music: "+manager.musicVolume; UpdateSlider();}
	}

	void UpdateSlider()
	{
		//Don't update if there no manager
		if(manager == null) return;
		//@ Update slider value to be manager volume
		if(type == VolumeType.master) {slider.value = (float)manager.masterVolume/100;}
		if(type == VolumeType.sound) {slider.value = (float)manager.soundVolume/100;}
		if(type == VolumeType.music) {slider.value = (float)manager.musicVolume/100;}
	}
}