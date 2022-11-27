using UnityEngine;

//? Operate the whole game
public class SessionOperator : MonoBehaviour
{
	public AudioOperator audios;
	public KeyOperator keys;
	public static SessionOperator i;

	void Awake()
	{
		//Only set to "don't destroy on load" if haven't then destroy any duplicate
		if(i == null) {i = this; DontDestroyOnLoad(this);} else {Destroy(gameObject);}
	}
}