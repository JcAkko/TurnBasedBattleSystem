using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private UnitBasic unit;

    
    void Start()
    {
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            
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
