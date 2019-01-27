using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchView : AbstractView
{
    [SerializeField, Range(0, 0.5f)] float mouseSideThreshold = 0.15f;
    [SerializeField] float maxPanSpeed = 1;

    [SerializeField] Camera cam;

    [SerializeField] float gapPanSpeedMod = 0.25f, gapInOutSpeedMod = 0.25f;

    [SerializeField] Vector2 gapFOVMinMax;

    private float currentFOV, targetFOV;

    private Cinemachine.CinemachineBrain cinemachineBrain;

    GapExplorer gapExplorer;
    SofaView sofaView;

    public override void Begin()
    {
        this.enabled = true;
        currentFOV = gapFOVMinMax.y;
        gapExplorer.MoveIn(2);
        gapExplorer.SetFOV(currentFOV);
        PlayerInput.ShowMouse(false);

        //sofaView.enabled = false;
        //throw new System.NotImplementedException();
    }

    public override void End()
    {
        //sofaView.enabled = true;
        PlayerInput.ShowMouse(true);

        this.enabled = false;

        //throw new System.NotImplementedException();
    }

    private void Awake()
    {
        cinemachineBrain = cam.GetComponent<Cinemachine.CinemachineBrain>();

        sofaView = GetComponent<SofaView>();
        gapExplorer = GetComponent<GapExplorer>();
    }

    private void Start()
    {
        Begin();
    }

    private void Update()
    {
        if (PlayerInput.GetRightMouseDown())
        {
            sofaView.Begin();
            End();
        }

        if (cinemachineBrain.IsBlending)
            return;

        float speedToMoveInOut;
        float speedToMovePan = speedToMoveInOut = maxPanSpeed * Time.deltaTime;

        if (PlayerInput.GetLeftMouse())
        {
            speedToMovePan *= gapPanSpeedMod;
            speedToMoveInOut *= gapInOutSpeedMod;
        }

        gapExplorer.MoveLeft(speedToMovePan * -PlayerInput.GetMouseX());
        gapExplorer.MoveIn(speedToMovePan * PlayerInput.GetMouseY());

        currentFOV = Mathf.Lerp(currentFOV, Mathf.Lerp(gapFOVMinMax.x, gapFOVMinMax.y, gapExplorer.GetHandInGapPos().y), 0.35f);
        gapExplorer.SetFOV(currentFOV);

        //float mousePosX = cam.ScreenToViewportPoint(PlayerInput.GetMousePos()).x;

        //float speedToMove = 0;

        //if (mousePosX < mouseSideThreshold)
        //{

        //    speedToMove = maxPanSpeed * Time.deltaTime;
        //    gapExplorer.MoveLeft(speedToMove);

        //    //transform.position = gapExplorer.GetHandPosWorldSpace();
        //} else if (mousePosX > 1 - mouseSideThreshold)
        //{
        //    speedToMove = maxPanSpeed * Time.deltaTime;
        //    gapExplorer.MoveRight(speedToMove);

        //    //transform.position = gapExplorer.GetHandPosWorldSpace();
        //}
    }
}
