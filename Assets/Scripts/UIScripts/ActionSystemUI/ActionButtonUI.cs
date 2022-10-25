using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


// this script attach to the action button prefab
// used to update the action name as well as link the actual function to the button
public class ActionButtonUI : MonoBehaviour
{
    // for text
    [SerializeField]
    private TextMeshProUGUI buttonText;

    // for the button
    [SerializeField]
    private Button button;

    // for the seleted UI visual
    [SerializeField]
    private GameObject selectedVisual;

    // used to store which action is now assigned to this baseAction
    private BaseAction currentBaseActionType;


    // function used to setup the baseAction button
    public void SetUpActionButton(BaseAction baseAction_)
    {
        //update the current base action type
        this.currentBaseActionType = baseAction_;
        // update the text
        buttonText.text = baseAction_.GetActionName();
        // sign the button click event
        button.onClick.AddListener(() =>
        {
            // sign the current action as this action
            UnitActionSystem.Instance.SetSelectedAction(baseAction_);
        
        });
    }


    // function used to update the selectedVisual
    public void UpdateSelectedVisual()
    {
        // check which actio is now active
        BaseAction activeBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        // chech if the active action is this one
        selectedVisual.SetActive(activeBaseAction == this.currentBaseActionType);


    }
    
}
