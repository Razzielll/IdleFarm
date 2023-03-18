using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable 
{
    public bool Interact();
    public Vector3 GetPosition();
}
