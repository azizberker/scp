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
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.TryGetComponent(out Targetable targetable))
            {
                if (currentTargetable != targetable)
                {
                    if (currentTargetable != null)
                        currentTargetable.ToggleHighlight(false);

                    currentTargetable = targetable;
                    currentTargetable.ToggleHighlight(true);

                    if (currentTargetable.TryGetComponent(out Collectable collectable))
                    {
                        currentCollectable = collectable;
                    }
                    else
                    {
                        currentCollectable = null;
                    }
                }
            }
            else if (currentTargetable)
            {
                currentTargetable.ToggleHighlight(false);
                currentTargetable = null;
                currentCollectable = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentCollectable != null)
            {
                float distance = Vector3.Distance(cam.transform.position, currentCollectable.transform.position);
                if (distance <= currentCollectable.collectionRange)
                {
                    currentCollectable.Collect();
                    currentCollectable = null;
                }
                else
                {
                    Debug.Log("Too far to collect!");
                }
            }
        }
    }
}
