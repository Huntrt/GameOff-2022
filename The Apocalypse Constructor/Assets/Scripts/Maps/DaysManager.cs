using UnityEngine;

public class DaysManager : MonoBehaviour
{
	#region Set this class to singleton
	public static DaysManager i {get{if(_i==null){_i = GameObject.FindObjectOfType<DaysManager>();}return _i;}} static DaysManager _i;
	#endregion

	public int passes; //How many day has pass
	public float duration; //How long an day in second
	float dayTimer;
	[Range(0,1)] public float progress; //How many percent of an day has progress
	public delegate void OnCycle(bool night); public OnCycle onCycle;
	public bool isNight;
	 
	[SerializeField] int breakEveryDay;
	bool isInBreak = true;

	void Start()
	{
		//Begin at the morning of day 0
		isNight = false; onCycle?.Invoke(false);
	}
	
	void Update()
	{
		//If this is not in break
		if(!isInBreak) 
		{
			//Begin timing day
			TimingDay();
		}
	}

	void TimingDay()
	{
		//Counting timer by second
		dayTimer += Time.deltaTime;
		//Get % progress of an day
		progress = dayTimer/duration;
		//If has reach half day but still havent night time then start to night
		if(progress >= 0.5f && !isNight) {StartNight();}
		//If timer has reached duration
		if(dayTimer >= duration)
		{
			//Another day has pass
			passes++;
			//Reset progress and timer
			progress = 0; dayTimer -= dayTimer;
			//Start to morning
			StartMorning();
			//Enter break when passes the day needed to
			if(passes % breakEveryDay == 0) isInBreak = true;
		}
	}

	void StartNight()
	{
		//Day has cycle to the night
		isNight = true; onCycle?.Invoke(true);
	}

	void StartMorning()
	{
		//Day has cycle back to the morning
		isNight = false; onCycle?.Invoke(false);
	}

	public void SkipMorning() 
	{
		dayTimer = duration/2;
		isInBreak = false;
	}
}