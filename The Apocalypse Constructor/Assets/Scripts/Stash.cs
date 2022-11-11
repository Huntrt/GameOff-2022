using UnityEngine;

[CreateAssetMenu(fileName = "Stash Name", menuName = "Stash", order = 0)]
public class Stash : ScriptableObject
{
	[Header("Item")]
	public GameObject prefab;
	public Sprite icon;
	[TextArea(5,50)] public string description;
	public enum Occupation {nothing, tower, platform, fill};
	public Occupation occupation;
	public int maxStack;
	[Header("Recipe")]
	public int wood;
	public int steel;
	public int gunpowder;
	public int rarity;
}