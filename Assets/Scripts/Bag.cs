using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bag : MonoBehaviour
{
    [SerializeField] GameObject[] cropPool;
    [SerializeField] float wiggleStrength;
    [SerializeField] float wiggleSpeed;
    Farmer farmer;
    NavMeshAgent navMeshAgent;

    int totalCrops;
    int currentCropsCount =0;
    

    // Start is called before the first frame update
    void Start()
    {
        totalCrops = cropPool.Length;
        DisablePool();
        farmer = GetComponentInParent<Farmer>();
        farmer.OnCropHarvested += CropAdded;
        farmer.OnCropMoved += OnCropRemoved;
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    void CropAdded()
    {
        if(currentCropsCount < totalCrops)
        {
            cropPool[currentCropsCount].SetActive(true);
            currentCropsCount++;
        }       
    }

    void Wiggle()
    {
        if (navMeshAgent.isStopped)
        {
            return;
        }
        Vector3 wiggle = new Vector3(0, Mathf.Sin(Time.time * wiggleSpeed), 0) * wiggleStrength;
        
        transform.localEulerAngles =wiggle;
    }

    void OnCropRemoved()
    {
        if(currentCropsCount > 0)
        {
            currentCropsCount--;
            cropPool[currentCropsCount].SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Wiggle();
    }

    void DisablePool()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
