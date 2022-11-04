using UnityEngine;

public class Ground : MonoBehaviour
{
	public int groundWidth, fill;
	public Vector2 initalSize;
	public GameObject groundPrefab, fillerPrefab; Transform grouper;

	void Start()
	{
		InitializeGround();
	}

	void InitializeGround()
	{
		//Renew the group grouper
		if(grouper != null) {Destroy(grouper);} grouper = new GameObject().transform; grouper.name = "Grounds";
		//Create an new ground at center
		CreateGround(0);
		//Go through all the width need to create
		for (int x = 1; x <= initalSize.x; x++)
		{
			//Convert width with spacing
			float setWidth = x * Map.i.spacing;
			//Create ground for both side on this width
			CreateGround(setWidth); CreateGround(-setWidth);
			//Increase ground width
			groundWidth++;
		}
	}

	void CreateGround(float widthPos)
	{
		//Create the ground on the map at width given
		GameObject ground = Map.Placing(groundPrefab, new Vector2(widthPos, initalSize.y), "blocked");
		//Group then rename the ground created
		ground.transform.SetParent(grouper); ground.name = widthPos + " -  Ground";
		//Go through all the time need to fill this ground
		for (int y = 1; y <= fill; y++)
		{
			//Get position to place filler
			Vector2 fillPos = new Vector2(widthPos, initalSize.y - (y * Map.i.spacing));
			//Create the filler object the save it
			GameObject filler = Instantiate(fillerPrefab, fillPos, Quaternion.identity);
			//Group then rename the filler created
			filler.transform.SetParent(grouper); filler.name = y + " - Filler";
		}
	}
}