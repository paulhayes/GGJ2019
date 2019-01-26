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

    private GapExplorer gapExplorer;

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
        gapExplorer = GetComponent<GapExplorer>();
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

            SetIndicatorPos(hit.point, hit.collider);

            if (currentGap != null && PlayerInput.GetLeftMouseDown())
            {
                gapExplorer.SelectNearestGap(currentGap.GetNearest(creaseIndicator.transform.position));
                currentGap.ShowIndicator(false);
                transform.position = gapExplorer.GetHandPosWorldSpace();

                this.enabled = false;
            } else
            {
                creaseIndicator.SetActive(true);
            }
        }
        else
        {
            if (currentGap != null)
                currentGap.ShowIndicator(false);

            currentGap = null;
        }
    }

    void SetIndicatorPos (Vector3 hitPos, Collider hitCollider)
    {
        creaseIndicator.transform.SetParent(hitCollider.transform);

        Vector3 indPos = Vector3.zero;
        indPos.x = hitCollider.transform.InverseTransformPoint(hitPos).x;

        creaseIndicator.transform.localPosition = indPos;
        creaseIndicator.transform.SetParent(transform);
    }
}
