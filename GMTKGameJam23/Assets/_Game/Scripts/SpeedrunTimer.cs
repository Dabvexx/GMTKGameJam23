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
    public int unusedShots = 0;
    #endregion

    #region Unity Methods

    void Update()
    {
        timer += Time.deltaTime;
    }

    #endregion
}
