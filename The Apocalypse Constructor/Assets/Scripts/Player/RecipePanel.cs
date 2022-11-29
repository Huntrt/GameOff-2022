using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Crafts
{
public class RecipePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] TextMeshProUGUI nameDisplay;
	[SerializeField] Image iconImage;
	[SerializeField] Button craftButton;
	Stash stash; Crafting craft; Inventory inv;
	Structure structure; Tower tower; Dynamo dynamo;
	bool pointerOver;

	void Start() {craft = Crafting.i; inv = Inventory.i;}

	public void SetupPanel(Stash recipe)
	{
		//Save the stash that given from recipe
		stash = recipe;
		//Display item name on text
		nameDisplay.text = stash.name;
		//Display the item icon on image
		iconImage.sprite = stash.icon;
		//Clear any existing crafting event
		craftButton.onClick.RemoveAllListeners();
		//Add crafting event to it button to craft this stash
		craftButton.onClick.AddListener(delegate {Crafting.i.Craft(stash);});
		//Begin setup this stash info
		SetupInfo();
	}

	void SetupInfo()
	{
		//Get the structure component from stash of this panel
		structure = stash.prefab.GetComponent<Structure>();
		//If structure are dynamo then save it
		if(structure.function == Structure.Function.dynamo) dynamo = stash.prefab.GetComponent<Dynamo>();
		//If structure are tower then save it
		if(structure.function == Structure.Function.tower) tower = stash.prefab.GetComponent<Tower>();
	}

	public void OnPointerEnter(PointerEventData eventData) 
	{
		//Toggle consume modifer of this panel's stash
		inv.materials.gameGui.ShowModifier(stash.ingredients, false);
		//This panel are now over pointer
		pointerOver = true;
		DisplayInfo();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Close the modifer
		inv.materials.gameGui.ShowModifier(null, false);
		//This panel no longer over pointer
		pointerOver = false;
	}

	void DisplayInfo()
	{
		DisplayGeneralInfo();
		DisplayStructureStats();
	}

	void DisplayGeneralInfo()
	{
		craft.infoGUI.iconImage.sprite = stash.icon;
		craft.infoGUI.nameText.text = stash.name;
		//Display materials
		craft.infoGUI.woodText.text = stash.ingredients.wood.ToString();
		craft.infoGUI.steelText.text = stash.ingredients.steel.ToString();
		craft.infoGUI.gunpowderText.text = stash.ingredients.gunpowder.ToString();
	}

	void DisplayStructureStats()
	{
		//@ Display each function stats of structure onto it own GUI
		if(structure.function == Structure.Function.filler)
		{
			StatsInfoHidden(0);
			craft.fillGUI.descriptionText.text = stash.description;
			craft.fillGUI.healthText.text = "Health:<b> " + structure.InitialMaxHP + "</b>";
		}
		else if(structure.function == Structure.Function.dynamo)
		{
			StatsInfoHidden(1);
			craft.dynamoGUI.descriptionText.text = stash.description;
			craft.dynamoGUI.healthText.text = "Health:<b> " + structure.InitialMaxHP + "</b>";
			craft.dynamoGUI.energyText.text = "Energy: <b>+" + dynamo.provide + "</b>";
		}
		else if(structure.function == Structure.Function.tower)
		{
			StatsInfoHidden(2);
			//Save the initial stats of tower
			Combats.Stats stats = tower.caster.InitialStats;
			craft.towerGUI.descriptionText.text = stash.description;
			craft.towerGUI.healthText.text = "Health: <b>" + structure.InitialMaxHP + "</b>";
			craft.towerGUI.damageText.text = "Damage: <b>" + stats.damage + "</b>";
			craft.towerGUI.rateText.text = "Rate: <b>" +  stats.rateTimer + "s</b>";
			craft.towerGUI.rangeText.text = "Range: <b>" + stats.range + "</b>";
			craft.towerGUI.depletedText.text = "Depleted: <b>" + tower.depleted + "</b>";
			craft.towerGUI.aimText.text = "Aim: <b>" + tower.GetComponent<Combat_Aiming>().mode + "</b>";
		}
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

	void OnDisable()
	{
		//If get disable while still point over this panel
		if(pointerOver)
		{
			//Close the materials modifier
			Inventory.i.materials.gameGui.ShowModifier(null);
			//No longer being pointer over
			pointerOver = false;
		}
	}
}
}