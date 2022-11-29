using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurnTimer : MonoBehaviour
{
    // *** check if its trun to act
    private bool isMyTurn;

    // the max wait time
    [SerializeField] private float maxTurnTimer;

    // the current timer
    private float turnTimer = 0;

    // refer to the character Tag UI
    private GameObject characterTagUI;

    // variables used for unit timeline Tag
    private float YPos;
    private float XStart;
    private float XEnd;
    private Vector3 startPos;
    private Vector3 endPos;


    private void Start()
    {
        // get the positions used for update the character Tag UI
        YPos = TimelineUI.Instance.GetSliderYValue();
        XStart = TimelineUI.Instance.GetMinSliderValue();
        XEnd = TimelineUI.Instance.GetMaxSliderValue();
        // calcualte the start and end position of the unit Tag UI
        startPos = new Vector3(XStart, -YPos, 0);
        endPos = new Vector3(XEnd, -YPos, 0);
    }



    private void Update()
    {
        // used for double unit system timer
        if (isMyTurn == false && turnTimer > 0.2f)
        {
            turnTimer -= Time.deltaTime;
        }
        else if (isMyTurn == false)
        {
            isMyTurn = true;
            // recharge the aciton point
            this.GetComponent<UnitBasic>().RechargeUnitActionPoint();
            // refresh the UI
            TurnSystem.Instance.IndieUnitCountDownFinished();
        }

        // update the character tag UI for the timeline system
        UpdateCharacterTag();

    }

    // ***function used to return if ite the unit turn or not
    public bool IsMyTurn()
    {
        return isMyTurn;
    }


    // *** function used to switch isMyTurn
    public void SwithIsMyTurnState()
    {
        isMyTurn = !isMyTurn;
    }

    // function used to reset the turnTimer back to max
    public void ResetTurnTimer()
    {
        turnTimer = maxTurnTimer;
    }


    // function used to set the character UI
    public void SetCharacterTagUI(GameObject TagUI)
    {
        characterTagUI = TagUI;
    }


    // function used to update the CharacterTag if there is one
    private void UpdateCharacterTag()
    {
        if (characterTagUI == null)
        {
            return;
        }

        //calculate the position ratio
        float ratio = turnTimer / maxTurnTimer;

        // update the slider
        characterTagUI.transform.localPosition = Vector3.Lerp(endPos, startPos, ratio);
    }

}
