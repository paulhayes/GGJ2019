using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;

    [SerializeField] GapSpace gapSpace;

    [SerializeField] GameObject gapIndicator;

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
}
