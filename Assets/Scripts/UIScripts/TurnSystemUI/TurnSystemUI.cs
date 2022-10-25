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
    }



    private void UpdateTurnNumber()
    {
        turnNumber.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }


    private void TurnSystem_OnTurnChanged(object sender, EventArgs empty)
    {
        UpdateTurnNumber();
    }
}
