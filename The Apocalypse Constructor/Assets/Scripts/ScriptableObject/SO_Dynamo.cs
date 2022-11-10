using UnityEngine;

[CreateAssetMenu(fileName = "Dynamo Name", menuName = "Scriptable Object/Dynamo", order = 1)]
public class SO_Dynamo : SO_Building
{
	[Header("Dynamo")]
	public int energyProvide;
}