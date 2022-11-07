using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Crafts
{
public class RecipePanel : MonoBehaviour
{
	public TextMeshProUGUI nameDisplay;
	public Button craftButton;

	public void SetRecipeInfo(Recipe recipe)
	{
		//Display recipe object name on text
		nameDisplay.text = recipe.name;
		//Update the object name to be obj of recipe it got
		gameObject.name = recipe.obj + " Recipe";
		//Add crafting event to it button to craft this recipe
		craftButton.onClick.AddListener(delegate {Crafting.i.Craft(recipe);});
	}
}
}