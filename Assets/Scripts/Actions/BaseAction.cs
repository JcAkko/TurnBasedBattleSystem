using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// this is the base for all the action scripts
// this class is abstract to prevent any instance made from this class by accident
public abstract class BaseAction : MonoBehaviour
{

    // bool used to check if this action is active
    // protected so only childs can reach these variables
    protected bool isActive = false;

    // refer to the unitBasic
    protected UnitBasic unit;

    protected virtual void Awake()
    {
        // get the unitBasic script upon awake
        unit = this.GetComponent<UnitBasic>();
    }



}
