using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedCrop : MonoBehaviour
{
    Transform target;
    [SerializeField] float speed = 3f;
    [SerializeField] float minDistance;
    Farmer farmer;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Barn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCrop();
    }

    private void MoveCrop()
    {
        transform.Translate(target.position * Time.deltaTime * speed);
        if(Vector3.Distance(target.position,transform.position) < minDistance)
        {
           // farmer.SellCrop();
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetFarmer(Farmer farmer)
    {
        this.farmer = farmer;
    }
}
