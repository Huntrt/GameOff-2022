using UnityEngine;

public class AudioOperator : MonoBehaviour
{
	public float masterVolume, soundVolume, musicVolume;
	public AudioSource soundSource, musicSource;

	void Awake()
	{
		//Updated the master, sound and music volume value scene open
		SetMaster(masterVolume); SetSound(soundVolume); SetMusic(musicVolume);
	}

	public void SetMaster(float value)
	{
		//Get master volume at given value 
		masterVolume = value; 
		//Apply master volume to audio listener
		AudioListener.volume = 1;
	}

	public void SetSound(float value)
	{
		//Get sound volume at given value 
		soundVolume = value; 
		//Apply sound volume to audio source
		soundSource.volume = soundVolume/100;
	}

	public void SetMusic(float value)
	{
		//Get music volume at given value 
		musicVolume = value; 
		//Apply music volume to audio source
		musicSource.volume = musicVolume/100;
	}
}
