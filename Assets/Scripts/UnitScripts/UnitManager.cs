using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to manage all the units in game
public class UnitManager : MonoBehaviour
{
    // singleton
    public static UnitManager Instance { get; private set; }


    // three list to hold all unit, player unit and enemy unit
    private List<UnitBasic> unitList;
    private List<UnitBasic> playerUnitList;
    private List<UnitBasic> enemyUnitList;

    private void Awake()
    {
        // check if there's multiple instance for this class, if so destory them and only leave one exist
        if (Instance != null)
        {
            Destroy(this.gameObject);
            // exit the function so no more new instance created
            return;
        }

        // if not instance yet, sign the instance
        Instance = this;

        // initialize all the lists
        unitList = new List<UnitBasic>();
        playerUnitList = new List<UnitBasic>();
        enemyUnitList = new List<UnitBasic>();
    }
    private void Start()
    {
        // subscribe to the unit spawn and death event
        // go project manager to make sure this script runs first
        UnitBasic.OnAnyUnitSpawned += UnitBasic_OnAnyUnitSpwaned;
        UnitBasic.OnAnyUnitDead += UnitBasic_OnAnyUnitDead;
    }

    // response to unit spawn event
    private void UnitBasic_OnAnyUnitSpwaned(object sender, EventArgs empty)
    {
        // get the sender
        UnitBasic unit = sender as UnitBasic;
        // add the unit into the general list
        unitList.Add(unit);
        // test if enemy and add to list accordingly
        if (unit.IsUnitEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            playerUnitList.Add(unit);
        }

        //Debug.Log(unit + " Spawned");

    }


    // response to unit dead event
    private void UnitBasic_OnAnyUnitDead(object sender, EventArgs empty)
    {
        // get the sender
        UnitBasic unit = sender as UnitBasic;
        // remvoe the unit from general list
        unitList.Remove(unit);
        // test if enemy and remove from list accordingly
        if (unit.IsUnitEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            playerUnitList.Remove(unit);
        }

        //Debug.Log(unit + " Died");

    }


    // expose function
    public List<UnitBasic> GetUnitList()
    {
        return unitList;
    }

    // expose function
    public List<UnitBasic> GetPlayerUnitList()
    {
        return playerUnitList;
    }

    // expose function
    public List<UnitBasic> GetEnemyUnitList()
    {
        return enemyUnitList;
    }


}
