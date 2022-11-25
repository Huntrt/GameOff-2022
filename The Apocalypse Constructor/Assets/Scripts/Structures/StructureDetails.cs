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
			if(detailings.Length > 0) OpenDetails(); else CloseDetails();
		}
	}

	void OpenDetails()
	{
		CloseDetails();
		//Go through all the structure need to details
		for (int d = 0; d < detailings.Length; d++)
		{
			//Save this structure need to detalis
			Structure detail = detailings[d];
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

	void DetailStructure(Structure structure, StructureDetailPanel panel)
	{
		//@ Display structure raw name and description
		panel.nameText.text = structure.stashed.prefab.name; 
		panel.descText.text = structure.stashed.description;
		//@ Display structure health text and health bar
		panel.healthText.text = "Health: " + structure.health + "/" + structure.maxHealth;
		panel.healthBar.fillAmount = structure.health / structure.maxHealth;
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

	void CloseDetails()
	{
		//@ Close all details panels
		towerDetailPanel.grouper.SetActive(false);
		dynamoDetailPanel.grouper.SetActive(false);
		fillerDetailPanel.grouper.SetActive(false);
	}
}
