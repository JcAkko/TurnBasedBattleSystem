using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBasic : MonoBehaviour
{
    // used to identify if the unit is an enemy or player unit
    [SerializeField]
    private bool isEnemy;
    
    // use to store the current grid positon of the unit
    private GridPosition currentGridPostion;

    // list of actions that unit has
    private BaseAction[] baseActionArray;

    // expose the moveaction linked to the unit
    private MoveAction moveAction;

    // expose the spin action script attached to the unit
    private SpinAction spinAction;

    // the max action Points for a unit
    [SerializeField] private int maxActionPoint = 2;

    // dynamic action points for the unit
    private int actionPoints = 2;


    // static event that fired on every point change
    public static event EventHandler OnAnyActionPointChange;

    private void Awake()
    {
        // find the moveaction attached to the unit
        moveAction = this.GetComponent<MoveAction>();
        // find the spinaction
        spinAction = this.GetComponent<SpinAction>();
        // store all the actions into the array
        baseActionArray = GetComponents<BaseAction>();
        // update the action point
        actionPoints = maxActionPoint;
    }

    private void Start()
    {
        // find the grid postion of this unit and sign the unit to the grid object
        currentGridPostion = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(currentGridPostion, this);

        // listen to the turn change event to refresh the action point
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        

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


    // get the unit world position
    public Vector3 GetUnitWorldPosition()
    {
        return transform.position;
    }

    // function used to expose the baseActionArray
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }


    // function used to expose the action points
    public int GetActionPoints()
    {
        return actionPoints;
    }

    // function used to test if unit still have action point
    public bool DoUnitHaveEnoughActionPoints(BaseAction action_)
    {
        // return true if have enough to take the action
        return actionPoints >= action_.GetActionPointsCost();
        
    }


    // function used to use the action points upon spend
    private void SpendActionPoint(int points_)
    {
        actionPoints -= points_;

        // just incase
        if (actionPoints < 0)
        {
            actionPoints = 0;
        }

        // on points change
        OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);
    }


    // function that try spend the action point if have enought ap to spend
    public bool TryUseAPToTakeAction(BaseAction action_)
    {
        if (DoUnitHaveEnoughActionPoints(action_))
        {
            // spend the action points
            SpendActionPoint(action_.GetActionPointsCost());
            // true
            return true;
        }
        else
        {
            return false;
        }
    }

    // recharge the action point upon turn changed
    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {
        // only recharge the AP if: unit is enemy and it is the enemy turn or unit is player and its the unit turn
        if ((IsUnitEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsUnitEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            // set the action point back to max
            actionPoints = maxActionPoint;
            // on points change
            OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);

        }
        
    }


    // function used to expose if the unit is enemy
    public bool IsUnitEnemy()
    {
        return isEnemy;
    }


    // take damage from attack
    public void TakeDamage()
    {
        Debug.Log(this.transform + " receive damage");
    }


}
