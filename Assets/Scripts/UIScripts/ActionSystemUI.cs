using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSystemUI : MonoBehaviour
{

    // action button prefab
    [SerializeField]
    private Transform actionButtonPrefab;

    // action button holder
    [SerializeField]
    private Transform actionButtonsFolder;

    // list that holds all the populated action UI buttons
    private List<ActionButtonUI> actionButtonUiList;


    private void Awake()
    {
        // instantiate the list
        actionButtonUiList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        // subscribe and listen to the UnitChange event from the unitActionSystem
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;

        // subscribe and listen to the action change event
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedAvtionChange;

        // create action UI buttons
        PopulateActionUIButtons();
        // update the selected visual
        UpdateSelectedVisual();
    }


    // function used to instantiate the action buttons
    private void PopulateActionUIButtons()
    {
        // delete all the old buttons
        foreach (Transform existingButtons in actionButtonsFolder)
        {
            Destroy(existingButtons.gameObject);
        }

        // clear the button UI list
        actionButtonUiList.Clear();

        // get the current selected unit
        UnitBasic selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        // find out the action list this unit has and instantiate the action button
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            // instantiate the prefab as the child of the button folder
            Transform button = Instantiate(actionButtonPrefab, actionButtonsFolder);
            // setup the base action for the button
            button.GetComponent<ActionButtonUI>().SetUpActionButton(baseAction);

            // add the button UI to the list
            actionButtonUiList.Add(button.GetComponent<ActionButtonUI>());
        }

    }

    // function to execute when listen to the unit selection event
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        
        // update the action buttons
        PopulateActionUIButtons();

        // update the active action buttons
        UpdateSelectedVisual();
    }


    // function to execute when listen to the actioon selection event
    private void UnitActionSystem_OnSelectedAvtionChange(object sender, EventArgs empty)
    {

        // update the active action buttons
        UpdateSelectedVisual();
    }



    // function used to update the selected action UI visual
    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI buttonUI in actionButtonUiList)
        {
            // loop through all the button UI, high light the selected button
            buttonUI.UpdateSelectedVisual();
        }
    }


}
