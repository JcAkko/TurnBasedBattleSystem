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


    // refer to the unit animator
    [SerializeField]
    private Animator unitAnimator;

    // the max movement distance for the unit
    [SerializeField]
    private int maxMoveDistance = 3;


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
        float stoppingDistance = 0.01f;

        // calculate the vector from unit origin position to the target position
        Vector3 movingDirection = (targetPosition - this.transform.position).normalized;

        // only move the unit when out of stopping distance
        if (Vector3.Distance(targetPosition, this.transform.position) >= stoppingDistance)
        {
            
            // move the unit towards the target positon
            this.transform.position += movingDirection * unitMovingSpeed * Time.deltaTime;
            
            // play the walking animation
            unitAnimator.SetBool("isWalking", true);
        }
        else
        {
            // stop the walking animation
            unitAnimator.SetBool("isWalking", false);
            // set the action to disactive
            isActive = false;
            // call delegate 
            onActionComplete();
        }

        // always rotate the plaer towards the moving direction
        // use lerp for smooth transition
        this.transform.forward = Vector3.Lerp(this.transform.forward, movingDirection, Time.deltaTime * rotationSpeed);


    }


    // function is used to expose the movement logic
    // this function will be public in order for unitActionSytem to call
    public override void TakeAction(GridPosition gridPosition_, Action OnMovementComplete_)
    {
        // sign the delegate
        this.onActionComplete = OnMovementComplete_;
        // update the target position
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition_);
        // set the action as active
        isActive = true;
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

}
