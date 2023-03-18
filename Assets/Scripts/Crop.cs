using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Crop 
{
    public void CollectCrop(GameObject collector);
    public float GetTimeToRipen();
    public void SetHarvestable();
    public event Action OnHarvest;
    public void Harvest();
    public Vector3 GetPosition();
    public bool IsReadyToHarvest();
}
