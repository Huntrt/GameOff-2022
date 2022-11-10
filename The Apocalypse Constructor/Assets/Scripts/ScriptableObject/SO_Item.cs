using UnityEngine;

public class SO_Item : ScriptableObject
{
	[Header("Item")]
	public GameObject prefab;
	public string description;
	[Tooltip("0 = nothing \n 1 = tower \n 2 = platform \n 3 = blocked ")]
	public int occupation;
	public Sprite icon;
	public Stack stack; 
	[System.Serializable] public class Stack {public int max, cur;}
	public Recipe recipe;
	[System.Serializable] public class Recipe {public int wood, steel, gunpowder, rarity;}
}