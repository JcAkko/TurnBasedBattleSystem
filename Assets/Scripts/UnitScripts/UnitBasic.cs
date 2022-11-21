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

    /*
    // expose the moveaction linked to the unit
    private MoveAction moveAction;

    // expose the spin action script attached to the unit
    private SpinAction spinAction;

    // expose the shoot action script attached to the unit
    private AttackAction attackAction;
    */

    // get the health system from the unit
    private HealthSystem healthSystem;

    // the max action Points for a unit
    [SerializeField] private int maxActionPoint = 10;

    // dynamic action points for the unit
    private int actionPoints = 10;


    // static event that fired on every point change
    public static event EventHandler OnAnyActionPointChange;
    // two event called when unit spawned or dead
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private void Awake()
    {
        /*
        // find the moveaction attached to the unit
        moveAction = this.GetComponent<MoveAction>();
        // find the spinaction
        spinAction = this.GetComponent<SpinAction>();
        // find the attackAction
        attackAction = this.GetComponent<AttackAction>();
        */
        // sign the health system
        healthSystem = this.GetComponent<HealthSystem>();
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

        // *** listen to the turn system on enemy count down finished
        TurnSystem.Instance.OnEnemyCountDownFinished += TurnSystem_OnEnmyCountDownFinished;

        // subscribe to the health system on unit death
        healthSystem.onUnitDeath += healthSystem_OnUnitDeath;

        // call unit spawned upon start
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

    }


    private void Update()
    {
        
        // update the grid positon
        GridPostionUpdate();
    }


    // function used to get all kinds of action from the unit
    // used generic to receive the action type and constrain the type as the baseAction extensions
    public T GetAction<T>() where T: BaseAction
    {
        // loop through all the baseActions attached to this unit
        foreach (BaseAction baseAction_ in baseActionArray)
        {
            // if the action is the T type, return it
            if (baseAction_ is T)
            {
                return (T)baseAction_;
            }
        }

        // else return null
        return null;
    }



    // function used to update the unit gridPosition
    private void GridPostionUpdate()
    {
        // get the current position
        GridPosition newGridPos = LevelGrid.Instance.GetGridPosition(this.transform.position);
        // if the location is new, then update the grid info
        if (newGridPos != currentGridPostion)
        {
            // recrod the old positon
            GridPosition oldGridPos = currentGridPostion;
            
            // updat the current grid positon
            currentGridPostion = newGridPos;

            // unit changed grid postion
            LevelGrid.Instance.UpdateUnitGridPosition(this, oldGridPos, newGridPos);
        }
    }

    /*
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


    // expose the attack action
    public AttackAction GetAttackAction()
    {
        return attackAction;
    }
    */

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
            
            // true
            return true;
        }
        else
        {
            return false;
        }
    }


    // function used to spend the cost for taking an action
    public void SpendCost(BaseAction action_)
    {
        SpendActionPoint(action_.GetActionPointsCost());
    }

    // recharge the action point upon turn changed
    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {
        /*
        // only recharge the AP if: unit is enemy and it is the enemy turn or unit is player and its the unit turn
        if ((IsUnitEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsUnitEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            // set the action point back to max
            actionPoints = maxActionPoint;
            // on points change
            OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);

        }
        */

        // only recharge the AP if: unit is player and its the unit turn
        if ((!IsUnitEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            // set the action point back to max
            actionPoints = maxActionPoint;
            // on points change
            OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);

        }

    }


    // **** used for individual timeline system, recahrge the action ponints for the enemy
    private void TurnSystem_OnEnmyCountDownFinished(object sender, EventArgs empt)
    {
        if (IsUnitEnemy() == true)
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
    public void TakeDamage(int damageAmount_)
    {
        healthSystem.TakeDamage(damageAmount_);
    }

    // return the current health
    public float GetCurrentHealth()
    {
        return healthSystem.GetHealthPercentage();
    }


    // action takes upon unit death
    private void healthSystem_OnUnitDeath(object sender, EventArgs empty)
    {
        // remove the unit from the grid
        LevelGrid.Instance.RemoveUnitAtGridPosition(currentGridPostion, this);
        // call unit death event
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        // destory object for now
        Destroy(this.gameObject);
        
    }


    // *** used to recharge the action point for the individual unit
    public void RechargeUnitActionPoint()
    {
        // set the action point back to max
        actionPoints = maxActionPoint;
        // on points change
        OnAnyActionPointChange?.Invoke(this, EventArgs.Empty);
    }


   
}
