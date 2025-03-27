using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    private Targetable currentTargetable;
    private Collectable currentCollectable;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if(Physics.Raycast(ray,out hit, 100))
        {
          if(hit.collider.TryGetComponent(out Targetable targetable))
            {
                currentTargetable = targetable;
                currentTargetable.ToggleHighlight(true);
                if(currentTargetable.TryGetComponent(out Collectable collectable))
                {
                    currentCollectable = collectable;
                }
            }
          else if (currentTargetable)
            {
                currentTargetable.ToggleHighlight(false);
                currentTargetable = null;
                if (currentCollectable)
                {
                    currentCollectable = null;
                }
            }


        } 
        if(Input.GetMouseButtonDown(0))
        {
            if (currentCollectable)
            {
                currentCollectable.Collect();
                currentCollectable = null;
            }
        }
    }
}
