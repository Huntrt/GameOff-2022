using UnityEngine;
using Crafts;

[System.Serializable] public class Stash 
{
	public string name, description, category;
    public GameObject obj;
	public int maxStack, stack;

	public Stash(string name, string description, string category, GameObject obj, int maxStack)
	{
		this.name = name;
		this.description = description;
		this.category = category;
		this.obj = obj;
		this.maxStack = maxStack;
	}
}
