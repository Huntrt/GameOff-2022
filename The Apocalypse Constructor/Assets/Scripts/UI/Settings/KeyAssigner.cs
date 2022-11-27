using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class KeyAssigner : MonoBehaviour
{
    public string action;
	public TextMeshProUGUI keyDisplay;
	[SerializeField] Button button;
	[Tooltip("Will auto: \n- Get this object name as action\n- Get button component of object it on \n- Get the 0 child text as keycode display")]
	[SerializeField] bool autoSetup; bool hasSetup;
	[SerializeField] int keyDisplayChildIndex;
	KeyOperator manager;

	void OnValidate() 
	{
		//If auto setup are enable while haven't setup
		if(autoSetup && !hasSetup)
		{
			//The action are object name
			action = gameObject.name;
			//Get the button component of this object
			button = GetComponent<Button>();
			//Get the key display from the child of index given on this object
			keyDisplay = transform.GetChild(keyDisplayChildIndex).GetComponent<TextMeshProUGUI>();
			//Has complete setup
			hasSetup = true;
		}
		//Clear all the button, display and action if disable auto setup while has setup
		if(!autoSetup && hasSetup) {action = ""; button = null; keyDisplay = null; hasSetup = false;}
	}

	void Start()
	{
		//Get the key manager
		manager = KeyOperator.i;
		//Upon clicking button it will send this assigner to manager
		button.onClick.AddListener(delegate {manager.StartAssign(this);});
		///If there NO keycode variable in manager that has the same name as this assigner action
		if(manager.GetType().GetField(action) == null)
		{
			//Print an error
			Debug.LogError("There are no keycode variable named '" + action + " in KeyManager.cs");
		}
		///If there IS keycode variable in manager that has the same name as this assigner action
		else
		{
			//Display the keycode variable in manager that has the same name as action
			keyDisplay.text = manager.GetType().GetField(action).GetValue(manager).ToString();
		}
	}
}
