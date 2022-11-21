using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurnTimer : MonoBehaviour
{
    // *** check if its trun to act
    private bool isMyTurn;

    // the max wait time
    [SerializeField] private float maxTurnTimer;

    // the current timer
    private float turnTimer = 0;


    private void Update()
    {
        // used for double unit system timer
        if (isMyTurn == false && turnTimer > 0.2f)
        {
            turnTimer -= Time.deltaTime;
        }
        else if (isMyTurn == false)
        {
            isMyTurn = true;
            // recharge the aciton point
            this.GetComponent<UnitBasic>().RechargeUnitActionPoint();
            // refresh the UI
            TurnSystem.Instance.IndieUnitCountDownFinished();
        }
    }

    // ***function used to return if ite the unit turn or not
    public bool IsMyTurn()
    {
        return isMyTurn;
    }


    // *** function used to switch isMyTurn
    public void SwithIsMyTurnState()
    {
        isMyTurn = !isMyTurn;
    }

    // function used to reset the turnTimer back to max
    public void ResetTurnTimer()
    {
        turnTimer = maxTurnTimer;
    }

}
