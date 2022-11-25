using UnityEngine;

public class DaysManager : MonoBehaviour
{
	#region Set this class to singleton
	public static DaysManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<DaysManager>();}return _i;}} static DaysManager _i;
	#endregion

	public int passes; //How many day has pass
	public float duration; //How long an day in second
	[Range(0,1)] public float progress; //How many percent of an day has progress
	public delegate void OnCycle(bool night); public OnCycle onCycle; //When day cycle between night and moring
	public bool isNight; //Is the day nightime now?
	float dayTimer; //How many second has pass in one day

	void Start()
	{
		//Begin at the morning of day 0
		isNight = false; onCycle?.Invoke(false);
	}
	
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
		//If has reach half day but still havent night time then cycle to night
		if(progress >= 0.5f && !isNight) {isNight = true; onCycle?.Invoke(true);}
		//If timer has rached duration
		if(dayTimer >= duration)
		{
			//Another day has pass
			passes++;
			//Reset progress and timer
			progress = 0; dayTimer -= dayTimer;
			//Day has cycle back to the morning
			isNight = false; onCycle?.Invoke(false);
		}
	}
}
