using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        // subscribe and listen to the action busy event
        UnitActionSystem.Instance.OnActionBusyChange += UnitActionSystem_OnActionBusyChange;

        // hide the busy
        HideBusyUI();
    }

    // function to execute when listen to the action busy event and recevie the bool
    private void UnitActionSystem_OnActionBusyChange(object sender, bool isBusy_)
    {
        // update the UI
        if (isBusy_)
        {
            ShowBusyUI();
        }
        else
        {
            HideBusyUI();
        }
    }


    private void HideBusyUI()
    {
        this.gameObject.SetActive(false);
    }
    

    private void ShowBusyUI()
    {
        this.gameObject.SetActive(true);
    }
}
