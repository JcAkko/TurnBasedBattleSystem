using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // for test
    private float timer;


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


        // test
        timer -= Time.deltaTime;
        // time up load the next turn
        if (timer < 0)
        {
            TurnSystem.Instance.NextTurn();
        }



    }


    // reset timer
    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {
        timer = 2.0f;
    }

}
