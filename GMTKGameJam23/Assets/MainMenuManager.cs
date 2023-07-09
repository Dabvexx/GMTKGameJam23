using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// 
///</summary>
public class MainMenuManager : MonoBehaviour
{
    #region Variables
    // Variables.
    
    #endregion

    #region Unity Methods

    void Start()
    {
        if (SpeedrunTimer.Instance != null)
        {
            Destroy(SpeedrunTimer.Instance.gameObject);
        }

        if (GameObject.FindWithTag("AudioManager") != null)
        {
            Destroy(GameObject.FindWithTag("AudioManager"));
        }
    }

    void Update()
    {
        
    }

    #endregion

    #region Private Methods
    // Private Methods.
    
    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
