using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class SpeedrunTimer : MonoBehaviour
{
    #region Variables
    // Variables.
    public float timer = 0f;
    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    #endregion

    #region Private Methods
    // Private Methods.
    
    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
