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

    public Vector3 GetNearest()
    {
        return Vector3.zero;
    }

    public Vector3 Move(float delatPos)
    {
        return Vector3.zero;
    }


}
