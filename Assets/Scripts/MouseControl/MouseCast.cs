using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCast : MonoBehaviour
{
    // layer mask for the floor which mouse can click on
    [SerializeField]
    private LayerMask floorLayerMask;

    
    // create an instance for the class
    // the game will only have single instance of this class
    private static MouseCast instance;



    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        // move the position of this object to the point where it hits
        // this is for visual aids only
        this.transform.position = MouseCast.GetMousePosition();
    }

    // get the mouse point positon
    public static Vector3 GetMousePosition()
    {
        // cast ray to catch the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // does ray cast, return true if hit
        Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, instance.floorLayerMask);
        // the point been hit by the ray cast is the rayCastHit
        return rayCastHit.point;

    }



}
