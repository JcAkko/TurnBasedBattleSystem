using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{

    // the turn number text
    [SerializeField]
    private TextMeshProUGUI turnNumber;

    // the end turn button
    [SerializeField]
    private Button EndTurnButton;

    // the enemy turn UI
    [SerializeField]
    private GameObject EnemyTurnUI;

    // the unit cool down UI
    [SerializeField] private GameObject UnitCoolDownUI;


    // *** the individual enenmy end turn button
    [SerializeField] private Button IndieEndTurnButton;



    private void Start()
    {
        // add function to the button click
        EndTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        // *** the individual end turn
        IndieEndTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.UnitRechargeForNextTurn();
        });


        // subscribe to the turn change event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        //subscribe to the Unit turn down finished event
        TurnSystem.Instance.OnIndividualUnitTurnChanged += TurnSystem_OnIndieTurnChanged;

        //subscribe to the on unit selection change event
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;

        // update the turn number
        UpdateTurnNumber();

        // update the enemy turn UI and the end turn button UI
        UpdateEnemyTurnUI();
        //HideEndTurnButtonUponEnemyPhase();
    }



    private void UpdateTurnNumber()
    {
        turnNumber.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }


    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {
        UpdateTurnNumber();
        UpdateEnemyTurnUI();
        HideEndTurnButtonUponEnemyPhase();
    }


    private void TurnSystem_OnIndieTurnChanged(object sender, EventArgs empty)
    {
        UpdateTurnNumber();
        UpdateCoolDownUI();
        // show hide the button
        //HideEndTurnButtonUponEnemyPhase();
    }


    // response to the unit selection changed
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        // show or hide the charging UI depends on the active state of the current selected unit
        UpdateCoolDownUI();
    }


    // update the enemy turn UI is its the enemy turn
    private void UpdateEnemyTurnUI()
    {
        EnemyTurnUI.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }


    // *** update the player cool down UI is unit is in cool down
    private void UpdateCoolDownUI()
    {
        UnitCoolDownUI.SetActive(!UnitActionSystem.Instance.IsUnitIndividualTurn());
    }

    // function used to hide the end turn button when its enemy turn
    private void HideEndTurnButtonUponEnemyPhase()
    {
        // only show it during player's turn
        EndTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }


}
