using UnityEngine;

public class SelfDeactive : MonoBehaviour
{
	public float delay;

	void OnEnable() {CancelInvoke("Activing"); Invoke("Activing", delay);}

    public void Deactiving() {gameObject.SetActive(false);}
}