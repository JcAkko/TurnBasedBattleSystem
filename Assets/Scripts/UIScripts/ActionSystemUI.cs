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


    private void Start()
    {
        // subscribe and listen to the UnitChange event from the unitActionSystem
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        // create action UI buttons
        PopulateActionUIButtons();
    }


    // function used to instantiate the action buttons
    private void PopulateActionUIButtons()
    {
        // delete all the old buttons
        foreach (Transform existingButtons in actionButtonsFolder)
        {
            Destroy(existingButtons.gameObject);
        }

        // get the current selected unit
        UnitBasic selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        // find out the action list this unit has and instantiate the action button
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            // instantiate the prefab as the child of the button folder
            Transform button = Instantiate(actionButtonPrefab, actionButtonsFolder);
            // setup the base action for the button
            button.GetComponent<ActionButtonUI>().SetUpActionButton(baseAction);
        }

    }

    // function to execute when listen to the unit selection event
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        
        // update the action buttons
        PopulateActionUIButtons();
    }


}
