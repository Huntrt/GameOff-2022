using System.Collections.Generic;
using UnityEngine;

namespace Crafts
{
public class Crafting : MonoBehaviour
{
	#region Set this class to singleton
	public static Crafting i {get{if(_i==null){_i = GameObject.FindObjectOfType<Crafting>();}return _i;}} static Crafting _i;
	#endregion
	
	//List of recipe of stash
	public List<Stash> recipe = new List<Stash>();
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
		for (int r = 0; r < recipe.Count; r++)
		{
			//Create an new panel for this recipe
			GameObject panel = Instantiate(recipePanel);
			//Set the panel to be this recipe info
			panel.GetComponent<RecipePanel>().SetRecipeInfo(recipe[r]);
			//Adding panel to it layout 
			panel.transform.SetParent(recipePanelLayout);
			//Reset the panel's scale
			panel.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void Craft(Stash crafted)
	{
		//Stop if inventory dont has enough material to craft given item
		if(!Inventory.i.materials.Consume(crafted.wood, crafted.steel, crafted.gunpowder)) return;
		//Add the crafted stash to inventory
		Inventory.Add(crafted);
	}
}
} //? End namespace