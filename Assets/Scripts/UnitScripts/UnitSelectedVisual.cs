using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    // only refers to the unit itself
    [SerializeField]
    private UnitBasic unit;

    // refer to the mesh render in order to control on and off
    private MeshRenderer selfMeshRender;

    private void Awake()
    {
        // sign the mesh render of the unit
        selfMeshRender = this.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        // sign the event
        UnitActionSystem.Instance.OnSelectedShowVisual += UnitActionSystem_OnSelectedShowVisual;
        // update the visual
        UpdateSelectionVisual();
    }

    // function called when show visual event triggered
    private void UnitActionSystem_OnSelectedShowVisual(object sender, EventArgs empty)
    {
        // update the visual aid
        UpdateSelectionVisual();
    }


    // update the visual aid
    private void UpdateSelectionVisual()
    {
        // check if this unit is the selected unit, if so, enable the visual
        if (this.unit == UnitActionSystem.Instance.GetSelectedUnit())
        {
            selfMeshRender.enabled = true;
        }
        else
        {
            selfMeshRender.enabled = false;
        }
    }


}
