using System;

public static class Combats
{
    [Serializable] public class Stats 
	{
		public float DPS;
		public float damage, rate, range;
		public float rateTimer {get {return (float)System.Math.Round(1/rate,1);}}
		public void RefreshDPS() {DPS = (float)Math.Round(damage / rateTimer,2);}
		public static float Scale(float stat, float scaling) {return (scaling / 100) * stat;}
	}

	public static Stats GrowingStats(int level, Stats init, Stats growth)
	{
		//Create an new empty stats to grow
		Stats grow = new Stats();
		//? Formula = get growth percent of initial multiply by level (take initial into account)
		grow.damage = init.damage + (((growth.damage / 100) * init.damage) * level);
		grow.rate = init.rate + (((growth.rate / 100) * init.rate) * level);
		grow.range = init.range + (((growth.range / 100) * init.range) * level);
		return grow;
	}
}