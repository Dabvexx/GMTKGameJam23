using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class ShootScript : MonoBehaviour
{
    #region Variables
    // Variables.
    
    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Rigidbody>().AddForce(CalculateLaunchAngle() * CalculateLaunchForce() * Time.deltaTime, ForceMode.Impulse);
        }
    }

    #endregion

    #region Private Methods
    // Private Methods.
    private Vector3 CalculateLaunchAngle()
    {
        var angle = transform.position - Input.mousePosition;
        return angle.normalized;
    }

    private float CalculateLaunchForce()
    {
        // Calculate force based on distance between player and mouse, with clamped upper and lower bounds.
        return 500;
    }
    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
