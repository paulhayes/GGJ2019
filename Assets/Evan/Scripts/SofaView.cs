using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaView : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask whatIsGapSpace;

    [SerializeField]
    private GameObject creaseIndicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        creaseIndicator.SetActive(false);

        Ray ray = cam.ScreenPointToRay(PlayerInput.GetMousePos());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsGapSpace))
        {
            creaseIndicator.SetActive(true);
            SetIndicatorPos(hit.point, hit.collider);
        }


    }


    void SetIndicatorPos (Vector3 hitPos, Collider hitCollider)
    {
        creaseIndicator.transform.SetParent(hitCollider.transform);

        Vector3 indPos = Vector3.zero;
        //indPos.y = hitCollider.bounds.size.y*0.5f;
        indPos.x = hitCollider.transform.InverseTransformPoint(hitPos).x;

        creaseIndicator.transform.localPosition = indPos;

        creaseIndicator.transform.SetParent(transform);
    }
}
