using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Collectable
{
    public override void Collect()
    {
        base.Collect();
        Debug.Log("Item collected!");
    }
}
    

