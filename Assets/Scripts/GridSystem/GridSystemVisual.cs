using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    // expose the class by make an static instance of the class
    public static GridSystemVisual Instance { get; private set; }

    // use struct to store all the grid visual prefab info
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }


    // grid visual for different action
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }



    // refer to the grid visual prefab
    [SerializeField]
    private Transform gridVisualPrefab;

    // refer to the grid visual mat list
    [SerializeField]
    private List<GridVisualTypeMaterial> gridVisualTypeMatList;

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

        // subscribe to the unit selection event
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        // subscribe to the actio changed event
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        // subscribe to the unit grid pos change
        LevelGrid.Instance.OnAnyUnitGridPosChange += LevelGrid_OnAnyUnitGridPosChange;

        // initial update
        UpdateGridVisuals();

    }


    // response to the unit selection changed event
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        UpdateGridVisuals();
    }

    // response to the selected action changed
    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs empty)
    {
        UpdateGridVisuals();
    }

    // response to the unit grid pos change
    private void LevelGrid_OnAnyUnitGridPosChange(object sender, EventArgs empty)
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


    // show the action range for certain action
    public void ShowGridPostionRange(GridPosition gridPosition_, int range_, GridVisualType gridVisualType_)
    {
        // list to store all the valid position
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range_; x <= range_; x++)
        {
            for (int z = -range_; z <= range_; z++)
            {
                // record the postion
                GridPosition testGridPosition = gridPosition_ + new GridPosition(x, z);

                // check if the gridpos is valid
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range_)
                {
                    continue;
                }

                // if pass the test, add to the list
                gridPositionList.Add(testGridPosition);

            }
        }


        // show the visual on list
        ShowGridVisualsOnList(gridPositionList, gridVisualType_);

    }


    // function used to only show the selected grid visuals
    public void ShowGridVisualsOnList(List<GridPosition> gridVisualList_, GridVisualType gridVisualType_)
    {
        foreach ( GridPosition gridPosition_ in gridVisualList_)
        {
            // only show the ones on the list
            // check the action type and sign different material accordingly
            singleGridVisualArray[gridPosition_.x, gridPosition_.z].EnableMeshRender(GetGridVisualMaterial(gridVisualType_));
        }

    }

    // function used to update the grid visuals
    private void UpdateGridVisuals()
    {
        // hide all the grids first
        HideAllGridVisuals();
        // get the selected action
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        // get the current unit grid position
        GridPosition currentGridPos = UnitActionSystem.Instance.GetSelectedUnit().GetUnitCurrentGridPosition();

        // grid visual type for the action
        GridVisualType gridVisualType;
        
        // find the action type
        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case AttackAction attackAction:
                gridVisualType = GridVisualType.Red;

                // show the attack range
                ShowGridPostionRange(currentGridPos, attackAction.GetAttackRange(), GridVisualType.RedSoft);
                break;
            
        }
        // only shows the one that validate
        ShowGridVisualsOnList(UnitActionSystem.Instance.GetSelectedAction().GetValidGridPositionList(), gridVisualType);

    }


    // functin used to find the correct materail based on the grid visual type
    private Material GetGridVisualMaterial(GridVisualType gridVisualType_)
    {
        // loop through the list and find the correct one
        foreach (GridVisualTypeMaterial gridVisualMat in gridVisualTypeMatList)
        {
            if (gridVisualMat.gridVisualType == gridVisualType_)
            {
                return gridVisualMat.material;
            }
            
        }


        Debug.LogError("Could not find the correstponding materail for the gird visual, please check");
        // if find nothing return null
        return null;

    }


}
