using UnityEngine;

public class Ground : MonoBehaviour
{
	public Vector2 initalSize;
	public int groundLeft, groundRight, fill;
	public GameObject dirtPrefab, fillerPrefab; Transform grouper;

	void Start()
	{
		InitializeGround();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.DownArrow)) ExpandGround(0);
		if(Input.GetKeyDown(KeyCode.RightArrow)) ExpandGround(1);
		if(Input.GetKeyDown(KeyCode.LeftArrow)) ExpandGround(-1);
	}

	void InitializeGround()
	{
		//Renew the group grouper
		if(grouper != null) {Destroy(grouper);} grouper = new GameObject().transform; grouper.name = "Grounds";
		//Create an dirt at center
		CreateDirt(0);
		//Go through all the width need to create
		for (int x = 1; x <= initalSize.x; x++)
		{
			//Convert width with spacing
			float setWidth = Map.Spaced(x);
			//Create dirt for both side on this width
			CreateDirt(setWidth); CreateDirt(-setWidth);
		}
	}

	/// 1 To expand to the right | -1 To expand to the left | 0 to expand both side
	public void ExpandGround(int direction)
	{
		if(direction < -1 || direction > 1)
		{
			Debug.LogError("Cant exoand the ground in ["+direction+"] direction");
			return;
		}
		if(direction == 0)
		{
			CreateDirt(Map.Spaced(groundRight+1));
			CreateDirt(Map.Spaced(-groundLeft-1));
		}
		else
		{
			if(direction == +1) {CreateDirt(Map.Spaced((groundRight+1)));}
			if(direction == -1) {CreateDirt(Map.Spaced((-groundLeft-1)));}
		}
	}

	void CreateDirt(float widthPos)
	{
		//Extend an plot on the map for the dirt at width given and block it
		GameObject dirt = Map.ExtendPlot(dirtPrefab, new Vector2(widthPos, initalSize.y), 3);
		//Extend an empty plot above the dirt created
		Map.ExtendPlot(null, new Vector2(widthPos, initalSize.y + Map.i.spacing), 0);
		//Group then rename the dirt created
		dirt.transform.SetParent(grouper); dirt.name = widthPos + " -  Dirt";
		//Go through all the time need to fill this ground
		for (int y = 1; y <= fill; y++)
		{
			//Get position to place filler
			Vector2 fillPos = new Vector2(widthPos, initalSize.y - Map.Spaced(y));
			//Create the filler object the save it
			GameObject filler = Instantiate(fillerPrefab, fillPos, Quaternion.identity);
			//Group then rename the filler created
			filler.transform.SetParent(grouper); filler.name = y + " - Filler";
		}
		//Ground increase to the right if width pos are positive and opposite if negative
		if(widthPos > 0) groundRight++; if(widthPos < 0) groundLeft++;
	}
}