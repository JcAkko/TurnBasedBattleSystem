using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
   
    // used to store spin amount
    private float totalSpinAmount;


    private void Update()
    {
        // is inactive, reture and do nothing
        if (!isActive)
        {
            return;
        }


        // else, spin unit
        spinUnit();
            
            
    }

    // function used to spin the unit
    private void spinUnit()
    {
        // track how much unit spined
        float spinAmount = 360f * Time.deltaTime;
        // add it to the unit rotation
        this.transform.eulerAngles += new Vector3(0, spinAmount, 0);
        // add it to the total spin count
        totalSpinAmount += spinAmount;
        // if exceed 360, stop spin
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
        }

    }


    // used to call spin from outside
    public void SpinUnit()
    {
        // reset the spin amount
        totalSpinAmount = 0;
        // start spin
        isActive = true;
       
    }
}
