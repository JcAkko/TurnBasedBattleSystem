using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    // the gridsystem this gridobjct belongs to
    private GridSystem gridSystem;
    // record the grid positon of this grid object
    private GridPosition gridPosition;
    // used to store all the units on this grid
    private List<UnitBasic> unitList;


    // constructor
    public GridObject(GridSystem gridSystem_, GridPosition gridPosition_)
    {
        // sign the parameters
        this.gridSystem = gridSystem_;
        this.gridPosition = gridPosition_;

        // populate the list 
        unitList = new List<UnitBasic>();
    }


    // override the to string to expose the grid x,z
    public override string ToString()
    {
        // loop the list and show all the units on list
        string unitInfomation = "";
        foreach (UnitBasic unit in unitList)
        {
            unitInfomation += unit + "\n";
        }

        // return the coordinate postion as well as the unit on this grid
        return gridPosition.ToString() + "\n" + unitInfomation;
    }


    // function used to add unit to the list
    public void AddUnit(UnitBasic unit_)
    {
        unitList.Add(unit_);
    }


    // function used to remove unit from the list
    public void RemoveUnit(UnitBasic unit_)
    {
        unitList.Remove(unit_);
    }


    // function used to retrun the unitList
    public List<UnitBasic> GetUnitList()
    {
        return unitList;
    }


    // function used to check if this grid is occupied by any unit
    public bool IsOccupied()
    {
        return unitList.Count > 0;
    }


}
