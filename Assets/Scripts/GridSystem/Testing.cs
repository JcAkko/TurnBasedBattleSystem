using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private UnitBasic unit;

    
  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            /*
            // get the mouse grid position
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseCast.GetMousePosition());

            // calculate the path from zero to the mouse grid pos
            GridPosition startPosition = new GridPosition(0,0);

            List<GridPosition> gridPosList = PathFinding.Instance.FindPath(startPosition, mouseGridPosition);

            for (int i = 0; i < gridPosList.Count - 1; i++)
            {

                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPosList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPosList[i + 1]),
                    Color.red,
                    10.0f
                    ) ;
            }
            */
            
        }
      
    }


}


/*
// testing class for generatic
// do not need to define the type of the class until the class is created
// used to limit the T type: public class MyClass<T> where T: BaseAction
public class MyClass<T>
{
    // T for type, can refers to any type of the variables
    private T i;

    // constructor
    public MyClass(T i_)
    {
        this.i = i_;
        Debug.Log(i);
    }


    // also generatic can be used inside the function as well
    public void Testing<T>(T t_)
    {
        Debug.Log(t_);
    }
}
*/
