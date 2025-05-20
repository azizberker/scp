using UnityEngine;

public class Collectable : MonoBehaviour
{
    public virtual void Collect()
    {
        Debug.Log("Collect �a�r�ld� ve obje yok edildi: " + gameObject.name);
        Destroy(gameObject);
    }
}
