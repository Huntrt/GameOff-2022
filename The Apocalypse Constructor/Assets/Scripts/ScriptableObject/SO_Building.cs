using UnityEngine;

[CreateAssetMenu(fileName = "Building Name", menuName = "Scriptable Object/Building", order = 0)]
public class SO_Building : SO_Item
{
	[Header("Building")]
	public float maxHealth;
	public int consumption;
}