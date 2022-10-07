using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    // which gridObject this debugObject refers to
    private GridObject gridObject;

    // refer to the textmesh pro
    [SerializeField] private TextMeshPro textMeshPro;


    // functions used to sign the gridobject to this debugobject
    public void SetGridObject(GridObject gridObject_)
    {
        // sign the gridobject
        this.gridObject = gridObject_;
    }


    private void Update()
    {
        // update the gridobject information
        textMeshPro.text = gridObject.ToString();
    }
}
