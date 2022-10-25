using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



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

    // the current selected action
    private BaseAction selectedAction;

    // layer mask for the unit
    [SerializeField]
    private LayerMask unitLayerMask;

    // create an event to change the visual of the unit upon selection
    public event EventHandler OnSelectedUnitChange;

    // create an event to change the visual of the action upon active action changes
    public event EventHandler OnSelectedActionChange;

    // create an event to hide or show the action busy UI
    // this event return a bool by checking the isExcutingAction variable
    public event EventHandler<bool> OnActionBusyChange;

    // event use to spend the action point upon action started
    public event EventHandler OnActionStarted;

    // bool used to check if there is an action running or not
    private bool isExcutingAction;


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


    private void Start()
    {
        // defualt select the unit
        SetSelectedUnit(selectedUnit);
    }


    private void Update()
    {
        // if there is an action running, do not update
        if (isExcutingAction)
        {
            return;
        }


        // if the pointer is over game object, it mean the mouse the on an UI element, so do not execute action
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // if selected new unit, skip the unit movement function for the current frame
        if (TryUnitSelection())
        {
            return;
        }

        // execute the action
        HandleSelectedAction();
        
    }


    // function used to execute the current selected action
    private void HandleSelectedAction()
    {
        

        if (Input.GetMouseButtonDown(0))
        {

            // convert the mouse world position into the grid postion
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseCast.GetMousePosition());

            // validate if the mouse grid positon is a valid position
            if (selectedAction.IsThisGridValidMovePosition(mouseGridPosition))
            {
                // test if the selected unit have enough action points to take the action
                if (selectedUnit.TryUseAPToTakeAction(selectedAction))
                {
                    // set the action system as running
                    StartExcuteAction();

                    // execute the selected action
                    selectedAction.TakeAction(mouseGridPosition, EndExcuteAction);

                    // let unit consume its action points
                    OnActionStarted?.Invoke(this, EventArgs.Empty);

                }
                else
                {
                    Debug.Log("Unit do not have enough action point to act");
                }
                
                
            }

        }
    }



    // function that handles the unit selection
    private bool TryUnitSelection()
    {

        // left mouse click to check
        if (Input.GetMouseButtonDown(0))
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
                    // if the unit is already selected, do not seleted again
                    if (unit == selectedUnit)
                    {
                        return false;
                    }
                    // sign the selected unit
                    SetSelectedUnit(unit);
                    // return true
                    return true;
                }
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
        // sign the default action as the move action
        SetSelectedAction(unit_.GetMoveAction());

        //OnSelectedShowVisual?.Invoke(this, EventArgs.Empty);

        // call selected unit change for all the evet subscriber
        if (OnSelectedUnitChange != null)
        {
            // sender is this, no event args so set it to empty
            OnSelectedUnitChange(this, EventArgs.Empty);
        }
    }


    // function used to switch the selected action
    public void SetSelectedAction(BaseAction action_)
    {
        selectedAction = action_;

        // call selected action change for all the evet subscriber
        if (OnSelectedActionChange != null)
        {
            // sender is this, nothing to return so set args to empty
            OnSelectedActionChange(this, EventArgs.Empty);
        }
    }


    // this function exposes the selected unit
    public UnitBasic GetSelectedUnit()
    {
        return selectedUnit;
    }

    // function used to expose the selected action
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }


    // this function set action as running
    private void StartExcuteAction()
    {
        isExcutingAction = true;

        // fire the event
        OnActionBusyChange?.Invoke(this, isExcutingAction);
    }

    //this function set action as not running
    private void EndExcuteAction()
    {
        isExcutingAction = false;

        // fire the event
        OnActionBusyChange?.Invoke(this, isExcutingAction);
    }



}
