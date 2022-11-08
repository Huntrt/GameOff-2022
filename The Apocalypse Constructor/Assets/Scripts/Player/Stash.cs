using UnityEngine;
using Crafts;

[System.Serializable] public class Stash 
{
	public string name, description;
    public GameObject obj;
	public int maxStack, stack;

	public Stash(string name, string description, GameObject obj, int maxStack)
	{
		this.name = name;
		this.description = description;
		this.obj = obj;
		this.maxStack = maxStack;
	}
}
