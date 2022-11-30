using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class House : Entity
{
	[SerializeField] Image healthBar;
	[SerializeField] TextMeshProUGUI healthCounter;
	[SerializeField] AudioClip gameOverSound;
	[SerializeField] GameObject gameOverParticle;
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
		//Stop if the game already over
		if(gameOverPanel.activeInHierarchy) return;
		//Disable enemy spawner upon game over
		EnemiesManager.i.GetComponent<EnemiesSpawner>().enabled = false;
		//Play the game over sound upon die
		SessionOperator.i.audios.soundSource.PlayOneShot(gameOverSound);
		//Play game over particle
		gameOverParticle.SetActive(true);
		//Set the game over title text display
		gameOverTitleText.text = "You survive for <color=red><size=40>"+ DaysManager.i.passes+"</color></size> days";
		//Enable game over panel
		gameOverPanel.SetActive(true);
	}
}
