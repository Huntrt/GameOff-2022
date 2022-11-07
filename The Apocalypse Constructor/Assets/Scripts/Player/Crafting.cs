using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Crafts
{
public class Crafting : MonoBehaviour
{
	#region Set this class to singleton
	public static Crafting i {get{if(_i==null){_i = GameObject.FindObjectOfType<Crafting>();}return _i;}} static Crafting _i;
	#endregion
	
	[System.Serializable] public class Cookbook 
	{
		public List<Recipe> recipes = new List<Recipe>();
		public List<Recipe> unlocked = new List<Recipe>();
	}
	public Cookbook cookbook;
	string jsonData;
	[Header("User Interface")]
	[SerializeField] GameObject panelPrefab;
	[SerializeField] RectTransform panelLayout;
	

	void Start()
	{
		//Getting data from the item json
		string data = System.IO.File.ReadAllText(@"Assets\Scripts\Player\Items.json");
		//Import all the json data into recipe
		cookbook = JsonUtility.FromJson<Cookbook>(data);
		AddPanel();
	}

	void AddPanel()
	{
		//Go through all the recipes
		for (int r = 0; r < cookbook.recipes.Count; r++)
		{
			//Create an new panel for this recipe
			GameObject panel = Instantiate(panelPrefab);
			//Set the panel to be this recipe info
			panel.GetComponent<RecipePanel>().SetRecipeInfo(cookbook.recipes[r]);
			//Adding panel to it layout 
			panel.transform.SetParent(panelLayout);
			//Reset the panel's scale
			panel.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void Craft(Recipe recipe)
	{
		print("Crafted " + recipe.obj);
	}
}

[System.Serializable] public class Recipe
{
	public string obj, desc;
	public enum Category {structure, tower, upgrade}; public Category category;
	public int wood, steel, gunpowder, rarity;

	public Recipe(int wood, int steel, int gunpowder, int rarity, string obj, string desc, string category)
	{
		this.wood = wood;
		this.steel = steel;
		this.gunpowder = gunpowder;
		this.rarity = rarity;
		this.obj = obj;
		this.desc = desc;
		//Convert string to category enum
		this.category = (Category)System.Enum.Parse(typeof(Category), category);
	}
}
} //? End namespace