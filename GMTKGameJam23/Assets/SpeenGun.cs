using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class SpeenGun : MonoBehaviour
{
    #region Variables
    // Variables.
    [SerializeField] private Transform pivot;
    [SerializeField] private float spinSpeed;
    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(pivot.position, transform.right, spinSpeed * Time.deltaTime);
    }

    #endregion

    #region Private Methods
    // Private Methods.
    
    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
