using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class House : Entity
{
	[SerializeField] Image healthBar;
	[SerializeField] TextMeshProUGUI healthCounter;
	[SerializeField] GameObject gameOverPanel;
	[SerializeField] TextMeshProUGUI gameOverTitleText;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Extend the plot at house position then blocked it
		Map.ExtendPlot(transform.position, 3);
		//Grow the house health
		GrowingHealth(0);
		//Update the health bar when place
		UpdateHealthGUI();
	}

	public override void Hurt(float amount)
	{
		base.Hurt(amount);
		UpdateHealthGUI();
	}

	public override void Heal(float amount)
	{
		base.Heal(amount);
		UpdateHealthGUI();
	}

	void UpdateHealthGUI() 
	{
		healthBar.fillAmount = Health/finalMaxHP;
		healthCounter.text = Health + "/" + finalMaxHP;
	}

	public override void Die()
	{
		//Set the game over title text display
		gameOverTitleText.text = "You survive for <color=red><size=40>"+ DaysManager.i.passes+"</color></size> days";
		//Enable game over panel
		gameOverPanel.SetActive(true);
	}
}
