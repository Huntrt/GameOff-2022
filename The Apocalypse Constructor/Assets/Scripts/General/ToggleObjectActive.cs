using UnityEngine;

public class ToggleObjectActive : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToToggle;

	void Toggling()
	{
		//Go through all object to toggle them
		for (int o = 0; o < objectsToToggle.Length; o++)
		{
			//Switch between their active state
			objectsToToggle[o].SetActive(!objectsToToggle[o].activeInHierarchy);
		}
	}
}
