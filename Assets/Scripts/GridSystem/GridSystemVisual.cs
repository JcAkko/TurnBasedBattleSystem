using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    // expose the class by make an static instance of the class
    public static GridSystemVisual Instance { get; private set; }


    // refer to the grid visual prefab
    [SerializeField]
    private Transform gridVisualPrefab;

    // 2d array used to hold all the grids in game
    private SingleGridVisualControl[,] singleGridVisualArray;



    private void Awake()
    {
        // there should be only one instance of this class exist
        // check if there's multiple instance for this class, if so, destory them and only leave one exist
        if (Instance != null)
        {
            Destroy(this.gameObject);
            // exit the function so no more new instance created
            return;
        }

        // if not instance yet, sign the instance
        Instance = this;
    }


    private void Start()
    {
        // create the 2d array to hold all the grid visuals
        singleGridVisualArray = new SingleGridVisualControl[LevelGrid.Instance.GetSystemWidth(), LevelGrid.Instance.GetSystemHeight()];


        // loop through all the grids in the system and find the qualified ones
        for (int x = 0; x < LevelGrid.Instance.GetSystemWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetSystemHeight(); z++)
            {
                // get the gridposition of the current grid visual object
                GridPosition gridPosition = new GridPosition(x,z);
                // instantiate the visual prefabs
                Transform gridVisualSingle = Instantiate(gridVisualPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                // add it to the array
                singleGridVisualArray[x, z] = gridVisualSingle.GetComponent<SingleGridVisualControl>();
            }
        }
    }


    private void Update()
    {
        UpdateGridVisuals();
    }

    // function used to hide all the grid visuals
    public void HideAllGridVisuals()
    {
        // loop through all the grids in the system and disable the mesh
        for (int x = 0; x < LevelGrid.Instance.GetSystemWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetSystemHeight(); z++)
            {
                
                singleGridVisualArray[x, z].DisableMeshRender();
            }
        }

    }

    // function used to only show the selected grid visuals
    public void ShowGridVisualsOnList(List<GridPosition> gridVisualList_)
    {
        foreach ( GridPosition gridPosition_ in gridVisualList_)
        {
            // only show the ones on the list
            singleGridVisualArray[gridPosition_.x, gridPosition_.z].EnableMeshRender();
        }

    }

    // function used to update the grid visuals
    private void UpdateGridVisuals()
    {
        // hide all the grids first
        HideAllGridVisuals();
        // only shows the one that validate
        ShowGridVisualsOnList(UnitActionSystem.Instance.GetSelectedAction().GetValidGridPositionList());

    }


}
