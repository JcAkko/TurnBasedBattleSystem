using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this game use A star path finding algorithm
// the path finding system will utilize the same grid system base as the levelGrid do, but a differetn gridsystem Type
public class PathFinding : MonoBehaviour
{

    // prefab for debug object
    [SerializeField] private Transform gridDebugObjectPrefab;

    // define the width, height and cellsize of the path finding grids
    // the length of the map on x axis
    private int xlength;

    // the length of the map on z axis
    private int zlength;

    // the size of each individual floor cell
    private float cellSize;

    // store the current path finding grid system
    private GridSystem<PathNode> pfGridSystem;

    private void Awake()
    {
        // create the grid system for path finding
        pfGridSystem = new GridSystem<PathNode>(10, 10, 2.0f,
            (GridSystem<PathNode> g_, GridPosition gridPosition_) => new PathNode(gridPosition_));

        // populate the gridDebugObjects onto each grid
        pfGridSystem.CreateDebugObjects(gridDebugObjectPrefab);

    }


}
