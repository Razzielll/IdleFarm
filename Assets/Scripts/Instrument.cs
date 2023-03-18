using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    public event Action<Collider> OnTrigger;
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
        if(other.GetComponent<Crop>() == null)
        {
            return;
        }
        other.GetComponent<Crop>().Harvest();
        OnTrigger?.Invoke(other);
    }
}
