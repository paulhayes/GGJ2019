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

    [SerializeField] float grabMaxShakeAmount = 5f;

    private bool isPaused;

    [SerializeField] float maxGrabDistance = 0.1f;

    [SerializeField] Vector2 mouseMovementRummageSoundThreshold = Vector3.one;

    [SerializeField] Cinemachine.CinemachineImpulseSource wobble;

    private float currentFOV, targetFOV;

    private Vector3 camPosAtGrab;
    private float handPosAtGrabY = -1;

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

                if (!holdingItem)
                    wobble.GenerateImpulse();

                OnOverItem(hoverItem);
            }

            if (PlayerInput.GetLeftMouseDown())
            {
                holdingItem = hoverItem;
                camPosAtGrab = cam.gameObject.transform.position;
                handPosAtGrabY = gapExplorer.GetHandInGapPos().y;
            }

        }
        else {
            if(hoverItem){
                OnOutItem(hoverItem);
            }
            hoverItem = null;
        }        

        float speedToMoveInOut;
        float speedToMovePan = speedToMoveInOut = maxPanSpeed * mouseSens.value * Time.deltaTime;

        if ( holdingItem ){
            if (gapExplorer.GetHandInGapPos().y == 0)
            {
                cam.transform.position = camPosAtGrab;

                camPosAtGrab = Vector3.zero;
                handPosAtGrabY = -1;

                examineView.currentItem = holdingItem;
                examineView.Begin();
                End();
            } else if (!PlayerInput.GetLeftMouse())
            {
                cam.transform.position = camPosAtGrab;

                camPosAtGrab = Vector3.zero;
                handPosAtGrabY = -1;

                holdingItem = null;

                //if (gapExplorer.GetHandInGapPos().y == 0)
                //{
                //    examineView.currentItem = holdingItem;
                //    examineView.Begin();
                //    End();
                //}
                //else
                //{
                //    holdingItem = null;
                //}
            } else
            {
                speedToMovePan *= 0;
                Vector3 newCamPos = camPosAtGrab;

                float shakeAmount = Mathf.Clamp01(ExtensionMethods.Map((1-gapExplorer.GetHandInGapPos().y), 0f, handPosAtGrabY, 0f, 1f )) * Time.deltaTime;
                //Debug.Log("handPosAtGrabY: " + handPosAtGrabY.ToString() + ", 1-GetHandInGapPos.y: " + (1-gapExplorer.GetHandInGapPos().y).ToString());
                Debug.Log(shakeAmount);

                newCamPos.x += grabMaxShakeAmount * UnityEngine.Random.Range(-1f, 1f) * shakeAmount;
                newCamPos.z += grabMaxShakeAmount * UnityEngine.Random.Range(-1f, 1f) * shakeAmount;
                cam.transform.position = newCamPos;
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
