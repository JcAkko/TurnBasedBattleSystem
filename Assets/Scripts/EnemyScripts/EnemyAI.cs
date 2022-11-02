using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    // state machine for AI
    private enum State
    {
        waitingForEnemyPhase,
        TakingAction,
        Busy,
    }

    // current state
    private State currentState;

    // timer for all state
    private float stateTimer;

    // for time config
    private float timer;


    private void Awake()
    {
        // sign the default state as waiting
        currentState = State.waitingForEnemyPhase;
    }

    private void Start()
    {
        // subscribe to the on turnchange event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        // ***if the current turn is player turn, can do nothing
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }


        // if enemy turn, switch between different states
        // decrement the state timer
        stateTimer -= Time.deltaTime;

        // else, start state turn and start timer
        switch (currentState)
        {
            case State.waitingForEnemyPhase:
                break;
            case State.TakingAction:
                // test
                timer -= Time.deltaTime;
                // time up load the next turn
                if (timer < 0)
                {
                    
                    // try take the actual action if there is enemy unit exist
                    if(EnemyAITryTakeAction(SetStateTakingAction))
                    {
                        // set the state to busy
                        currentState = State.Busy;
                    }
                    else
                    {
                        // end turn if no more enemy can take anymore actions
                        TurnSystem.Instance.NextTurn();
                    }
                    
                  
                }
                break;
            case State.Busy:
                break;
        }

    }

    
    // function used to make AI take action
    // delegate called when the action complete, state set back to the taking action state
    private bool EnemyAITryTakeAction(Action onEnemyAIActionComplete_)
    {
        // loop through all the enemies and take action
        foreach (UnitBasic enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            // enemy unit take action in turn if have enough action point
            if (EnemyTryTakeAction(enemyUnit, onEnemyAIActionComplete_))
            {
                return true;
            }
            
        }
        
        return false;

    }

    // function for the selected unit to take action
    private bool EnemyTryTakeAction(UnitBasic enemyUnit_, Action onEnemyAIActionComplete_)
    {
        // test: get the spin action from enemy
        SpinAction spinAction = enemyUnit_.GetSpinAction();

        // convert the mouse world position into the grid postion
        GridPosition actionGridPosition = enemyUnit_.GetUnitCurrentGridPosition();

        // validate if the mouse grid positon is a valid position
        if (spinAction.IsThisGridValidMovePosition(actionGridPosition))
        {
            // test if the selected unit have enough action points to take the action
            if (enemyUnit_.TryUseAPToTakeAction(spinAction))
            {
                
                // execute the selected action
                spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete_);
                return true;

            }
            else
            {
                Debug.Log("Unit do not have enough action point to act");
                return false;
            }


        }


        return false;
    }


    // function called by delegate
    // reset the state back to taking action
    private void SetStateTakingAction()
    {
        timer = 0.5f;
        // set state
        currentState = State.TakingAction;

    }


    // reset timer
    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {
        // if the current state is the enemy turn, increment the timer
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            currentState = State.TakingAction;
            timer = 2.0f;
        }
        
    }

}
