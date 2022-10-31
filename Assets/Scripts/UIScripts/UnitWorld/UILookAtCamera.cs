using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamera : MonoBehaviour
{
    // the cam
    private Transform mainCamTransform;


    // option to invert the canvas
    [SerializeField] private bool canvasInvert;

    private void Awake()
    {
        // sign the main camera
        mainCamTransform = Camera.main.transform;
    }



    // use late update incase need move the camera during update 
    private void LateUpdate()
    {
        if (canvasInvert)
        {
            // the direction from unit to the camera
            Vector3 dirToCamera = (mainCamTransform.position - this.transform.position).normalized;
            // invert it by -1
            transform.LookAt(this.transform.position + dirToCamera *-1);
        }
        else
        {
            this.transform.LookAt(mainCamTransform);
        }
        
    }
}
