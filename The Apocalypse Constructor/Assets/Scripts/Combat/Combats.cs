public static class Combats
{
    [System.Serializable] public class Stats 
	{
		public float DPS;
		public float damage, rate, range;
		public float rateTimer {get {return (float)System.Math.Round(1/rate,1);}}
		public static float Scale(float stat, float scaling) {return (scaling /100) * stat;}
	}
}
