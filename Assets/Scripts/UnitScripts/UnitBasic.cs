using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBasic : MonoBehaviour
{
    
    // use to store the current grid positon of the unit
    private GridPosition currentGridPostion;

    // list of actions that unit has
    private BaseAction[] baseActionArray;

    // expose the moveaction linked to the unit
    private MoveAction moveAction;

    // expose the spin action script attached to the unit
    private SpinAction spinAction;

    private void Awake()
    {
        // find the moveaction attached to the unit
        moveAction = this.GetComponent<MoveAction>();
        // find the spinaction
        spinAction = this.GetComponent<SpinAction>();
        // store all the actions into the array
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        // find the grid postion of this unit and sign the unit to the grid object
        currentGridPostion = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(currentGridPostion, this);
    }


    private void Update()
    {
        
        // update the grid positon
        GridPostionUpdate();
    }



    // function used to update the unit gridPosition
    private void GridPostionUpdate()
    {
        // get the current position
        GridPosition newGridPos = LevelGrid.Instance.GetGridPosition(this.transform.position);
        // if the location is new, then update the grid info
        if (newGridPos != currentGridPostion)
        {
            // unit changed grid postion
            LevelGrid.Instance.UpdateUnitGridPosition(this, currentGridPostion, newGridPos);
            // updat the current grid positon
            currentGridPostion = newGridPos;
        }
    }


    // function used to expose the moveAction script
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }


    // function used to expose the spinaction
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }


    // functions used to expose the unit current grid position
    public GridPosition GetUnitCurrentGridPosition()
    {
        return currentGridPostion;
    }

    // function used to expose the baseActionArray
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    
}
