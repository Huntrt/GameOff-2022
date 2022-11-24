using UnityEngine;

public class Ground : MonoBehaviour
{
	public Vector2 initalSize;
	public int groundLeft, groundRight, fill;
	public GameObject dirtPrefab, fillerPrefab;
	public System.Action onExpand;
	[SerializeField] Transform grouper;

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
		//Create an dirt at center
		CreateDirt(0);
		//Go through all the width need to create
		for (int x = 1; x <= initalSize.x; x++)
		{
			//Convert width with spacing
			float setWidth = Map.Spaced(x);
			//Expand both side of the map
			ExpandGround(0);
		}
	}

	public int LongestGroundWay()
	{
		//Return depend on which way of ground the longer
		if((Mathf.Abs(groundLeft) > groundRight)) return groundLeft; return groundRight;
	}

	/// 1 To expand to the right | -1 To expand to the left | 0 to expand both side
	public void ExpandGround(int direction)
	{
		if(direction < -1 || direction > 1)
		{
			Debug.LogError("Cant expand the ground in ["+direction+"] direction");
			return;
		}
		//If wanted to expand both way
		if(direction == 0)
		{
			//Create dirt in both direction
			CreateDirt(Map.Spaced(groundRight+1));
			CreateDirt(Map.Spaced(groundLeft-1));
		}
		//If only wanted to expand in an single direction
		else
		{
			//Get the way of ground left or right base on given direction
			int groundWay = (direction == 1)? groundRight : groundLeft;
			//Create dirt in the given direction with way has get
			CreateDirt(Map.Spaced(groundWay+(1 * direction)));
		}
		onExpand?.Invoke();
	}

	void CreateDirt(float widthPos)
	{
		//Extend an empty plot on the map at given position
		Map.ExtendPlot(new Vector2(widthPos, initalSize.y), 0);
		//Placing the blocked dirt prefab at position just extend
		GameObject dirt = Map.Placing(dirtPrefab, new Vector2(widthPos, initalSize.y), 3);
		//Extend an empty plot above the dirt created
		Map.ExtendPlot(new Vector2(widthPos, initalSize.y + Map.i.spacing), 0);
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
		if(widthPos > 0) groundRight++; if(widthPos < 0) groundLeft--;
	}
}