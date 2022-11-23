using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] Vector2[] spawnPoint = new Vector2[2];
	[SerializeField] float spawnPointInward;
	[SerializeField] Ground ground;

	void OnEnable()
	{
		ground.onExpand += RefreshPoint;
		//@ Set the spawn point Y axis using ground hight move 1 block above
		spawnPoint[0].y = ground.initalSize.y + Map.i.spacing;
		spawnPoint[1].y = ground.initalSize.y + Map.i.spacing;
	}

	void RefreshPoint()
	{
		//@ Get spawn point X position by current ground left/righ decrease with inward amount
		spawnPoint[0].x = Map.Spaced(ground.groundLeft + spawnPointInward);
		spawnPoint[1].x = Map.Spaced(ground.groundRight - spawnPointInward);
	}

	void OnDrawGizmos() 
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(spawnPoint[0], 0.25f);
		Gizmos.DrawWireSphere(spawnPoint[1], 0.25f);
	}

	void OnDisable()
	{
		ground.onExpand -= RefreshPoint;
	}
}
