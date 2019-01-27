using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchView : AbstractView
{
    [SerializeField, Range(0, 0.5f)] float mouseSideThreshold = 0.15f;
    [SerializeField] float maxPanSpeed = 1;

    [SerializeField] Camera cam;

    [SerializeField] private Cinemachine.CinemachineBrain cinemachineBrain;

    [SerializeField] float gapPanSpeedMod = 0.25f, gapInOutSpeedMod = 0.25f;

    [SerializeField] Vector2 gapFOVMinMax;

    [SerializeField] SettingFloat mouseSens;

    [SerializeField] SoundCollection sofaRummageSounds;

    [SerializeField] AudioSource seachingSoundSource;

    [SerializeField] ItemHint itemHint;


    private bool isPaused;

    [SerializeField] float maxGrabDistance = 0.1f;

    [SerializeField] Vector2 mouseMovementRummageSoundThreshold = Vector3.one;

    [SerializeField] Cinemachine.CinemachineImpulseSource wobble;

    private float currentFOV, targetFOV;


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
        

        //throw new System.NotImplementedException();
    }

    private void Awake()
    {
        //cinemachineBrain = cam.GetComponent<Cinemachine.CinemachineBrain>();

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
            gapExplorer.Deselect();
            End();
        }        



        if (cinemachineBrain.IsBlending)
            return;

        float dist = float.MaxValue;
        Item closestItem = gapExplorer.GetClosestItem( ref dist );
        
        if( dist <= maxGrabDistance ){
            if( closestItem != hoverItem){
                hoverItem = closestItem;
                wobble.GenerateImpulse();
                OnOverItem(hoverItem);
            }
        }
        else {
            if(hoverItem){
                OnOutItem(hoverItem);
            }
            hoverItem = null;
        }        

        if( PlayerInput.GetLeftMouseDown() )
        {            
            holdingItem = hoverItem;            
        }

        float speedToMoveInOut;
        float speedToMovePan = speedToMoveInOut = maxPanSpeed * mouseSens.value * Time.deltaTime;

        if ( holdingItem ){
            if (!PlayerInput.GetLeftMouse())
            {
                if (gapExplorer.GetHandInGapPos().y == 0)
                {
                    examineView.currentItem = holdingItem;
                    examineView.Begin();
                    End();
                }
                else
                {
                    holdingItem = null;
                }
            } else
            {
                speedToMovePan *= 0;
            }
        }

        if (holdingItem && PlayerInput.GetLeftMouse())
        {
            
            speedToMovePan *= gapPanSpeedMod;
            speedToMoveInOut *= gapInOutSpeedMod;


        }


        if( Mathf.Abs( PlayerInput.GetMouseY() ) > mouseMovementRummageSoundThreshold.y && !seachingSoundSource.isPlaying ||
            Mathf.Abs( PlayerInput.GetMouseX() ) > mouseMovementRummageSoundThreshold.x && !seachingSoundSource.isPlaying
        ){
            seachingSoundSource.PlayOneShot( sofaRummageSounds.GetNext() );
        }

        gapExplorer.MoveLeft(speedToMovePan * -PlayerInput.GetMouseX());

       
        gapExplorer.MoveIn(speedToMoveInOut * -PlayerInput.GetMouseY());

        currentFOV = Mathf.Lerp(currentFOV, Mathf.Lerp(gapFOVMinMax.x, gapFOVMinMax.y, 1-gapExplorer.GetHandInGapPos().y), 0.35f);
        cam.fieldOfView = currentFOV;
        //gapExplorer.SetFOV(currentFOV);

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

    private void OnOverItem(Item hoverItem)
    {
        //Debug.LogFormat("Over {0}",hoverItem.name);

        itemHint.Hint(hoverItem);        
    }
    private void OnOutItem(Item hoverItem)
    {
        //Debug.LogFormat("Out");
        itemHint.Hint(null);
    }

}
