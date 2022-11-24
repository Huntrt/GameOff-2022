using UnityEngine;

public class DaysManager : MonoBehaviour
{
	#region Set this class to singleton
	public static DaysManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<DaysManager>();}return _i;}} static DaysManager _i;
	#endregion

	public int counter; //How many day has pass
	public float duration; //How long an day in second
	[Range(0,1)] public float progress; //How many percent of an day has progress
	float dayTimer; //How many second has pass in one day

	void Update()
	{
		TimingDay();
	}

	void TimingDay()
	{
		//Counting timer by second
		dayTimer += Time.deltaTime;
		//Get % progress of an day
		progress = dayTimer/duration;
		//If timer has rached duration
		if(dayTimer >= duration)
		{
			//Another day has pass
			counter++;
			//Reset progress and timer
			progress = 0; dayTimer -= dayTimer;
		}
	}
}
