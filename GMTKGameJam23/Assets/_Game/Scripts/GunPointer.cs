using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class GunPointer : MonoBehaviour
{
    #region Variables
    // Variables.
    [SerializeField] private GameObject pivot;
    [SerializeField] private float rotationSpeed;
    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    void Update()
    {
        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        target.z = 0;

        var direction = (target - transform.position).normalized;

        var lookRotation = Quaternion.LookRotation(direction);
        lookRotation *= Quaternion.Euler(-90, 180, 0);

        // Rotate just along x
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Private Methods
    // Private Methods.
    
    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
