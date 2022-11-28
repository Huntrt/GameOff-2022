using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class House : Entity
{
	[SerializeField] Image healthBar;
	[SerializeField] TextMeshProUGUI healthCounter;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Extend the plot at house position then blocked it
		Map.ExtendPlot(transform.position, 3);
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
		//Game are over instead of destroy house
		print("Game Over");
	}
}
