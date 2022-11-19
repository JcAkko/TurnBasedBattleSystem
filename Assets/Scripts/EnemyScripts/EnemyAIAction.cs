using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this class holds the enemy AI data
public class EnemyAIAction
{
    // the gridpostion of the AI action
    // when game start, there will be EnemyAIAction object created for each grid position
    public GridPosition gridPostion;

    // the value used by AI to determine the value of this AI action
    public int actionValue;
    
}
