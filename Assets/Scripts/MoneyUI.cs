using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] Farmer farmer;
    [SerializeField] TextMeshProUGUI moneyValue;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float pingPongAmplitude = 1f;
    [SerializeField] float pulsationFrequencyY = 1f;
    [SerializeField] float pulsationFrequencyX = 1f;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] int wiggleCount;
    Vector2 startingTextPosition;
    bool isWiggling = false;

    private void UpdateTextPosition(int i)
    {
        float x = pingPongAmplitude * (Mathf.Sin(pulsationFrequencyX * i * 2 * Mathf.PI / wiggleCount) );
        float y = pingPongAmplitude * (Mathf.Cos(pulsationFrequencyY * i * 2 * Mathf.PI / wiggleCount) );
        coinsText.GetComponent<RectTransform>().position +=  new Vector3(x, y, 0);
    }

    private void Update()
    {
        
        moneyValue.text = farmer.GetMoneyCount().ToString();
    }

    IEnumerator TextWiggle()
    {
        for (int i = 0; i < wiggleCount; i++)
        {
            
            yield return new WaitForEndOfFrame();
            UpdateTextPosition(i);
        }
        yield return new WaitForEndOfFrame();
        //coinsText.GetComponent<RectTransform>().anchoredPosition = startingTextPosition;
        isWiggling = false;
    }

    private void Start()
    {
        startingTextPosition = coinsText.GetComponent<RectTransform>().anchoredPosition;
        farmer.OnCropSold += Farmer_OnCropSold;
    }

    private void Farmer_OnCropSold()
    {
        if (isWiggling)
        {
            return;
        }
        Wiggle();
    }

    private void Wiggle()
    {
        isWiggling = true;
        StartCoroutine(TextWiggle());
        

    }
}
