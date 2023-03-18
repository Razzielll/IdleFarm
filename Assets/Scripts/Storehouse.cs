using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storehouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        Farmer farmer = other.GetComponent<Farmer>();
        if (farmer.GetCropCount() >0)
        {
            
            farmer.MoveCropToBarn();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        Farmer farmer = other.GetComponent<Farmer>();
        if (farmer.GetCropCount() > 0)
        {

            farmer.MoveCropToBarn();
        }
    }
}
