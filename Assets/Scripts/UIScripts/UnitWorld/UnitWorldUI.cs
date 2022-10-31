using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    // refer to the action point text
    [SerializeField] TextMeshProUGUI actionPointTextUI;

    // refer to the unit
    [SerializeField] UnitBasic unit;

    // refer to the health bar image
    [SerializeField] Image healthBarImage;

    // refer to the health system
    [SerializeField] HealthSystem healthSystem;


    private void Start()
    {
        // updat the action point and health bar
        UpdateActionPointUI();
        UpdateHealthBarUI();

        //subscribe to the action point change event
        UnitBasic.OnAnyActionPointChange += UnitBasic_OnAnyActionPointChange;

        // subscribe to the unit damaged event
        healthSystem.onUnitDamaged += healthSystem_onUnitDamaged;
    }


    // function used to update the action point
    private void UpdateActionPointUI()
    {
        actionPointTextUI.text = unit.GetActionPoints().ToString();
    }


    // called when action point updated
    private void UnitBasic_OnAnyActionPointChange(object sender, EventArgs empty)
    {
        // update the points
        UpdateActionPointUI();
    }

    // call when unit damaged
    private void healthSystem_onUnitDamaged(object sender, EventArgs empty)
    {
        // update the health bar
        UpdateHealthBarUI();
    }


    // used to update the healthbar
    private void UpdateHealthBarUI()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthPercentage();
    }

}
