using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantThorn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Invoke("HidePlantThorn", 1f);
        }
    }

    private void HidePlantThorn()
    {
        this.gameObject.SetActive(false);
    }
}
