using UnityEngine;

public class SelfDeactive : MonoBehaviour
{
	public float delay;

	void OnEnable() {CancelInvoke("Activing"); Invoke("Activing", delay);}

    public void Activing() {gameObject.SetActive(false);}
}