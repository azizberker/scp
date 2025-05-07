using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 'abstract' kelimesini çýkarýyoruz:
public class Targetable : MonoBehaviour
{
    public GameObject infoObject;

    public void ToggleHighlight(bool status)
    {
        infoObject.SetActive(status);
    }
}
