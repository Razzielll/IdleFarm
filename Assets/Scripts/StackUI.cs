using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StackUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stackText;
    [SerializeField] Farmer farmer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stackText.text = "Stack full " + farmer.GetCropCount() + "/" + farmer.GetTargetCropCount();
    }
}
