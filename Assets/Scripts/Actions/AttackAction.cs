using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BaseAction
{
    // state machine that allow cam and animation play in sequence while taking action
    private enum State
    {
        Turning,
        Slashing,
        CoolOff,
    }

    // store current active state
    private State currentState;

    // timer for all state
    private float stateTimer;

    // the max movement distance for the unit
    [SerializeField]
    private int maxAttackDistance = 1;

    // the damage amount
    [SerializeField]
    private int damageAmount = 20;


    // the action cost
    [SerializeField]private int actionCost = 2;

    // used to store the target unit
    private UnitBasic targetUnit;

    // bool to check if can start attacking
    private bool canStartSlashing;


    // two events that handle to unit slash animation
    public event EventHandler OnUnitStartSlashing;

    // layer mask for the enemy
    [SerializeField] private LayerMask obstacleLayerMask;



    private void Update()
    {
        // is inactive, reture and do nothing
        if (!isActive)
        {
            return;
        }


        // decrement the state timer
        stateTimer -= Time.deltaTime;

        // else, start state turn and start timer
        switch (currentState)
        {
            case State.Turning:
                // calculate the enemy direction
                Vector3 targetDirection = (targetUnit.GetUnitWorldPosition() - unit.GetUnitWorldPosition()).normalized;
                // define rotation speed
                float rotationSpeed = 20.0f;
                // turn to the right direction first and then move
                if (this.transform.forward != targetDirection)
                {
                    // always rotate the plaer towards the moving direction
                    // use lerp for smooth transition
                    this.transform.forward = Vector3.Lerp(this.transform.forward, targetDirection, Time.deltaTime * rotationSpeed);
                }
                break;
            case State.Slashing:
                if (canStartSlashing)
                {
                    // attack logic here
                    Slash();
                    // set bool to false
                    canStartSlashing = false;
                }
                break;
            case State.CoolOff:
                break;
        }

        // switch to next state upon time up
        if (stateTimer <= 0)
        {
            NextState();
        }
      


    }


    // function used to switch state
    private void NextState()
    {
        // used to switch state
        switch (currentState)
        {
            case State.Turning:
                currentState = State.Slashing;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Slashing:
                currentState = State.CoolOff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.CoolOff:
                // call delegate from base action and end the action
                ActionComplete();
                break;
        }

        

    }



    public override string GetActionName()
    {
        return "Attack";
    }


    public override int GetActionPointsCost()
    {
       return actionCost;
    }



    public override List<GridPosition> GetValidGridPositionList()
    {
        // get the current unit grid position
        GridPosition unitGridPosition = unit.GetUnitCurrentGridPosition();
        // return the list with target position
        return GetValidGridPositionList(unitGridPosition);

    }


    // logic to loop through all the grid positions aroud the given gridPos's attack range and try find the attackable unit
    public List<GridPosition> GetValidGridPositionList(GridPosition unitGridPosition_)
    {
        // create a list
        List<GridPosition> validGridPositions = new List<GridPosition>();

        

        // start search for all the grids around the unit, from left to right, bottom to top
        for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
        {
            for (int z = -maxAttackDistance; z <= maxAttackDistance; z++)
            {
                // get the offset
                GridPosition ValidPositionOffSet = new GridPosition(x, z);
                // add it to the current unit position
                GridPosition mergedGridPosition = unitGridPosition_ + ValidPositionOffSet;
                // check if the merged grid position is valid (inside the map range)
                if (!LevelGrid.Instance.IsValidGridPosition(mergedGridPosition))
                {
                    // if the position is not valid, loop the next one
                    continue;
                }


                /*
                // calculate the shooting range in a circle shape
                // used for long range attack only
                int ShootRange = Mathf.Abs(x) + Mathf.Abs(z);
                if (ShootRange > maxAttackDistance)
                {
                    continue;
                }
                */

                // if no unit is standing inside the range, continue
                if (!LevelGrid.Instance.IsGridPostionOccupied(mergedGridPosition))
                {
                    // grid positon has no attack target, continue
                    continue;
                }


                // try get the target unit at the positon
                UnitBasic targetUnit = LevelGrid.Instance.GetUnitOnGrid(mergedGridPosition);

                // check if the targetUnit is in same team, is so do nothing
                if (targetUnit.IsUnitEnemy() == unit.IsUnitEnemy())
                {
                    continue;
                }

                // check if there is not obstacle between unit and the attack target
                Vector3 unitWorldPos = LevelGrid.Instance.GetWorldPosition(unitGridPosition_);
                Vector3 attackDir = (targetUnit.GetUnitWorldPosition() - unitWorldPos).normalized;

                // the world position is at the bottom of the unit, so need to add shoulder height before ray cast
                float unitShoulderHeight = 1.7f;
                // ray cast to see if theres obstacle
                if (Physics.Raycast(
                    unitWorldPos + Vector3.up * unitShoulderHeight,
                    attackDir,
                    Vector3.Distance(unitWorldPos, targetUnit.GetUnitWorldPosition()),
                    obstacleLayerMask
                    ))
                {
                    // blocked by obstacle
                    continue;
                }

                

                // if the grid passed all the check, add it into the valid grid list
                validGridPositions.Add(mergedGridPosition);

               

            }

        }


        return validGridPositions;
    }


    // used to attack other unit
    // take an delegate function to call when function ends
    public override void TakeAction(GridPosition gridPosition_, Action onAttackActionComplete_)
    {

        // get the target unit from the grid position
        targetUnit = LevelGrid.Instance.GetUnitOnGrid(gridPosition_);

        // set initial state and timer
        currentState = State.Turning;
        float turningStateTime = 1.0f;
        stateTimer = turningStateTime;

        // can start attacking
        canStartSlashing = true;

        // start the slashing animation
        OnUnitStartSlashing?.Invoke(this, EventArgs.Empty);

        // set the action as active and start execute action
        ActionStart(onAttackActionComplete_);
    }

    // slash logic
    private void Slash()
    {
        
        // damage to the target
        targetUnit.TakeDamage(damageAmount);

    }


    // function used to expose the target unit
    public UnitBasic GetTargetUnit()
    {
        return targetUnit;
    }

    // function used to expose the attac range
    public int GetAttackRange()
    {
        return maxAttackDistance;
    }


    // sign the enemy AI Action info for the AI usage
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition_)
    {
        // the rank of an potential eney depends on how much hp target left
        UnitBasic targetUnit = LevelGrid.Instance.GetUnitOnGrid(gridPosition_);

        // return the spin action AI action info
        return new EnemyAIAction
        {
            // construction
            gridPostion = gridPosition_,
            // attack has the highest action value 
            actionValue = 100 + Mathf.RoundToInt((1- targetUnit.GetCurrentHealth()) * 100f),
        };
    }


    // function used to return all the attackable targets
    public int GetTargetCountAtThisPosition(GridPosition gridPos_)
    {
        
        // find all the valid targets at the current grid pos
        // the length of the list is the enemy count
        return GetValidGridPositionList(gridPos_).Count;


    }

}
