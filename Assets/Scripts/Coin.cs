using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float minDistance =100f;
    Farmer farmer;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetFarmer(Farmer farmer, Transform target)
    {
        this.farmer = farmer;
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((target.position - transform.position).normalized*speed *Time.deltaTime);
        if(Vector3.Distance(target.position, transform.position) < minDistance)
        {
            farmer.SoldCrop();
            Destroy(gameObject);
        }
    }
}
