using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapExplorer : MonoBehaviour
{
    [SerializeField]
    Gap[] gaps;

    protected Gap currentGap;
    protected Vector3 currentPos;
    protected Vector2 handInGapPosition;

    void MoveLeft(float amount)
    {
        MoveRight(-amount);       
    }

    void MoveRight(float amount)
    {
        if(!currentGap)
            return;
        
        currentPos = currentGap.Move(amount);        
    }

    void MoveIn(float amount){
        handInGapPosition.y += amount;
        handInGapPosition.y = Mathf.Clamp01(handInGapPosition.y);
    }

    void MoveOut(float amount){
        MoveIn(-amount);
    }
    
    public Vector3 GetHandPosWorldSpace()
    {
        return currentPos;
    }

    public Item GetClosestItem(ref float distance){
        return currentGap.GetGapSpace().GetClosestItem(handInGapPosition, ref distance);
    }


    public void SelectNearestGap(Vector3 pos)
    {
        Gap nearest = null;
        float nearestDistSqr = float.MaxValue;
        for(int i=0;i<gaps.Length;i++){
            var gapPos = gaps[i].GetNearest(pos);
            var nearPos = -gaps[i].GetNearest(pos);
            var distSqr = (pos-nearPos).sqrMagnitude;
            if( distSqr < nearestDistSqr ){
                nearestDistSqr = distSqr;
                nearest = gaps[i];
                currentPos = nearPos;
            }
        }

    }

}
