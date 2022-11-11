using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Crafts
{
public class RecipePanel : MonoBehaviour
{
	public TextMeshProUGUI nameDisplay;
	public Button craftButton;

	public void SetRecipeInfo(Stash item)
	{
		//Display item name on text
		nameDisplay.text = item.name;
		//Update the panel name to to be the item's
		gameObject.name = item.name + " Recipe";
		//Add crafting event to it button to craft this recipe
		craftButton.onClick.AddListener(delegate {Crafting.i.Craft(item);});
	}
}
}