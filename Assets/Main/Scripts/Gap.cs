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

    public Vector3 GetNearest()
    {
        return Vector3.zero;
    }

    public Vector3 Move(float delatPos)
    {
        return Vector3.zero;
    }

}
