using System.Collections.Generic;
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
	[SerializeField] GameObject craftingGUI;
	[SerializeField] GameObject recipePanel;
	[SerializeField] RectTransform recipePanelLayout;

	void Start()
	{
		//Getting data from the item json
		string data = System.IO.File.ReadAllText(@"Assets\Scripts\Player\Items.json");
		//Import all the json data into recipe
		cookbook = JsonUtility.FromJson<Cookbook>(data);
		AddRecipe();
	}
	
	void Update()
	{
		//todo: keybinds CRAFTING GUI
		if(Input.GetKeyDown(KeyCode.C))
		{
			//Toggle craft gui active
			craftingGUI.SetActive(!craftingGUI.activeInHierarchy);
		}
	}

	void AddRecipe()
	{
		//Go through all the recipes
		for (int r = 0; r < cookbook.recipes.Count; r++)
		{
			//Create an new panel for this recipe
			GameObject panel = Instantiate(recipePanel);
			//Set the panel to be this recipe info
			panel.GetComponent<RecipePanel>().SetRecipeInfo(cookbook.recipes[r]);
			//Adding panel to it layout 
			panel.transform.SetParent(recipePanelLayout);
			//Reset the panel's scale
			panel.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void Craft(Recipe recipe)
	{
		//Create an new stash that gonna craft from recipe
		Stash stashed = new Stash(recipe.name, recipe.description, recipe.category,recipe.obj, recipe.maxStack);
		//Add the crafted recipe to inventory
		Inventory.Add(stashed);
	}
}

[System.Serializable] public class Recipe : Stash
{
	public int wood, steel, gunpowder, rarity;

	public Recipe(string name, string desc,string cate, GameObject obj, int maxStack) : base(name,desc,cate,obj,maxStack)
	{
		this.name = name;
		this.description = desc;
		this.category = cate;
		this.obj = obj;
		this.maxStack = maxStack;
	}
}
} //? End namespace