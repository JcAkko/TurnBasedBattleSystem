using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    // make turn system singleton
    public static TurnSystem Instance { get; private set; }

    // track the current turn number
    private int turnNumber = 1;

    // is the current turn player turn
    private bool isPlayerTurn = true;

    // timer for player turn cool down
    private float PlayerCoolDownTimer;

    // event called when turn number changed
    public event EventHandler OnTurnChanged;

    // event called when enemy count finished
    public event EventHandler OnEnemyCountDownFinished;

    private void Awake()
    {
        // check if there's multiple instance for this class, if so destory them and only leave one exist
        if (Instance != null)
        {
            Destroy(this.gameObject);
            // exit the function so no more new instance created
            return;
        }

        // if not instance yet, sign the instance
        Instance = this;
    }



    private void Update()
    {
        
        if (isPlayerTurn == false && PlayerCoolDownTimer > 0)
        {
            PlayerCoolDownTimer -= Time.deltaTime;
        }
        else if (isPlayerTurn == false)
        {
            isPlayerTurn = true;
            // fire the on turn change event
            OnTurnChanged?.Invoke(this, EventArgs.Empty);
        }
        
    }

    // function used to expose turn number
    public int GetTurnNumber()
    {
        return turnNumber;
    }



    // function used to call next turn
    public void NextTurn()
    {
        turnNumber++;
        // switch between player and enemy turn
        isPlayerTurn = !isPlayerTurn;
        // fire the on turn change event
        OnTurnChanged?.Invoke(this, EventArgs.Empty);

        // if player turn ends, reset timer
        if (isPlayerTurn == false)
        {
            Debug.Log("Player cool down start");
            PlayerCoolDownTimer = 5.0f;
        }
    }


    // function used to expose if the current turn is player turn
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }


    // function used to reset the enemy action points upon count down finished
    public void EnemyCountDownFinished()
    {
        OnEnemyCountDownFinished?.Invoke(this, EventArgs.Empty);
    }
}
