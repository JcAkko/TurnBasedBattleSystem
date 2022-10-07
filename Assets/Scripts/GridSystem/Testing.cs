using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private UnitBasic unit;

    
    void Start()
    {
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //unit.GetMoveAction().GetValidGridPositionList();

            // visualize all the valid grid positons
            //GridSystemVisual.Instance.HideAllGridVisuals();
            //GridSystemVisual.Instance.ShowGridVisualsOnList(unit.GetMoveAction().GetValidGridPositionList());
        }
      
    }


}
