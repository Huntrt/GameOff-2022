using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StructureDetails : MonoBehaviour
{
	[SerializeField] PlayerCursor pCursor;
    public Structure[] detailings;
	#region  GUI
	[Header("GUI")]
	[SerializeField] TowerDetailPanel towerDetailPanel;
	[SerializeField] DynamoDetailPanel dynamoDetailPanel;
	[SerializeField] StructureDetailPanel fillerDetailPanel;

	[System.Serializable] class TowerDetailPanel : StructureDetailPanel
	{
		public TextMeshProUGUI damageText, rateText, rangeText, depletedText, aimText;
	}

	[System.Serializable] class DynamoDetailPanel : StructureDetailPanel
	{
		public TextMeshProUGUI energyText;
	}

	[System.Serializable] class StructureDetailPanel
	{
		public Structure structure;
		public GameObject grouper;
		public TextMeshProUGUI nameText, descText, healthText; 
		public Image healthBar;
	}
	#endregion

	void Update()
	{
		//todo: Add keybinds to details structure hover
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			//Get the structure current hovering
			detailings = pCursor.structureHovered;
			//Open and close details base on does hover anything
			if(detailings.Length > 0) OpenAllDetails(); else CloseAllDetails();
		}
		//If detailnig any structure
		if(detailings.Length > 0)
		{
			//@ Update structure health onto details panel or close the panel if structure no longger exist
			if(towerDetailPanel.structure != null) DetailHealth(towerDetailPanel); else CloseDetails(towerDetailPanel);
			if(dynamoDetailPanel.structure != null) DetailHealth(dynamoDetailPanel); else CloseDetails(dynamoDetailPanel);
			if(fillerDetailPanel.structure != null) DetailHealth(fillerDetailPanel); else CloseDetails(fillerDetailPanel);
		}
	}

	void OpenAllDetails()
	{
		CloseAllDetails();
		//Go through all the structure need to details
		for (int d = 0; d < detailings.Length; d++)
		{
			//Save this structure need to detalis
			Structure detail = detailings[d];
			//Skip if attempt to details nothing
			if(detail == null) continue;
			//@ Detailing panel base on what function this structure does
			if(detail.function == Structure.Function.tower)
			{
				DetailTower(detail.GetComponent<Tower>(), towerDetailPanel);
				towerDetailPanel.grouper.SetActive(true);
			}
			else if(detail.function == Structure.Function.dynamo)
			{
				DetailDynamo(detail.GetComponent<Dynamo>(), dynamoDetailPanel);
				dynamoDetailPanel.grouper.SetActive(true);
			}
			else if(detail.function == Structure.Function.filler)
			{
				DetailStructure(detail, fillerDetailPanel);
				fillerDetailPanel.grouper.SetActive(true);
			}
		}
	}

	void DetailHealth(StructureDetailPanel panel)
	{
		//@ Display the given details panel's structure health
		panel.healthText.text = "Health: " + panel.structure.health + "/" + panel.structure.maxHealth;
		panel.healthBar.fillAmount = panel.structure.health / panel.structure.maxHealth;
	}

	void DetailStructure(Structure structure, StructureDetailPanel panel)
	{
		//Save the structure currently details
		panel.structure = structure;
		//@ Display structure stash name and description
		panel.nameText.text = structure.stashed.name; 
		panel.descText.text = structure.stashed.description;
		DetailHealth(panel);
	}

	void DetailTower(Tower tower, TowerDetailPanel panel)
	{
		//Detail anything about tower's structure first
		DetailStructure(tower, panel);
		//Get the stats from tower's caster
		Combats.Stats stats = tower.GetComponent<Combat_Caster>().stats;
		//@ Display the tower stats
		panel.damageText.text = "Damage: <b>" + stats.damage + "</b>";
		panel.rateText.text = "Rate: <b>" + stats.rateTimer + "s</b>";
		panel.rangeText.text = "Range: <b>" + stats.range + "</b>";
		panel.depletedText.text = "Depleted: <b>" + tower.depleted + "</b>";
		panel.aimText.text = "Aim: <b>" + tower.GetComponent<Combat_Aiming>().mode + "</b>";
	}

	void DetailDynamo(Dynamo dynamo, DynamoDetailPanel panel)
	{
		//Detail anything about dynamo's structure first
		DetailStructure(dynamo, panel);
		//Display the energy dynamo provide
		panel.energyText.text = "Energy: <b>+" + dynamo.provide + "</b>";
	}

	void CloseAllDetails()
	{
		CloseDetails(towerDetailPanel);
		CloseDetails(dynamoDetailPanel);
		CloseDetails(fillerDetailPanel);
	}

	void CloseDetails(StructureDetailPanel panel)
	{
		//Given panel structure are now null
		panel.structure = null;
		//Deactive given panel grouper
		panel.grouper.SetActive(false);
	}
}
