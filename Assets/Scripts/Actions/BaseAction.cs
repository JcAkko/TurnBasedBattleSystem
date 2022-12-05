using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// this is the base for all the action scripts
// this class is abstract to prevent any instance made from this class by accident
public abstract class BaseAction : MonoBehaviour
{

    // bool used to check if this action is active
    // protected so only childs can reach these variables
    protected bool isActive = false;

    // refer to the unitBasic
    protected UnitBasic unit;


    // delegate used to represent a function that might be called when an action finished
    // use using System and call Action<> instead of creat one 
    // Action<> returns void while Func<> returns the type inside <>
    protected Action onActionComplete;


    // static event called on every action to help other system detected if action start or complete
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler onAnyActionCompleted;



    protected virtual void Awake()
    {
        // get the unitBasic script upon awake
        unit = this.GetComponent<UnitBasic>();
    }


    // abstract that will force all the child actions to define this function
    public abstract string GetActionName();


    // action that unit execute 
    // action required the unit current positon as well as the function to execute when acion finished
    public abstract void TakeAction(GridPosition gridPosition_, Action onActionComplete_);


    // function used to test if one gridpostion is a valid movepostion for the unit
    public virtual bool IsThisGridValidMovePosition(GridPosition gridPosition_)
    {
        // get the current valid position list
        List<GridPosition> validGridPositions = GetValidGridPositionList();
        // test if the list contains the position
        return validGridPositions.Contains(gridPosition_);
    }


    // this function is used to return moveable grids as a list
    public abstract List<GridPosition> GetValidGridPositionList();


    // function used to expose the action cost
    public virtual int GetActionPointsCost()
    {
        // default is 1
        return 1;
    }


    // common funciton that all child should apply upon action start
    // inside each action, call this function after everything are all setup to avoid potential issues
    protected void ActionStart(Action onActionComplete_)
    {
        isActive = true;
        // sign the delegate and run it when action complete
        this.onActionComplete = onActionComplete_;

        // fire off the action start event
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);

    }


    // common funciton that all child should apply upon action complete
    // 
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        // fire off the action complete event
        onAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }


    // expose the unit
    public UnitBasic GetUnit()
    {
        return unit;
    }


    // every action have to define this function to sign its own AI action value and grid position
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition_);


    // this function returns the best AI action on list based on the AI action value
    public EnemyAIAction GetBestAIAction()
    {
        // list to hold all the AIActions
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        // list that holds all the valid gridpositions for this action
        List<GridPosition> validActionGridPositionList = GetValidGridPositionList();

        // loop through all the valid grid positions and compare their action value
        // there will be an enemyAiAction object on each of the gridPosition
        foreach (GridPosition validGridPos_ in validActionGridPositionList)
        {
            // check the action Value for each grid
            EnemyAIAction enemyAIAction = GetEnemyAIAction(validGridPos_);
            // add it into the list
            enemyAIActionList.Add(enemyAIAction);
        }

        // if the list is not empty
        if (enemyAIActionList.Count > 0)
        {
            // sort the list
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            // return the highest action point
            return enemyAIActionList[0];

        }
        else
        {
            return null;
        }

        

    }



}
