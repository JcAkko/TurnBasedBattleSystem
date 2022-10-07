using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    // camera movement speed
    [SerializeField]
    float camMoveSpeed = 10.0f;

    // cam rotation speed
    [SerializeField]
    float rotationSpeed = 100.0f;


    // refer to the cinemachine cam
    [SerializeField]
    private CinemachineVirtualCamera mainCMCam;

    // the max and min boundary 
    private const float minOffsetOnY = 2.0f;
    private const float maxOffsetOnY = 12.0f;

    // refer to the cinemachine transposer thant controls the cam follow offset
    private CinemachineTransposer CMTransposer;

    // used to store the offset of the cam zoom
    private Vector3 targetCamZoomOffSet;

    // camera zoom speed
    [SerializeField]
    private float camZoomSpeed = 5.0f;


    private void Start()
    {
        // record the initial offset of the cam
        // locate the script that taking care of follow offset
        CMTransposer = mainCMCam.GetCinemachineComponent<CinemachineTransposer>();
        // apply the default offset first
        targetCamZoomOffSet = CMTransposer.m_FollowOffset;


    }



    private void Update()
    {
        // cam movement
        CamMovement();

        // cam rotation
        CamRatation();

        // zooming function
        CamZoom();

    }


    // function for cam movement
    private void CamMovement()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1.0f;
        }


        // use transform.forward and right to make sure the rotation is counted in
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;

        // move the cam
        this.transform.position += moveVector * camMoveSpeed * Time.deltaTime;
    }



    // function used to handle the rotation of the camera
    private void CamRatation()
    {
        // apply this to the cam follow object to rotate its y axis
        Vector3 rotationVector = new Vector3(0,0,0);

        // use Q and E to rotate the cam
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y -= 1.0f;
        }

        // apply to transform
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;


    }


    // function for camera zooming function
    private void CamZoom()
    {

        // setup the zoomAmont
        float zoomAmount = 1.0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            targetCamZoomOffSet.y -= zoomAmount; 
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetCamZoomOffSet.y += zoomAmount;
        }

        // limit the cam zoom boundary
        targetCamZoomOffSet.y = Mathf.Clamp(targetCamZoomOffSet.y, minOffsetOnY, maxOffsetOnY);

        // apply the offset and smooth out the transition
        CMTransposer.m_FollowOffset = Vector3.Lerp(CMTransposer.m_FollowOffset, targetCamZoomOffSet, Time.deltaTime * camZoomSpeed);

    }


    // function used to record the initial cam postion and rotation and zoom in order for result use
    private void CamReset()
    {
        // to do 
    }
    



}
