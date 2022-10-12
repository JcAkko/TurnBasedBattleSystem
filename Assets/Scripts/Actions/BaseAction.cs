using System;
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


    // delegate used to represent a function that might be called when spin action finished
    // use using System and call Action<> instead of creat one 
    // Action<> returns void while Func<> returns the type inside <>
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        // get the unitBasic script upon awake
        unit = this.GetComponent<UnitBasic>();
    }


    // abstract that will force all the child actions to define this function
    public abstract string GetActionName();



}
