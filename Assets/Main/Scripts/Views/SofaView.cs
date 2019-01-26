using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaView : AbstractView
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask whatIsGapSpace;

    [SerializeField]
    private GameObject creaseIndicator;

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera introCam;

    private Cinemachine.CinemachineBrain cinemachineBrain;

    private Gap currentGap;


    public override void Begin()
    {

        throw new System.NotImplementedException();
    }

    public override void End()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        cinemachineBrain = cam.GetComponent<Cinemachine.CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (introCam.gameObject.activeSelf || cinemachineBrain.IsBlending)
            return;

        creaseIndicator.SetActive(false);

        Ray ray = cam.ScreenPointToRay(PlayerInput.GetMousePos());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsGapSpace))
        {
            if (currentGap == null || currentGap.GetCollider() != hit.collider) {
                if (currentGap != null)
                    currentGap.ShowIndicator(false);

                currentGap = hit.collider.GetComponent<Gap>();

                if (currentGap != null)
                    currentGap.ShowIndicator(true);
            }

            Vector3 indPos = CalculateIndicatorPos(hit.point, hit.collider);

            if (currentGap != null && PlayerInput.GetLeftMouseDown())
            {
                Vector3 nearestPos = currentGap.GetNearest(hit.collider.transform.TransformPoint(indPos));
                transform.position = nearestPos;
            } else
            {
                creaseIndicator.SetActive(true);
                SetIndicatorPos(indPos, hit.collider);
            }
        }
        else
        {
            if (currentGap != null)
                currentGap.ShowIndicator(false);

            currentGap = null;
        }
    }

    Vector3 CalculateIndicatorPos (Vector3 hitPos, Collider hitCollider)
    {
        Vector3 indPos = Vector3.zero;
        indPos.x = hitCollider.transform.InverseTransformPoint(hitPos).x;

        return indPos;
    }

    void SetIndicatorPos (Vector3 indPos, Collider hitCollider)
    {
        creaseIndicator.transform.SetParent(hitCollider.transform);
        creaseIndicator.transform.localPosition = indPos;
        creaseIndicator.transform.SetParent(transform);
    }
}
