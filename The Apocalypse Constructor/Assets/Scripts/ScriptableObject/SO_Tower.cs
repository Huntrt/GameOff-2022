using UnityEngine;

[CreateAssetMenu(fileName = "Tower Name", menuName = "Scriptable Object/Tower", order = 0)]
public class SO_Tower : SO_Building
{
	[Header("Tower")]
	public float damage;
	public float speed, range, crit;
}
