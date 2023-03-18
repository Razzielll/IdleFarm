using UnityEngine;

public class Wind : MonoBehaviour
{
    public float windStrength = 1.0f;
    public float wiggleStrength = 0.1f;
    public float wiggleSpeed = 5.0f;
    public GameObject grassParent;

    void OnTriggerStay(Collider other)
    {
        if (other.transform.parent == grassParent.transform)
        {
            Vector3 windForce = transform.forward * windStrength;
            other.attachedRigidbody.AddForce(windForce);

            Vector3 wiggleForce = new Vector3(Mathf.Sin(Time.time * wiggleSpeed), 0, Mathf.Cos(Time.time * wiggleSpeed)) * wiggleStrength;
            other.attachedRigidbody.AddForce(wiggleForce);
        }
    }
}