using System.Collections.Generic;
using UnityEngine;

namespace Crafts
{
public class Crafting : MonoBehaviour
{
	#region Set this class to singleton
	public static Crafting i {get{if(_i==null){_i = GameObject.FindObjectOfType<Crafting>();}return _i;}} static Crafting _i;
	#endregion
	
	public List<SO_Item> items = new List<SO_Item>();
	[Header("User Interface")]
	[SerializeField] GameObject craftingGUI;
	[SerializeField] GameObject recipePanel;
	[SerializeField] RectTransform recipePanelLayout;

	void Start()
	{
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
		for (int r = 0; r < items.Count; r++)
		{
			//Create an new panel for this recipe
			GameObject panel = Instantiate(recipePanel);
			//Set the panel to be this recipe info
			panel.GetComponent<RecipePanel>().SetRecipeInfo(items[r]);
			//Adding panel to it layout 
			panel.transform.SetParent(recipePanelLayout);
			//Reset the panel's scale
			panel.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void Craft(SO_Item crafted)
	{
		//Stop if inventory dont has enough material to craft given item
		if(!Inventory.i.materials.Consume(crafted.recipe.wood, crafted.recipe.steel, crafted.recipe.gunpowder)) return;
		//@ Replicate the item that got from given recipe to stash it
		SO_Item stashing = ScriptableObject.CreateInstance("SO_Item") as SO_Item;
		stashing.name = crafted.name;
		stashing.prefab = crafted.prefab;
		stashing.description = crafted.description;
		stashing.occupation = crafted.occupation;
		stashing.icon = crafted.icon;
		stashing.stack = new SO_Item.Stack();
		stashing.stack.max = crafted.stack.max;
		//Add the crafted stash to inventory
		Inventory.Add(stashing);
	}
}
} //? End namespace