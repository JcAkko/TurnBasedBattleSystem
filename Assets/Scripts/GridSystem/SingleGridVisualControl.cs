using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGridVisualControl : MonoBehaviour
{
    // refer to the self mesh render
    [SerializeField]
    private MeshRenderer meshRender;

    // function used to disable the meshRender
    public void DisableMeshRender()
    {
        meshRender.enabled = false;
    }

    // function used to enable the meshRender
    public void EnableMeshRender(Material gridMat_)
    {
        meshRender.enabled = true;
        // sign the materal
        meshRender.material = gridMat_;
    }
}
