using System.Collections;
using UnityEngine;
using TMPro;

public class KeyOperator : MonoBehaviour
{
	public bool areAssigning;
	[SerializeField] string assignAction;
	[SerializeField] string waitingMessage;
    TextMeshProUGUI assignDisplay;
	//Create it instance whenever needed
	static KeyOperator _i;  public static KeyOperator i 
	{
		get {if(_i == null) 
		{
			_i = GameObject.FindObjectOfType<KeyOperator>();
		}
		return _i;}
	}

	/// Added key here... ///
	public KeyCode OpenCraft, UseItem, FlipStructure, OpenDetails, SellStructure, MapGrid;
	
	public void StartAssign(KeyAssigner assigner)
	{
		//Get the action of the assigner given
		assignAction = assigner.action;
		//Get the display of the assigner given
		assignDisplay = assigner.keyDisplay;
		//Begining key assigning
		areAssigning = true; StartCoroutine("Assigning");
	}
	
	IEnumerator Assigning()
	{
		//If currently assigning
		while(areAssigning)
		{
			//Change the assign display to waititng message
			assignDisplay.text = waitingMessage;
            //Go though all the key to check if there is currently any input
            foreach(KeyCode pressedKey in System.Enum.GetValues(typeof(KeyCode)))
			{
				//If there is an input from any key and there is action to assign with
				if(Input.GetKey(pressedKey) && this.GetType().GetField(assignAction) != null)
				{
					//Change keycode variable in this script that has same name as action to key pressed
					this.GetType().GetField(assignAction).SetValue(this, pressedKey);
					//Change the assign display text to key pressed
					assignDisplay.text = pressedKey.ToString();
					//Stop assigning
					areAssigning = false;
				}
			}
			//If no longer assigning
			if(!areAssigning)
			{
				//Revert assign action
				assignAction = "None";
				//Clear assign display
				assignDisplay = null;
			}
			yield return null;
		}
	}
}
