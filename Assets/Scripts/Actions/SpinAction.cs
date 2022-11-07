using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

   
    // used to store spin amount
    private float totalSpinAmount;

    // the action cost of the action
    [SerializeField]
    private int actionCost = 1;


    private void Update()
    {
        // is inactive, reture and do nothing
        if (!isActive)
        {
            return;
        }


        // else, spin unit
        spinUnit();
            
            
    }

    // function used to spin the unit
    private void spinUnit()
    {
        // track how much unit spined
        float spinAmount = 360f * Time.deltaTime;
        // add it to the unit rotation
        this.transform.eulerAngles += new Vector3(0, spinAmount, 0);
        // add it to the total spin count
        totalSpinAmount += spinAmount;
        // if exceed 360, stop spin
        if (totalSpinAmount >= 360f)
        {
            // call delegate from base action
            ActionComplete();
        }

    }


    // used to call spin from outside
    // take an delegate function to call when function ends
    public override void TakeAction(GridPosition gridPosition_, Action onSpinActionComplete_)
    {
        // reset the spin amount
        totalSpinAmount = 0;

        // Set the action as active and start execute action
        ActionStart(onSpinActionComplete_);
    }


    // define the getaction name function
    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        // get the current grid positon of the unit
        GridPosition unitGridPosition = unit.GetUnitCurrentGridPosition();

        // return a list of the current unit positon
        return new List<GridPosition>
        {
            unitGridPosition
        };


    }


    // override the action cost
    public override int GetActionPointsCost()
    {
        return actionCost;
    }


    // sign the enemy AI Action info for the AI usage
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition_)
    {
        // return the spin action AI action info
        return new EnemyAIAction
        {
            // construction
            gridPostion = gridPosition_,
            // spin has the lowest action value 
            actionValue = 0,
        };
    }
}
