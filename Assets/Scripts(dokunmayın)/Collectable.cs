using UnityEngine;

public class Collectable : MonoBehaviour
{
    public virtual void Collect()
    {
        Debug.Log("Collect çaðrýldý ve obje yok edildi: " + gameObject.name);
        Destroy(gameObject);
    }
}
