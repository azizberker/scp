using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Targetable
{
    public float collectionRange = 3f;
    public virtual void Collect()
    {
        Destroy(gameObject);    
    }

}


