using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchView : AbstractView
{
    [SerializeField, Range(0, 0.5f)] float mouseSideThreshold = 0.15f;
    [SerializeField] float maxPanSpeed = 1;

    [SerializeField] Camera cam;

    GapExplorer gapExplorer;
    SofaView sofaView;

    public override void Begin()
    {
        this.enabled = true;
        //sofaView.enabled = false;
        //throw new System.NotImplementedException();
    }

    public override void End()
    {
        //sofaView.enabled = true;
        this.enabled = false;
        //throw new System.NotImplementedException();
    }

    private void Awake()
    {
        sofaView = GetComponent<SofaView>();
        gapExplorer = GetComponent<GapExplorer>();
    }

    private void Start()
    {
        Begin();
    }

    private void Update()
    {
        float mousePosX = cam.ScreenToViewportPoint(PlayerInput.GetMousePos()).x;
        
        float speedToMove = 0;

        if (mousePosX < mouseSideThreshold)
        {
               
            speedToMove = maxPanSpeed * Time.deltaTime;
            Debug.Log(speedToMove);
            gapExplorer.MoveLeft(speedToMove);

            //transform.position = gapExplorer.GetHandPosWorldSpace();
        } else if (mousePosX > 1 - mouseSideThreshold)
        {
            speedToMove = maxPanSpeed * Time.deltaTime;
            Debug.Log(speedToMove);
            gapExplorer.MoveRight(speedToMove);

            //transform.position = gapExplorer.GetHandPosWorldSpace();
        }
    }
}
