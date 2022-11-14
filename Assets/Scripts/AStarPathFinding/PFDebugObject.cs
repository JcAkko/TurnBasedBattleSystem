using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PFDebugObject : GridDebugObject
{
    // refer to the textmesh pro, this is the world object so not GUI
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;

    // the path Node that this debug obj belongs to
    private PathNode pathNode;

    // overide the update
    protected override void Update()
    {
        // show the grid position info
        base.Update();

        // update the costs
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
    }



    // overide the set grid object
    public override void SetGridObject(object gridObject_)
    {
        // make sure it also receives the gridObject to show the grid position info
        base.SetGridObject(gridObject_);

        // sign the path node
        pathNode = (PathNode)gridObject_;
        
    }


}
