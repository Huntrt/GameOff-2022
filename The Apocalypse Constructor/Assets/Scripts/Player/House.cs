using UnityEngine.UI;
using UnityEngine;

public class House : Entity
{
	public Image healthBar;

	protected override void OnEnable()
	{
		base.OnEnable();
		//Placing itself onto the map at it position and then block the occupation plot
		Map.ExtendPlot(null, transform.position, 3);
		//Update the health bar when place
		UpdateHealthBar();
	}

	public override void Hurt(float amount)
	{
		base.Hurt(amount);
		UpdateHealthBar();
	}

	public override void Heal(float amount)
	{
		base.Heal(amount);
		UpdateHealthBar();
	}

	void UpdateHealthBar() {healthBar.fillAmount = health/maxHealth;}

	public override void Die()
	{
		//Game are over instead of destroy house
		print("Game Over");
	}
}
