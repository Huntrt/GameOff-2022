using UnityEngine;

public static class GetCloset
{
    public static GameObject Ray(Vector2 point, RaycastHit2D[] detects)
	{
		//The most closet distance
		float most = Mathf.Infinity;
		//The potential closet enemy
		GameObject potential = null;
		//Go through all the enemy got detect
		for (int d = 0; d < detects.Length; d++)
		{
			//Get the distance by sqr magnitude the direction from point to this detect object
			float dist = ((Vector2)detects[d].collider.transform.position - point).sqrMagnitude;
			//If this ditance are closer than the most closest
			if(dist < most)
			{
				//This distance are now the most closest
				most = dist;
				//Potential enemy are now this enemy
				potential = detects[d].collider.gameObject;
			}
		}
		//Return the final potential enemy
		return potential;
	}
}
