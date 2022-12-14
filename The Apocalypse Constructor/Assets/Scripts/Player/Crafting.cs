using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

namespace Crafts
{
public class Crafting : MonoBehaviour
{
	#region Set this class to singleton
	public static Crafting i {get{if(_i==null){_i = GameObject.FindObjectOfType<Crafting>();}return _i;}} static Crafting _i;
	#endregion
	
	[SerializeField] List<Stash> structureRecipes = new List<Stash>();
	[SerializeField] List<Stash> towerRecipes = new List<Stash>();
	[Header("GUI")]
	public GameObject craftingGUI;
	[SerializeField] GameObject recipePanel;
	[SerializeField] RectTransform recipePanelLayout;
	[SerializeField] List<RecipePanel> panels = new List<RecipePanel>();
	public InfoGUI infoGUI;
	public FillGUI fillGUI;
	public DynamoGUI dynamoGUI;
	public TowerGUI towerGUI;
	
	#region GUI Classes
	[Serializable] public class InfoGUI
	{
		public Image iconImage;
		public TextMeshProUGUI nameText;
		public TextMeshProUGUI woodText, steelText, gunpowderText;
	}	
	[Serializable] public class FillGUI
	{
		public GameObject group;
		public TextMeshProUGUI descriptionText;
		public TextMeshProUGUI healthText;
	}	
	[Serializable] public class DynamoGUI : FillGUI 
	{
		public TextMeshProUGUI energyText;
	}	
	[Serializable] public class TowerGUI : FillGUI
	{
		public TextMeshProUGUI damageText, rateText, rangeText, depletedText, aimText;
	}
	#endregion

	void Start()
	{
		AddPanels();
		RefreshRecipe(true);
	}
	
	void Update()
	{
		//When press open craft key toggle the craft gui
		if(Input.GetKeyDown(KeyOperator.i.OpenCraft)) ToggleGUI();
	}

	public void ToggleGUI()
	{
		//Switch between craft gui active
		craftingGUI.SetActive(!craftingGUI.activeInHierarchy);
	}

	void AddPanels()
	{
		//The amount of recipe panel need for both structure and tower
		int structP = structureRecipes.Count; int towerP = towerRecipes.Count;
		//Add panel until enough for both structure and panels recipe
		while (panels.Count <= structP || panels.Count <= towerP)
		{
			//Create an new panel for this recipe
			GameObject panel = Instantiate(recipePanel);
			//Save the panel component just got created
			panels.Add(panel.GetComponent<RecipePanel>());
			//Deactive the panel
			panel.SetActive(false);
			//Adding panel to it layout 
			panel.transform.SetParent(recipePanelLayout);
			//Reset the panel's scale
			panel.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void RefreshRecipe(bool forStructure)
	{
		//Go through all the ppanel has created
		for (int p = 0; p < panels.Count; p++)
		{
			//Deactive this panel first
			panels[p].gameObject.SetActive(false);
			//If needed to refresh for structure and havent refresh all of them
			if(forStructure && structureRecipes.Count > p) 
			{
				//Setup this panel for this structure recipe 
				panels[p].SetupPanel(structureRecipes[p]); panels[p].gameObject.SetActive(true);
			}
			//If needed to refresh for tower and havent refresh all of them
			else if(!forStructure && towerRecipes.Count > p)
			{
				//Setup this panel for this tower recipe 
				panels[p].SetupPanel(towerRecipes[p]); panels[p].gameObject.SetActive(true);
			}
		}
	}

	public void Craft(Stash crafted)
	{
		//Stop if inventory dont has enough material to craft given item
		if(!Inventory.i.materials.Consume(crafted.ingredients.wood, crafted.ingredients.steel, crafted.ingredients.gunpowder,0)) return;
		//Add the crafted stash to inventory
		if(!Inventory.Add(crafted)) Debug.LogWarning("Inventory full");
	}
}
} //? End namespace