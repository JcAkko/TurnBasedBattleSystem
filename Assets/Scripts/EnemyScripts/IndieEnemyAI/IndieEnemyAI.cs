using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script is similar to the EnemyAI script but for individual control
public class IndieEnemyAI : MonoBehaviour
{

    // refers to the enemy unitBasic script
    UnitBasic enemyUnit;

    // refer to the wait time between truns of this AI unit
    [SerializeField] float MaxGapTime;

    // the timer to count gap time
    private float gapTimer = 0;

    // refer to the character Tag UI
    private GameObject characterTagUI;

    // the end point of the slider
    [SerializeField] float sliderEndPoint;



    // state machine for AI
    private enum State
    {
        // idle state
        waitingForEnemyPhase,
        // enemy take action one at a time, after each action check if can take more
        // if not, switch to busy state
        TakingAction,
        // only switch to this state if not more action can take
        Busy,
    }

    // used to store the current state
    private State currentState;

    // timer for all state
    //private float stateTimer;

    // for time gao between each Action
    private float timer;


    // variables used for unit timeline Tag
    private float YPos;
    private float XStart;
    private float XEnd;
    private Vector3 startPos;
    private Vector3 endPos;



    private void Awake()
    {
        // sign the default state as waiting
        currentState = State.waitingForEnemyPhase;

        // sign the unit to self
        enemyUnit = this.GetComponent<UnitBasic>();
    }

    private void Start()
    {
        // subscribe to the on turnchange event
        //TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        // get the positions used for update the character Tag UI
        YPos = TimelineUI.Instance.GetSliderYValue();
        XStart = TimelineUI.Instance.GetMinSliderValue();
        XEnd = sliderEndPoint;
        // calcualte the start and end position of the unit Tag UI
        startPos = new Vector3(XStart, YPos, 0);
        endPos = new Vector3(XEnd, YPos, 0);
    }

    private void Update()
    {
        /*
        // ***if the current turn is player turn, do nothing
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        */


        // update chracter tag UI
        UpdateCharacterTag();


        if (Input.GetKeyDown(KeyCode.H))
        {
            // enemy turn start
            currentState = State.TakingAction;
            // wait time between each action
            timer = 2.0f;
            // reset the gap timer to max
            gapTimer = MaxGapTime;
        }

        // if enemy turn, switch between different states
        // decrement the state timer
        //stateTimer -= Time.deltaTime;

        // else, start state turn and start timer
        switch (currentState)
        {
            case State.waitingForEnemyPhase:
                if (gapTimer > 0)
                {

                    gapTimer -= Time.deltaTime;
                    //Debug.Log("Enemy Cool down" + gapTimer);
                }
                else
                {
                    Debug.Log("Enemy start action");
                    // reset the action points
                    TurnSystem.Instance.EnemyCountDownFinished();
                    // enemy turn start
                    currentState = State.TakingAction;
                    timer = 2.0f;

                }
                break;
            case State.TakingAction:
                // test
                timer -= Time.deltaTime;
                // time up load the next turn
                if (timer < 0)
                {

                    // try take action if there is an enemy unit exist in game
                    // after function compelet, reset the state back to TakingAction if enemies still can take action
                    if (TryFindEnemyToTakeAction(SetStateTakingAction))
                    {
                        // only come to this point if all enemy units used their action points
                        // set the state to busy
                        currentState = State.Busy;
                    }
                    else
                    {
                        // end turn if no more enemy can take anymore actions
                        //TurnSystem.Instance.NextTurn();
                        // reset gap timer to max
                        gapTimer = MaxGapTime;
                        // *** reset the state back to waiting
                        currentState = State.waitingForEnemyPhase;
                       
                    }


                }
                break;
            case State.Busy:
                break;
        }

    }


    // function used to make AI take action
    // delegate called when the action complete, state set back to the taking action state
    private bool TryFindEnemyToTakeAction(Action onEnemyAIActionComplete_)
    {
        // enemy unit take action in turn if have enough action point
        if (EnemyTryTakeAction(enemyUnit, onEnemyAIActionComplete_))
        {
            return true;
        }

        return false;

    }

    // function for the selected unit to take action
    private bool EnemyTryTakeAction(UnitBasic enemyUnit_, Action onEnemyAIActionComplete_)
    {
        // temp used to track which is the best action to take
        EnemyAIAction currentBestAIAction = null;

        // the best action
        BaseAction bestAction = null;

        // loop through all the actions that this enemy has and find the best action to take
        foreach (BaseAction baseAction_ in enemyUnit_.GetBaseActionArray())
        {
            // check if enemy have enough action point to take action
            // if so, spend the cost
            if (!enemyUnit_.TryUseAPToTakeAction(baseAction_))
            {
                // do not have enough points
                continue;
            }

            if (currentBestAIAction == null)
            {
                currentBestAIAction = baseAction_.GetBestAIAction();
                bestAction = baseAction_;
            }
            else
            {
                // if already have the best one, compare, if the new one if better, replace
                EnemyAIAction newAIAction = baseAction_.GetBestAIAction();
                if (newAIAction != null && newAIAction.actionValue > currentBestAIAction.actionValue)
                {
                    // replace
                    currentBestAIAction = newAIAction;
                    bestAction = baseAction_;
                }
            }
        }


        // double check again if there is a best action and unit can take the action
        if (currentBestAIAction != null && enemyUnit_.TryUseAPToTakeAction(bestAction))
        {
            // take action
            bestAction.TakeAction(currentBestAIAction.gridPostion, onEnemyAIActionComplete_);
            // spend the cost
            enemyUnit_.SpendCost(bestAction);
            Debug.Log("Enemy take action: " + bestAction.GetActionName());
            return true;
        }
        else
        {
            Debug.Log("Unit do not have enough action point to act");
            return false;
        }


      
    }


    // function called by delegate
    // reset the state back to taking action
    private void SetStateTakingAction()
    {
        // reset the takingAction timer
        timer = 2.0f;
        // set state
        currentState = State.TakingAction;

    }


    // reset timer
    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {
        // if the current state is the enemy turn, increment the timer
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            // enemy turn start
            currentState = State.TakingAction;
            timer = 2.0f;
        }

    }


    // function used to set the character UI
    public void SetCharacterTagUI(GameObject TagUI)
    {
        characterTagUI = TagUI;
    }


    // function used to update the CharacterTag if there is one
    private void UpdateCharacterTag()
    {
        if (characterTagUI == null)
        {
            return;
        }

        //calculate the position ratio
        float ratio = gapTimer / MaxGapTime;

        // update the slider
        characterTagUI.transform.localPosition = Vector3.Lerp(endPos, startPos, ratio);
    }


}
