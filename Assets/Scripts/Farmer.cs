using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    [SerializeField] Transform bagTransform;
    [SerializeField] Transform sellPoint;
    [SerializeField] float searchRadius;
    [SerializeField] LayerMask cropLayer;
    Instrument instrument;
    [SerializeField] int cropsCount=0;
    [SerializeField] int correctCropsCount=0;
    Crop currentTarget;
    private Harvest currentHarvestTarget;
    PlayerController playerController;
    bool isHarvesting = false;
    [SerializeField] int targetCropCount=10;
    float moneyCount = 0;
    private bool isSellingCrop;
    public event Action OnCropHarvested;
    public event Action OnCropMoved;
    public event Action OnCropSold;
    [SerializeField] Canvas canvas;
    [SerializeField] Coin coinPrefab;
    [SerializeField] Transform moneyUI;
    [SerializeField] float cropValue =15f;
    [SerializeField] Transform Barn;
    [SerializeField] GameObject cropPrefab;
    [SerializeField] float sellDelay;
    [SerializeField] Transform bag;
    private bool isCoinActive;
   // [SerializeField] int moneyReservCount;

    // Start is called before the first frame update
    void Start()
    {
        //   instrument = GetComponentInChildren<Instrument>(true);
        //  Debug.Log(instrument.gameObject.name);
        // instrument.OnTrigger += Instrument_OnTrigger;
         playerController = GetComponent<PlayerController>();
    }

  /*  private void Instrument_OnTrigger(Collider crop)
    {
        crop.GetComponent<Crop>().Harvest();
    }*/

    // Update is called once per frame
    void Update()
    {
        if(cropsCount >= targetCropCount)
        {
            GoToSellCrop();
            isSellingCrop = true;
            return;
        }

        if (isSellingCrop)
        {
            if (cropsCount < 1)
            {
                isSellingCrop = false;
            }
            return;
        }
        

        if(currentTarget == null)
        {
            UpdateTargets();
            return;
        }
        if(currentHarvestTarget != null &&  Vector3.Distance( transform.position, currentTarget.GetPosition()) > playerController.GetCuttingRange())
        {
            
            playerController.MoveTo(currentTarget.GetPosition());
            return;
        }
        playerController.CancelMoveAction();    

        if (!isHarvesting)
        {
            HarvestCrop();

        }
        
    }

    private void HarvestCrop()
    {
        transform.LookAt(currentTarget.GetPosition());
        GetComponent<Animator>().SetTrigger("Crop");
        isHarvesting = true;
        
    }

    public Transform GetBagTransform()
    {
        return bagTransform;
    }

    //Animation Event

    void StopHarvest()
    {
        isHarvesting = false;
        currentTarget = null;
    }

    public float GetMoneyCount()
    {
        return moneyCount;
    }
    public void MoveCropToBarn()
    {
        
        cropsCount--;
        correctCropsCount--;
     //   moneyReservCount++;
        GameObject cropGO = Instantiate(cropPrefab);
        cropGO.transform.position = bag.position;
        StartCoroutine(SellCrops());
        OnCropMoved?.Invoke();
        // coin.SetFarmer(this, moneyUI);
        // OnCropSold?.Invoke();
    }
    public void SellCrop()
    {
        
        Coin coin = Instantiate(coinPrefab, canvas.transform);
        coin.transform.position = Camera.main.WorldToScreenPoint(Barn.position);
        coin.SetFarmer(this, moneyUI);
     //   isCoinActive = true;
    }

    IEnumerator SellCrops()
    {
        yield return new WaitForSeconds(sellDelay);
        SellCrop();
    }

    public void SoldCrop()
    {
        OnCropSold?.Invoke();

        StartCoroutine(MoneyIncreaseCoroutine());
    }

    IEnumerator MoneyIncreaseCoroutine()
    {
        
        yield return new WaitForEndOfFrame();
        moneyCount += cropValue;
        
      //  moneyReservCount = 0;
       // isCoinActive = false;
    }

    public int GetTargetCropCount()
    {
        return targetCropCount;
    }

    public int GetCropCount()
    {
        return cropsCount;
    }
    public int GetCorrectCropCount()
    {
        return correctCropsCount;
    }

    void GoToSellCrop()
    {
        playerController.MoveTo(sellPoint.position);
        
        
    }

    public void AddCrop()
    {
        cropsCount++;
        
        OnCropHarvested?.Invoke();
    }

    public void AddCorrectCrop()
    {
        correctCropsCount++;
        
    }

    void UpdateTargets()
    {
        FindNewTarget();
    }

    void FindNewTarget()
    {

        Harvest harvest = FindNewTargetInRange(searchRadius);
        if(harvest == null)
        {
            return;
        }
        currentTarget = harvest.GetComponent<Crop>();
        currentHarvestTarget = harvest;
    }

    public bool IsCoinActive()
    {
        return isCoinActive;
    }
    private Harvest FindNewTargetInRange(float searchRadius)
    {        
        Harvest best = null;
        float bestDistance = Mathf.Infinity;
        foreach (var candidate in FindAllTargetsInRange(searchRadius))
        {
            float candidateDistance = Vector3.Distance(transform.position, candidate.transform.position);
            if (candidateDistance < bestDistance)
            {
                best = candidate;
                bestDistance = candidateDistance;
            }
        }
        return best;
    }

    private IEnumerable<Harvest> FindAllTargetsInRange(float searchRadius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, searchRadius, Vector3.up, 0);
        
        foreach (var hit in hits)
        {
            Harvest crop = hit.transform.GetComponent<Harvest>();
            if (crop != null && crop.GetComponent<Crop>().IsReadyToHarvest())
            {
                yield return crop;
            }
        }
    }
}
