using System.Collections.Generic;
using UnityEngine;

public class MapGridVisual : MonoBehaviour
{
	[SerializeField] bool show;
	[SerializeField] GameObject gridVisualPrefab;
	[SerializeField] Transform gridVisualGrouper;
	[SerializeField] Color emptyColor, towerColor, platformColor, blockColor;
	Map map;

	void OnEnable() {map = Map.i; map.onRextend += RefreshGridVisual;}

	void OnDisable() {map.onRextend -= RefreshGridVisual;}

	void RefreshGridVisual()
	{
		//Stop if dont need to show the visual
		if(!show) return;
		//Go through all the plot in map
		for (int p = 0; p < map.plots.Count; p++)
		{
			//Save this map plot
			Plot plot = map.plots[p];
			//If this plot haven't got visual
			if(plot.visual == null)
			{
				//Create an new grid visual prefab at this plot coordinate
				GameObject newVis = Instantiate(gridVisualPrefab, plot.coordinate, Quaternion.identity);
				//Group the newly created visual
				newVis.transform.SetParent(gridVisualGrouper);
				//Send sprite renderer of new visual to be plot's visual
				plot.visual = newVis.GetComponent<SpriteRenderer>();
			}
			//Get this plot's occupation
			switch(plot.occupation)
			{
				//@ Set plot visual color base on it occupation
				case 0: plot.visual.color = emptyColor; break;
				case 1: plot.visual.color = towerColor; break;
				case 2: plot.visual.color = platformColor; break;
				case 3: plot.visual.color = blockColor; break;
			}
		}
	}

	public void ToggleVisualGrid()
	{
		//Toggle between showing or not
		show = !show;
		//Refresh the grid visual
		RefreshGridVisual();
		//Enable the visual grouper base on showing
		gridVisualGrouper.gameObject.SetActive(show);
	}
}