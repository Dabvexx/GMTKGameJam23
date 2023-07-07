using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class PivotAround : MonoBehaviour
{
    #region Variables
    // Variables.
    public GameObject pivotPoint;
    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(pivotPoint.transform.position, pivotPoint.transform.up, 20 * Time.deltaTime);
    }

    #endregion

    #region Private Methods
    // Private Methods.
    
    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
