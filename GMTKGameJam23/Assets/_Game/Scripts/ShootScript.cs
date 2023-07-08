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
    private float holdTimer = 0f;
    private Rigidbody rb;
    private LineRenderer lr;
    bool isLaunching = false;
    #endregion

    #region Unity Methods

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (isLaunching)
        {
            // Detect if we are touching the floor in order to cancel and stand back up.
            // Reenabling the mvoement script and ability to launch again.
        }

        lr.SetPosition(1, transform.position);
        lr.SetPosition(0, transform.position);
        if (Input.GetMouseButton(0))
        {
            CalculateTimeHeld();
            Debug.Log(holdTimer);
            lr.SetPosition(1, transform.position + CalculateLaunchAngle() * 7 * holdTimer);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (holdTimer >= 0.2f)
            {
                // set rotation to the angle of launch
                rb.AddForce(CalculateLaunchAngle() * CalculateLaunchForce() * Time.deltaTime, ForceMode.Impulse);
                holdTimer = 0;
                // unset this when touching the ground.
                isLaunching = true;
            }
        }
    }
    #endregion

    #region Private Methods
    // Private Methods.
    private Vector3 CalculateLaunchAngle()
    {
        var screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = transform.position - screenPoint;
        angle.z = 0;
        return angle.normalized;
    }

    private float CalculateLaunchForce()
    {
        // Calculate force based on base force and a velocity scaler based on how long the mouse is clicked before being released. 
        return 5000 * holdTimer;
    }

    private void CalculateTimeHeld()
    {
        holdTimer += Time.deltaTime;
        holdTimer = Mathf.Clamp(holdTimer, 0f, 1.5f);
    }

    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
