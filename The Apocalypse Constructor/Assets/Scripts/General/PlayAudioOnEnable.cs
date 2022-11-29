using UnityEngine;

public class PlayAudioOnEnable : MonoBehaviour
{
	[SerializeField] AudioClip sound; 

    void OnEnable() {SessionOperator.i.audios.soundSource.PlayOneShot(sound);}
}