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

    [SerializeField] SettingFloat mouseSens;

    private bool isPaused;

    [SerializeField] float maxGrabDistance = 0.1f;

    [SerializeField] Cinemachine.CinemachineImpulseSource wobble;

    private float currentFOV, targetFOV;

    private Cinemachine.CinemachineBrain cinemachineBrain;

    GapExplorer gapExplorer;
    SofaView sofaView;

    PauseController pauseController;

    Item holdingItem; 
    Item hoverItem;
    ExamineView examineView;

    public override void Begin()
    {
        this.enabled = true;
        currentFOV = gapFOVMinMax.y;
        gapExplorer.MoveIn(0);
        gapExplorer.SetFOV(currentFOV);
        PlayerInput.ShowMouse(false);
        holdingItem = null;
        //sofaView.enabled = false;
        //throw new System.NotImplementedException();
    }

    public override void End()
    {
        //sofaView.enabled = true;
        PlayerInput.ShowMouse(true);

        this.enabled = false;
        gapExplorer.Deselect();

        //throw new System.NotImplementedException();
    }

    private void Awake()
    {
        cinemachineBrain = cam.GetComponent<Cinemachine.CinemachineBrain>();

        sofaView = GetComponent<SofaView>();
        examineView = GetComponent<ExamineView>();
        gapExplorer = GetComponent<GapExplorer>();

        pauseController = GetComponent<PauseController>();
    }

    private void Start()
    {
        Begin();
    }

    private void Update()
    {            

        if (pauseController.IsPaused())
        {
            if (!isPaused)
            {
                isPaused = true;
                PlayerInput.ShowMouse(true);
            }

            return;
        } else
        {
            if (isPaused == true)
                PlayerInput.ShowMouse(false);
        }

        if (isPaused != pauseController.IsPaused())
            isPaused = pauseController.IsPaused();

        if (PlayerInput.GetRightMouseDown())
        {
            sofaView.Begin();
            End();
        }        



        if (cinemachineBrain.IsBlending)
            return;

        float dist = 0;
        Item closestItem = gapExplorer.GetClosestItem( ref dist );
        
        Debug.LogFormat("> {0}",dist);
        if( dist <= maxGrabDistance ){
            if( closestItem != hoverItem){
                wobble.GenerateImpulse();
            }
            hoverItem = closestItem;
            Debug.LogFormat("Over {0}",hoverItem.name);
        }
        else {
            hoverItem = null;
        }        

        if( PlayerInput.GetLeftMouseDown() )
        {            
            holdingItem = closestItem;            
        }

        if( holdingItem && !PlayerInput.GetLeftMouse() ){
            if( gapExplorer.GetHandInGapPos().y == 0 ){
                examineView.currentItem = holdingItem;
                examineView.Begin();
                End();
            }
            else {
                holdingItem = null;
            }
        }

        float speedToMoveInOut;
        float speedToMovePan = speedToMoveInOut = maxPanSpeed * mouseSens.value * Time.deltaTime;

        if (holdingItem && PlayerInput.GetLeftMouse())
        {
            
            speedToMovePan *= gapPanSpeedMod;
            speedToMoveInOut *= gapInOutSpeedMod;


        }


        gapExplorer.MoveLeft(speedToMovePan * -PlayerInput.GetMouseX());
        gapExplorer.MoveIn(speedToMovePan * -PlayerInput.GetMouseY());

        currentFOV = Mathf.Lerp(currentFOV, Mathf.Lerp(gapFOVMinMax.x, gapFOVMinMax.y, 1-gapExplorer.GetHandInGapPos().y), 0.35f);
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
