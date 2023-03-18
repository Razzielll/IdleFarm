using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBed : MonoBehaviour
{
    [SerializeField] GameObject cropPrefab;
    [SerializeField] GameObject cropedPrefab;
    [SerializeField] float timeBetweenPlants = 2f;
    [SerializeField] float targertSize = 100f;

    float timer = 0;
    bool isReadyToHarvest = false;
    GameObject currentCrop;
    Vector3 defaultSize = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        PlantSeeds();
    }

    public void Ripe()
    {
        if(currentCrop == null)
        {
            return;
        }
        
        float ripeSize = targertSize * timer / currentCrop.GetComponent<Crop>().GetTimeToRipen();
        
        if(ripeSize >= targertSize)
        {
            isReadyToHarvest = true;
            currentCrop.GetComponent<Crop>().SetHarvestable();
            return;
        }
        Vector3 ripeProgress = new Vector3(defaultSize.x, defaultSize.y, ripeSize);
        currentCrop.transform.localScale = ripeProgress;
    }

    public void PlantSeeds()
    {
        timer = 0f;
        currentCrop = Instantiate(cropPrefab, transform);
        
        defaultSize = currentCrop.transform.localScale;
        isReadyToHarvest =false;
        currentCrop.GetComponent<Crop>().OnHarvest += CollectCrop;
    }

    public void CollectCrop()
    {
        isReadyToHarvest = false;
        currentCrop = null;
        StartCoroutine(CropCycle());
    }

    IEnumerator CropCycle()
    {
        yield return new WaitForSeconds(timeBetweenPlants);
        PlantSeeds();
    }

    // Update is called once per frame
    void Update()
    {
        Ripe();
        UpdateTimers();
    }

    void UpdateTimers()
    {
        timer +=Time.deltaTime;
    }
}
