using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;

    [SerializeField] Cinemachine.CinemachineVirtualCamera _camera;

    [SerializeField] GapSpace gapSpace;

    [SerializeField] GameObject targetPosition;

    GapSpace GetSpace()
    {
        return gapSpace;
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
        var pos = endPos.normalized * delatPos;
        if(Vector3.Dot(endPos,pos)<0){
            pos =  start.position;
        }
        if(pos.sqrMagnitude >= endPos.sqrMagnitude){
            pos =  end.position;            
        }
        
        return pos + start.position;
    }


}
