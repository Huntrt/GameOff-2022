using UnityEngine;

public class SelfDeactive : MonoBehaviour
{
	public float delay;

	void OnEnable() {CancelInvoke("Deactiving"); Invoke("Deactiving", delay);}

    public void Deactiving() {gameObject.SetActive(false);}
}