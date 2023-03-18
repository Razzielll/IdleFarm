using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackOfCrop : MonoBehaviour
{
    [SerializeField] float cropSpeed = 1f;
    Transform target = null;
    [SerializeField] float cropCollectionRadius=0.5f;
    Farmer farmer = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }

        MoveCrop();
        if(Vector3.Distance(target.position, transform.position) < cropCollectionRadius)
        {
            farmer.AddCrop();
            target = null;
            Destroy(gameObject);
        }
    }

    private void MoveCrop()
    {
        transform.Translate(cropSpeed* (target.position - transform.position )* Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        farmer = other.transform.GetComponent<Farmer>();
        if(farmer.GetCorrectCropCount() >= farmer.GetTargetCropCount())
        {
            return;
        }
        farmer.AddCorrectCrop();
        target = farmer.GetBagTransform();
        Rigidbody rigidbody1 = GetComponent<Rigidbody>();
        rigidbody1.isKinematic = true;
        rigidbody1.useGravity = false;
        Collider[] colliders = GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
