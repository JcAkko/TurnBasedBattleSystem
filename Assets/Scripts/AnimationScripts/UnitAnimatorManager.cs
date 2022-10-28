using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimatorManager : MonoBehaviour
{
    // unit animator
    [SerializeField]
    private Animator unitAnimator;


    private void Update()
    {
        // try get the move action of the unit
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            // if there is a move action on unit, subscirbe to the onstart and onstop event
            moveAction.OnUnitStartMoving += moveAction_OnUnitStartMoving;
            moveAction.OnUnitStopMoving += moveAction_OnUnitStopMoving;
        }


        // try get the slash/attak action of the unit
        if (TryGetComponent<AttackAction>(out AttackAction attackAction))
        {
            // if there is a attack action on unit, subscirbe to the onstart event
            attackAction.OnUnitStartSlashing += attackAction_OnUnitStartSlashing;
            // no stop event because use trigger in the animator
            //attackAction.OnUnitStopSlashing += attackAction_OnUnitStopSlashing;
        }

    }


    // on start moving play animation
    private void moveAction_OnUnitStartMoving(object sender, EventArgs empty)
    {
        if (unitAnimator.GetBool("isWalking") != true)
        {
            unitAnimator.SetBool("isWalking", true);
        }
        
    }


    // on stop moving stop playing animation
    private void moveAction_OnUnitStopMoving(object sender, EventArgs empty)
    {
        unitAnimator.SetBool("isWalking", false);
    }


    // on start play attack animation
    private void attackAction_OnUnitStartSlashing(object sender, EventArgs empty)
    {
        unitAnimator.SetTrigger("Slash");

    }





}
