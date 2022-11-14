using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    // which gridObject this debugObject refers to
    // use object instead of gridobject to make this debugobject usable for both grid system and path finding
    private object gridObject;

    // refer to the textmesh pro
    [SerializeField] private TextMeshPro textMeshPro;


    // functions used to sign the gridobject to this debugobject
    public virtual void SetGridObject(object gridObject_)
    {
        // sign the gridobject
        this.gridObject = gridObject_;
    }


    protected virtual void Update()
    {
        // update the gridobject information
        textMeshPro.text = gridObject.ToString();
    }
}
