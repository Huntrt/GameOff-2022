using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HoverHealthOverlay : MonoBehaviour
{
	[SerializeField] PlayerCursor pCursor;
	[SerializeField] HealthOverlay[] hoverHealthOverlay;
	bool overlayAll;

	void OnEnable() {pCursor.onHover += ShowHoverOverlay;}
	void OnDisable() {pCursor.onHover -= ShowHoverOverlay;}

	void ShowHoverOverlay()
	{
		//Deactive all hover overlay
		for (int h = 0; h < hoverHealthOverlay.Length; h++) hoverHealthOverlay[h].gameObject.SetActive(false);
		//If hover over an structure
		if(pCursor.structureHovered.Length > 0)
		{
			//Go through all structure being hover
			for (int h = 0; h < pCursor.structureHovered.Length; h++)
			{
				//Set this hover overlay as this hover structure
				hoverHealthOverlay[h].entity = pCursor.structureHovered[h];
				//Active this hover overlay
				hoverHealthOverlay[h].gameObject.SetActive(true);
			}
		}
	}
}
