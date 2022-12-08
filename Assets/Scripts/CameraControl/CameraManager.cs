using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    // action cam
    [SerializeField] private GameObject ActionCam;


    private void Start()
    {
        // subscribe to the action start event and end event
        //BaseAction.OnAnyActionStarted += baseAction_OnAnyActionStarted;
        //BaseAction.onAnyActionCompleted += baseAction_OnAnyActionCompleted;

        // hide the action cam on start
        SetActionCamInactive();
    }


    // function called when any action fired
    private void baseAction_OnAnyActionStarted(object sender, EventArgs empty)
    {
        // different cinemative event depends on the action type
        switch (sender)
        {
            case AttackAction attackAction:

                // get the attacker and the target
                UnitBasic attacker_ = attackAction.GetUnit();
                UnitBasic target_ = attackAction.GetTargetUnit();

                // the height off set of the cam
                Vector3 cameraHeightOffSet = Vector3.up * 1.4f;
                // the direction where cam face
                Vector3 attackDirection = (target_.GetUnitWorldPosition() - attacker_.GetUnitWorldPosition()).normalized;
                // the offset amount
                float attachOffSetAmount = 2.0f;
                // calculate the cam offset
                Vector3 finalOffSet = Quaternion.Euler(0, -90, 0) * attackDirection * attachOffSetAmount;

                // cam z offset
                Vector3 zOffSet = new Vector3(0,0,1f);

                // look at x offset
                Vector3 xOffSet = attacker_.transform.right;

                Vector3 actionCamPosition =
                    attacker_.GetUnitWorldPosition() +
                    cameraHeightOffSet +
                    zOffSet +
                    finalOffSet +
                    (attackDirection * -2.5f);

                // apply the positon to cam
                ActionCam.transform.position = actionCamPosition;
                ActionCam.transform.LookAt(target_.GetUnitWorldPosition() + cameraHeightOffSet + xOffSet);
                //ActionCam.transform.LookAt(attacker_.GetUnitWorldPosition() + cameraHeightOffSet);
                // set action cam to active
                SetActionCamActive();
                break;
        }
        
    }


    // function called when any action completed
    private void baseAction_OnAnyActionCompleted(object sender, EventArgs empty)
    {
        // different cinemative event depends on the action type
        switch (sender)
        {
            case AttackAction attackAction:
                // set action cam to inactive
                SetActionCamInactive();
                break;
        }
    }

  


    // function to set it active
    private void SetActionCamActive()
    {
        ActionCam.SetActive(true);
    }


    // function to deactive
    private void SetActionCamInactive()
    {
        ActionCam.SetActive(false);
    }


}
