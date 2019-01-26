using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;


    [SerializeField] GapSpace gapSpace;

    GapSpace GetSpace()
    {
        return gapSpace;
    }
}
