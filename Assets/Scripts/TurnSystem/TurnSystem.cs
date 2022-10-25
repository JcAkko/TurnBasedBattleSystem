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

    // event called when turn number changed
    public event EventHandler OnTurnChanged;

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

    // function used to expose turn number
    public int GetTurnNumber()
    {
        return turnNumber;
    }



    // function used to call next turn
    public void NextTurn()
    {
        turnNumber++;
        // fire the on turn change event
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }
}