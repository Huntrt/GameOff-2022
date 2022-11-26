using UnityEngine.UI;
using UnityEngine;

public class ToggleBetweenColor : MonoBehaviour
{
    
	[SerializeField] bool toggled;
    [SerializeField] Graphic[] graphics;
	[SerializeField] Color baseColor, toggleColor;

	public void Toggling()
	{
		toggled = !toggled;
		for (int g = 0; g < graphics.Length; g++)
		{
			if(toggled) graphics[g].color = toggleColor; else graphics[g].color = baseColor;
		}
	}
}
