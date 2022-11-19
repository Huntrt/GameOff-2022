using System.Collections.Generic;
using UnityEngine;

public class HealthFlashing : MonoBehaviour
{
	[SerializeField] float flashDuration;
	[SerializeField] Material flashMaterial;
	[SerializeField] Color hurtColor, healColor;
	[SerializeField] bool setUp;
	public List<RenderData> renderDatas;
	[SerializeField] Entity entity;
	[System.Serializable] public class RenderData
	{
		public SpriteRenderer render;
   		public Material defaultMat;
		public Color defaultColor;

		public RenderData(SpriteRenderer render)
		{
			this.render = render;
			this.defaultMat = render.sharedMaterial;
			this.defaultColor = render.color;
		}

		public void Set(Color color, Material material)
		{
			render.color = color;
			render.material = material;
		}

		public void SetDefault()
		{
			render.color = defaultColor;
			render.material = defaultMat;
		}
	}

	void OnValidate() 
	{
		//If request to setup
		if(setUp) 
		{
			//Clear and get all the child render
			renderDatas.Clear(); GettingRender(transform);
			//Get the entity of thiis object
			entity = GetComponent<Entity>();
		}
	}

	void GettingRender(Transform parent)
	{
		//Get sprite render of given parent
		SpriteRenderer render = parent.GetComponent<SpriteRenderer>();
		//Add given parent with it render if it does have an render
		if(render != null) renderDatas.Add(new RenderData(render));
		//Go through all of the given parent child to get their render
		for (int c = 0; c < parent.childCount; c++) GettingRender(parent.GetChild(c));
	}

	void OnEnable()
	{
		entity.onHurt += HurtFlashing;
		entity.onHeal += HealFlashing;
		entity.onDeath += DefaultFlashing;
	}

	void OnDisable()
	{
		entity.onHurt -= HurtFlashing;
		entity.onHeal -= HealFlashing;
		entity.onDeath -= DefaultFlashing;
	}

	void HurtFlashing(float amount) => Flashing(hurtColor);
	void HealFlashing(float amount) => Flashing(healColor);

	void Flashing(Color color)
	{
		//Cancel end flash currently running
		CancelInvoke("EndFlash");
		//Set every child render color to be the given color and set their material to flas 
		for (int c = 0; c < renderDatas.Count; c++) renderDatas[c].Set(color, flashMaterial);
		//Begin cooldown to end flash
		Invoke("EndFlash", flashDuration);
	}

	void EndFlash() => DefaultFlashing();

	void DefaultFlashing(float amount = 0)
	{
		//Set ervey child render to default
		for (int c = 0; c < renderDatas.Count; c++) renderDatas[c].SetDefault();
	}
}