using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;

    [SerializeField] GapSpace gapSpace;

    [SerializeField] GameObject gapIndicator;

    [SerializeField] Cinemachine.CinemachineVirtualCamera _camera;

    [SerializeField] GameObject targetPosition;

    Collider coll;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    public GapSpace GetGapSpace()
    {
        return gapSpace;
    }


    GapSpace GetSpace()
    {
        return gapSpace;
    }

    public Collider GetCollider ()
    {
        return coll;
    }

    public void ShowIndicator (bool visible)
    {
        gapIndicator.SetActive(visible);
    }

    public void Select()
    {
        _camera.gameObject.SetActive(true);
        _camera.LookAt = targetPosition.transform;
        _camera.Follow = targetPosition.transform;
    }

    public void Deselect()
    {
        _camera.gameObject.SetActive(false);
    }

    public void SetFOV (float fov)
    {
        _camera.m_Lens.FieldOfView = fov;
    }

    public float NormalizedPosOnLine(Vector3 position)
    {
        var pos = GetNearest( position ) - start.position;
        var endPos = end.position - start.position;
        return pos.magnitude / endPos.magnitude;
    }

    public Vector3 GetNearest(Vector3 pos)
    {
        var endPos = ( end.position - start.position );
        var intersectionPos = Vector3.Project(pos-start.position, endPos.normalized);
        if(Vector3.Dot(endPos,intersectionPos)<0){
            return start.position;
        }
        if(intersectionPos.sqrMagnitude >= endPos.sqrMagnitude){
            return end.position;            
        }
        return intersectionPos + start.position;
    }

    public Vector3 Move(float delatPos)
    {
        var endPos = ( end.position - start.position );
        var pos = ( targetPosition.transform.position - start.position ) + endPos.normalized * delatPos;
        if(Vector3.Dot(endPos,pos)<0){
            pos = Vector3.zero;
        }
        if(pos.sqrMagnitude >= endPos.sqrMagnitude){
            pos = endPos;
        }
        
        targetPosition.transform.position = pos + start.position;
        return targetPosition.transform.position;
    }

}
