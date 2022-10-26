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


    private void Start()
    {
        // add function to the button click
        EndTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });


        // subscribe to the turn change event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;


        // update the turn number
        UpdateTurnNumber();

        // update the enemy turn UI and the end turn button UI
        UpdateEnemyTurnUI();
        HideEndTurnButtonUponEnemyPhase();
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


    // update the enemy turn UI is its the enemy turn
    private void UpdateEnemyTurnUI()
    {
        EnemyTurnUI.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    // function used to hide the end turn button when its enemy turn
    private void HideEndTurnButtonUponEnemyPhase()
    {
        // only show it during player's turn
        EndTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }


}
