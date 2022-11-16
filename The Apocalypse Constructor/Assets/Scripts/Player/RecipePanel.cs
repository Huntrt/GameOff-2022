using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Crafts
{
public class RecipePanel : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI nameDisplay;
	[SerializeField] Image iconImage;
	[SerializeField] Button craftButton;
	Stash recipe; Crafting craft;
	Structure structure; Tower tower; Dynamo dynamo;

	void Start() {craft = Crafting.i;}

	public void SetupPanel(Stash recipe)
	{
		//Save the recipe that given from setup
		this.recipe = recipe;
		//Display item name on text
		nameDisplay.text = recipe.name;
		//Display the item icon on image
		iconImage.sprite = recipe.icon;
		//Add crafting event to it button to craft this recipe
		craftButton.onClick.AddListener(delegate {Crafting.i.Craft(recipe);});
		//Begin setup this recipe info
		SetupInfo();
	}

	void SetupInfo()
	{
		//Get the structure component from recipe prefab
		structure = recipe.prefab.GetComponent<Structure>();
		//If structure are dynamo then save it
		if(structure.function == Structure.Function.dynamo) dynamo = recipe.prefab.GetComponent<Dynamo>();
		//If structure are tower then save it
		if(structure.function == Structure.Function.tower) tower = recipe.prefab.GetComponent<Tower>();
	}

	public void DisplayInfo()
	{
		DisplayGeneralInfo();
		//@ Display each function stats of structure onto it own GUI
		if(structure.function == Structure.Function.filler)
		{
			StatsInfoHidden(0);
			craft.fillGUI.descriptionText.text = recipe.description;
			craft.fillGUI.healthText.text = "Health:<b> " + structure.maxHealth + "</b>";
		}
		else if(structure.function == Structure.Function.dynamo)
		{
			StatsInfoHidden(1);
			craft.dynamoGUI.descriptionText.text = recipe.description;
			craft.dynamoGUI.healthText.text = "Health:<b> " + structure.maxHealth + "</b>";
			craft.dynamoGUI.energyText.text = "Energy: <b>+" + dynamo.provide + "</b>";
		}
		else if(structure.function == Structure.Function.tower)
		{
			StatsInfoHidden(2);
			craft.towerGUI.descriptionText.text = recipe.description;
			craft.towerGUI.healthText.text = "Health: <b>" + structure.maxHealth + "</b>";
			craft.towerGUI.damageText.text = "Damage: <b>" + tower.stats.damage + "</b>";
			craft.towerGUI.speedText.text = "Rate: <b>" +  tower.stats.rateTimer + "s</b>";
			craft.towerGUI.rangeText.text = "Range: <b>" + tower.stats.range + "</b>";
			craft.towerGUI.depletedText.text = "Depleted: <b>" + tower.depleted + "</b>";
			craft.towerGUI.aimText.text = "Aim: <b>" + tower.GetComponent<Aiming>().mode + "</b>";
		}
	}

	void DisplayGeneralInfo()
	{
		craft.infoGUI.iconImage.sprite = recipe.icon;
		craft.infoGUI.nameText.text = recipe.name;
		//Display materials
		craft.infoGUI.woodText.text = recipe.wood.ToString();
		craft.infoGUI.steelText.text = recipe.steel.ToString();
		craft.infoGUI.gunpowderText.text = recipe.gunpowder.ToString();
	}

	void StatsInfoHidden(int function)
	{
		//@ Deactive all the GUI
		craft.fillGUI.group.SetActive(false);
		craft.dynamoGUI.group.SetActive(false);
		craft.towerGUI.group.SetActive(false);
		//@ Show each type of GUI depend on given function
		switch(function)
		{
			case 0: craft.fillGUI.group.SetActive(true); break;
			case 1: craft.dynamoGUI.group.SetActive(true); break;
			case 2: craft.towerGUI.group.SetActive(true); break;
		}
	}
}
}