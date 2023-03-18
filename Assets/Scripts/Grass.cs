using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour, IClickable, Crop
{
    [SerializeField] float timeToRipen = 10f;
    [SerializeField] GameObject harvestPrefab;
    bool isReadyToHarvest = false;
    public event Action OnHarvest;
    public bool Interact()
    {
        return IsReadyToHarvest();
       // animator.SetTrigger("Crop");
    }

    public bool IsReadyToHarvest()
    {
        return isReadyToHarvest;
    }

    public void SetHarvestable()
    {
        isReadyToHarvest = true;
    }

    public float GetTimeToRipen()
    {
        return timeToRipen;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Harvest()
    {
        if (!isReadyToHarvest)
        {
            return;
        }
        GameObject harvesetGO = Instantiate(harvestPrefab);
        harvesetGO.transform.position = this.transform.position;
        OnHarvest?.Invoke();
        Destroy(this.gameObject);
    }

    
    public void CollectCrop(GameObject collector)
    {

    }
}
