using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaView : AbstractView
{
    public static bool triggerTutorial = false;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask whatIsGapSpace, whatIsLockbox;

    [SerializeField]
    private GameObject creaseIndicator;

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera introCam;

    [SerializeField] private Cinemachine.CinemachineBrain cinemachineBrain;

    [SerializeField] DialogManager dialogManager;

    [SerializeField] Dialog[] introDialogs;

    Lockbox lockbox;

    private GapExplorer gapExplorer;
    private SearchView searchView;

    private PauseController pauseController;

    private Gap currentGap;


    public override void Begin()
    {
        this.enabled = true;

        if (currentGap)
            currentGap.Deselect();

        introDialogs[1].AutoContinue = false;

        //searchView.enabled = false;

        //throw new System.NotImplementedException();
    }

    public override void End()
    {
        this.enabled = false;

        //throw new System.NotImplementedException();
    }

    private void Awake()
    {
        Item.ResetAll();

        gapExplorer = GetComponent<GapExplorer>();
        searchView = GetComponent<SearchView>();

        pauseController = GetComponent<PauseController>();

        //cinemachineBrain = cam.GetComponent<Cinemachine.CinemachineBrain>();
    }

    private void Start()
    {
        Begin();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseController.IsPaused() || introCam.gameObject.activeSelf || cinemachineBrain.IsBlending)
            return;

        creaseIndicator.SetActive(false);

        if (triggerTutorial)
        {
            dialogManager.Play(introDialogs[0]);
            triggerTutorial = false;
        }

        if (DialogManager.CurrentMessage == introDialogs[0])
            return;

        Ray ray = cam.ScreenPointToRay(PlayerInput.GetMousePos());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsLockbox))
        {
            if (DialogManager.CurrentMessage != introDialogs[0] && DialogManager.CurrentMessage != introDialogs[1])
            {
                Debug.Log("is this working");

                if (!lockbox)
                {
                    lockbox = hit.collider.GetComponent<Lockbox>();
                    lockbox.SetHover(true);
                }

                if (PlayerInput.GetLeftMouseDown())
                    lockbox.OpenBox();

                if (currentGap != null)
                    currentGap.ShowIndicator(false);

                currentGap = null;
            }

        } else
        {
            if (lockbox)
            {
                lockbox.SetHover(false);
                lockbox = null;
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsGapSpace))
            {
                if (currentGap == null || currentGap.GetCollider() != hit.collider)
                {
                    if (currentGap != null)
                        currentGap.ShowIndicator(false);

                    currentGap = hit.collider.GetComponent<Gap>();

                    if (currentGap != null)
                        currentGap.ShowIndicator(true);
                }

                SetIndicatorPos(hit.point, hit.collider);

                if (currentGap != null && PlayerInput.GetLeftMouseDown())
                {
                    if (DialogManager.CurrentMessage == introDialogs[1])
                        introDialogs[1].AutoContinue = true;

                    gapExplorer.SelectNearestGap(currentGap.GetNearest(creaseIndicator.transform.position));
                    currentGap.ShowIndicator(false);
                    transform.position = gapExplorer.GetHandPosWorldSpace();

                    searchView.Begin();
                    End();
                }
                else
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
