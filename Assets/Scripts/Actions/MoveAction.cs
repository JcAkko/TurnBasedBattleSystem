using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{

    // list of vec3 that stores the path to the target positon of the unit
    private List<Vector3> positionList;

    // the current position index of the unit movement
    private int currentPositionIndex;

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


    // used to store the potentail target for enemy AI
    private UnitBasic potentialtargetUnit = null;

    // the potential enemy position that store into the AIGridObject
    private GridPosition LastEnemyGridPosition;



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
        // find the current target position
        Vector3 targetPosition = positionList[currentPositionIndex];

        // define the stopping distance
        float stoppingDistance = 0.05f;

        // calculate the vector from unit origin position to the target position
        Vector3 movingDirection = (targetPosition - this.transform.position).normalized;

        // always rotate the plaer towards the moving direction
        // use lerp for smooth transition
        this.transform.forward = Vector3.Lerp(this.transform.forward, movingDirection, Time.deltaTime * rotationSpeed);

        // only move the unit when out of stopping distance
        if (Vector3.Distance(targetPosition, this.transform.position) >= stoppingDistance)
        {

            OnUnitStartMoving?.Invoke(this, EventArgs.Empty);

            // move the unit towards the target positon
            this.transform.position += movingDirection * unitMovingSpeed * Time.deltaTime;

            /*
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
            */
            
        }
        else
        {
            // upon reach the target point, increment the index to move to the next position on list
            currentPositionIndex++;
            // check if reach the end of the list, if so, call action complete
            if (currentPositionIndex >= positionList.Count)
            {
                // stop the walking animation
                OnUnitStopMoving?.Invoke(this, EventArgs.Empty);

                // call delegate from base action and end the action
                ActionComplete();

            }
            
        }

    }


    // function is used to expose the movement logic
    // this function will be public in order for unitActionSytem to call
    public override void TakeAction(GridPosition gridPosition_, Action OnMovementComplete_)
    {
      
        // find the optimal path the reach the target position
        List<GridPosition> calculatedPathGridPositionList = PathFinding.Instance.FindPath(unit.GetUnitCurrentGridPosition(), gridPosition_, out int pathLength_);

        // reset the position index
        currentPositionIndex = 0;

        // populate the list to hold all the world positions of the list
        positionList = new List<Vector3>();

        // transfer all the grid positions into world position and add them into the positionList
        foreach (GridPosition gridPos_ in calculatedPathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(gridPos_));
        }
        
        // Set the action as active and start execute action
        ActionStart(OnMovementComplete_);

    }


    // this function is used to return moveable grids as a list
    // used for grid visuals
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

                // check if the grid position is walkable inside the pfGridSystem
                if (!PathFinding.Instance.IsWalkAblePosition(mergedGridPosition))
                {
                    continue;
                }

                // check if the grid position is reachable
                if (!PathFinding.Instance.IsThereAPathToReachThePosition(unitGridPosition, mergedGridPosition))
                {
                    continue;
                }

                // check if the gridpos is within the maxmovedistance but too far to reach by the path finding
                int pathFindingMultiplier = 10;
                if (PathFinding.Instance.GetPathLength(unitGridPosition, mergedGridPosition) > maxMoveDistance * pathFindingMultiplier)
                {
                    // the grid pos is too far to reach by the path finding due to the max move distance limit
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
        //int targetCount = unit.GetAction<AttackAction>().GetTargetCountAtThisPosition(gridPosition_);

        // find out if any player unit is within the move range
        List<UnitBasic> playerList = UnitManager.Instance.GetPlayerUnitList();

        
        // used to store the previous range
        int previousDistance = 0;
        // the index for action value
        int actionValueIndex = 0;

        foreach (UnitBasic playerUnit_ in playerList)
        {
            // calculate the diatance
            int distanceToPlayer = PathFinding.Instance.GetPathLength(unit.GetUnitCurrentGridPosition(), playerUnit_.GetUnitCurrentGridPosition());
         
            Debug.Log("DicToP: " + distanceToPlayer + "MovD: " + maxMoveDistance);
            // if the unit is within the move range
            if (distanceToPlayer <= maxMoveDistance * 10)
            {
                if (potentialtargetUnit == null)
                {
                    potentialtargetUnit = playerUnit_;
                    previousDistance = distanceToPlayer;
                }
                else
                {
                    // if found one that is closer
                    if (distanceToPlayer < previousDistance)
                    {
                        potentialtargetUnit = playerUnit_;
                        previousDistance = distanceToPlayer;
                    }
                }
                
            }
        }

        
        if (potentialtargetUnit != null)
        {
            actionValueIndex = 20;
            LastEnemyGridPosition = FindAttackPosition();
        }
        else
        {
            actionValueIndex = 0;
            LastEnemyGridPosition = gridPosition_;
        }




        // return the enemy AI action info
        return new EnemyAIAction
        {
            // construction
            //gridPostion = gridPosition_,
            gridPostion = LastEnemyGridPosition,
            // the action value of the move action depends on how many target around the specific grid
            // so eneny will tend move to the positon with more enemy
            //actionValue = targetCount * 10,
            actionValue = 0 + actionValueIndex,
        };
    }


    // function used to find the valid attack position
    private GridPosition FindAttackPosition()
    {
        // optimal gridpos
        GridPosition optimalGridPosition = new GridPosition();

        if (potentialtargetUnit != null)
        {
            // list to hold all the potential positions that can make enemy attack player
            List<GridPosition> targetPositions = new List<GridPosition>();

            

            // check the attack range
            int attackRange = unit.GetComponent<AttackAction>().GetAttackRange();

            // calculate the grid positions
            GridPosition Left = potentialtargetUnit.GetUnitCurrentGridPosition() + new GridPosition(-attackRange,0);
            GridPosition Right = potentialtargetUnit.GetUnitCurrentGridPosition() + new GridPosition(attackRange, 0);
            GridPosition Top = potentialtargetUnit.GetUnitCurrentGridPosition() + new GridPosition(0, attackRange);
            GridPosition Bot = potentialtargetUnit.GetUnitCurrentGridPosition() + new GridPosition(0, -attackRange);

            // add them to the list
            targetPositions.Add(Left);
            targetPositions.Add(Right);
            targetPositions.Add(Top);
            targetPositions.Add(Left);

            // remove the invalid ones
            foreach (GridPosition gridPos_ in targetPositions)
            {
                // if not valid, remove
                if (!LevelGrid.Instance.IsValidGridPosition(gridPos_))
                {
                    targetPositions.Remove(gridPos_);
                }
            }

            // float to hold the distance between player and enemy
            float distance = 0;

            // find the grid position that close the the target
            foreach (GridPosition gridPos_ in targetPositions)
            {
                // calculate the distance between enemy and the target pos
                float currentDistance = Mathf.Abs(Vector3.Distance(unit.GetUnitWorldPosition(), LevelGrid.Instance.GetWorldPosition(gridPos_)));
                
                if (distance == 0)
                {
                    distance = currentDistance;
                    optimalGridPosition = gridPos_;
                }
                else
                {
                    // repalce the distance if a closer positon has been found
                    if (currentDistance <= distance)
                    {
                        distance = currentDistance;
                        optimalGridPosition = gridPos_;
                    }
                }
            }

            
            return optimalGridPosition;
           

        }


        return potentialtargetUnit.GetUnitCurrentGridPosition();
        
    }

}
