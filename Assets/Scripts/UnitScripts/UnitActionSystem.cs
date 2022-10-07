using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// *** this script have to top priority, run first ***
public class UnitActionSystem : MonoBehaviour
{
    // a singleton for this class so make sure only once instance for this class exist
    // class can be called by other classes
    // other class can call this instance but can not edit it
    public static UnitActionSystem Instance { get; private set; }


    // this unit that been selected
    [SerializeField]
    private UnitBasic selectedUnit;

    // layer mask for the unit
    [SerializeField]
    private LayerMask unitLayerMask;

    // create an event to change the visual of the unit upon selection
    public event EventHandler OnSelectedShowVisual;


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


    private void Update()
    {
        
        // move the selected target when mouse click on floor
        if (Input.GetMouseButtonDown(0))
        {
            // if selected new unit, skip the unit movement function for the current frame
            if (TryUnitSelection()) return;

            // convert the mouse world position into the grid postion
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseCast.GetMousePosition());
            // check if the postion mouse clicked on is a valid move position
            if (selectedUnit.GetMoveAction().IsThisGridValidMovePosition(mouseGridPosition))
            {
                // if so, move the unit
                // reach the moveaction script and move the selected unit funciton
                selectedUnit.GetMoveAction().MoveUnitTo(mouseGridPosition);
            }
            
        }


        // spin function
        if (Input.GetMouseButtonDown(1))
        {
            // spin
            selectedUnit.GetSpinAction().SpinUnit(); ;

        }
    }

    // function that handles the unit selection
    private bool TryUnitSelection()
    {

        // cast ray to catch the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // does ray cast, if hit unit, get the unitBasic script
        if (Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, unitLayerMask))
        {
            // if there is the UnitBasic component on the object, return it
            // if there is not returns false
            if (rayCastHit.transform.TryGetComponent<UnitBasic>(out UnitBasic unit))
            {
                // sign the selected unit
                SetSelectedUnit(unit);
                // return true
                return true;
            }
        }

        // if nothing hit by ray
        return false;


    }


    // function that set the selected unit
    private void SetSelectedUnit(UnitBasic unit_)
    {
        // sign the selected unit
        selectedUnit = unit_;
        // sign the 

        //OnSelectedShowVisual?.Invoke(this, EventArgs.Empty);

        // call visual change for all the evet subscriber
        if (OnSelectedShowVisual != null)
        {
            // sender is this, no event args so set it to empty
            OnSelectedShowVisual(this, EventArgs.Empty);
        }
    }


    // this function exposes the selected unit
    public UnitBasic GetSelectedUnit()
    {
        return selectedUnit;
    }



}
