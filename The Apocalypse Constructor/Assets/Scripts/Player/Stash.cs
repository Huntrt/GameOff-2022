using UnityEngine;

[CreateAssetMenu(fileName = "Stash Name", menuName = "Scriptable Object/Stash", order = 0)]
public class Stash : ScriptableObject
{
	[Header("Item")]
	public GameObject prefab;
	public Sprite icon;
	[TextArea(5,50)] public string description;
	public enum Occupation {empty, tower, platform, fill};
	public Occupation occupation;
	public int maxStack;
	[System.Serializable] public class Ingredients {public int wood, steel, gunpowder;}
	public Ingredients ingredients;
	[Tooltip("How much percent of ingredients will be return when manually destroy")]
	[SerializeField] Ingredients leftover;
	[Tooltip("How much ingredients will be needed to upgrade")]
	public Ingredients upgrading;

	public Ingredients Leftovering()
	{
		//Get the value from leftover percent with ingredients 
		Ingredients left = new Ingredients();
		left.wood = (int)(((float)leftover.wood / 100) * ingredients.wood);
		left.steel = (int)(((float)leftover.steel / 100) * ingredients.steel);
		left.gunpowder = (int)(((float)leftover.gunpowder / 100) * ingredients.gunpowder);
		return left;
	}
}