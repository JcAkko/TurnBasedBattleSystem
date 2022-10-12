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

    // function used to setup the baseAction button
    public void SetUpActionButton(BaseAction baseAction_)
    {
        // update the text
        buttonText.text = baseAction_.GetActionName();
    }
    
}
