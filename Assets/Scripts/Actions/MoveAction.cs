using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    // vec3 that stores the target positon of the unit
    private Vector3 targetPosition;

    // unit moving speed
    [SerializeField]
    private float unitMovingSpeed = 4;
    // rotation speed of the unit
    [SerializeField]
    private float rotationSpeed = 10.0f;


    // the max movement distance for the unit
    [SerializeField]
    private int maxMoveDistance = 3;

    // the action cost of the action
    [SerializeField]
    private int actionCost = 2;


    // two events that handle to unit walking animation upon start and stop moving
    public event EventHandler OnUnitStartMoving;
    public event EventHandler OnUnitStopMoving;



    protected override void Awake()
    {
        // call the awake function from the baseAction script to locate the unitBasic script from the unit
        base.Awake();
        // set the initial target postion as the current unit position
        targetPosition = this.transform.position;
    }


    private void Update()
    {
        // only move the unit if action is active
        if (isActive)
        {
            // move the unit if targetpos updated
            unitMovement();
        }
        

    }


    // function that moves the unit
    private void unitMovement()
    {
        // define the stopping distance
        float stoppingDistance = 0.05f;

        // calculate the vector from unit origin position to the target position
        Vector3 movingDirection = (targetPosition - this.transform.position).normalized;

        // only move the unit when out of stopping distance
        if (Vector3.Distance(targetPosition, this.transform.position) >= stoppingDistance)
        {
            // turn to the right direction first and then move
            if (this.transform.forward != movingDirection)
            {
                // always rotate the plaer towards the moving direction
                // use lerp for smooth transition
                this.transform.forward = Vector3.Lerp(this.transform.forward, movingDirection, Time.deltaTime * rotationSpeed);

            }
            else
            {
                // start playing animation
                OnUnitStartMoving?.Invoke(this, EventArgs.Empty);

                // move the unit towards the target positon
                this.transform.position += movingDirection * unitMovingSpeed * Time.deltaTime;

            }
            
            
        }
        else
        {
            // stop the walking animation
            OnUnitStopMoving?.Invoke(this, EventArgs.Empty);
            
            // call delegate from base action and end the action
            ActionComplete();
        }

        // use lerp for smooth transition
        // use the code here if want move and rotation happend at the same time
        //this.transform.forward = Vector3.Lerp(this.transform.forward, movingDirection, Time.deltaTime * rotationSpeed);


    }


    // function is used to expose the movement logic
    // this function will be public in order for unitActionSytem to call
    public override void TakeAction(GridPosition gridPosition_, Action OnMovementComplete_)
    {
        
        // update the target position
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition_);

        // Set the action as active and start execute action
        ActionStart(OnMovementComplete_);

    }


    // this function is used to return moveable grids as a list
    public override List<GridPosition> GetValidGridPositionList()
    {
        // create a list
        List<GridPosition> validGridPositions = new List<GridPosition>();

        // get the current unit grid position
        GridPosition unitGridPosition = unit.GetUnitCurrentGridPosition();

        // start search for all the grids around the unit, from left to right, bottom to top
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                // get the offset
                GridPosition ValidPositionOffSet = new GridPosition(x, z);
                // add it to the current unit position
                GridPosition mergedGridPosition = unitGridPosition + ValidPositionOffSet;
                // check if the merged grid position is valid (inside the map range)
                if (!LevelGrid.Instance.IsValidGridPosition(mergedGridPosition))
                {
                    // if the position is not valid, loop the next one
                    continue;
                }

                // also the new position should not be the same as where the unit standing on
                if (mergedGridPosition == unitGridPosition)
                {
                    continue;
                }

                // if a position is already occupied by other unit, continue
                if (LevelGrid.Instance.IsGridPostionOccupied(mergedGridPosition))
                {
                    continue;
                }

                // if the grid passed all the check, add it into the valid grid list
                validGridPositions.Add(mergedGridPosition);
                
                // test
                //Debug.Log(mergedGridPosition);

            }

        }


        return validGridPositions;
    }



    // define the getaction name function
    public override string GetActionName()
    {
        return "Move";
    }


    // how much does this action cost
    public override int GetActionPointsCost()
    {
        return actionCost;
    }


    // sign the enemy AI Action info for the AI usage
    // enemy should move towards the player
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition_)
    {

        // find out if there are any valid target around the unit
        int targetCount = unit.GetAttackAction().GetTargetCountAtThisPosition(gridPosition_);

        // return the spin action AI action info
        return new EnemyAIAction
        {
            // construction
            gridPostion = gridPosition_,
            // the action value of the move action depends on how many target around the specific grid
            // so eneny will tend move to the positon with more enemy
            actionValue = targetCount * 10,
        };
    }

}
