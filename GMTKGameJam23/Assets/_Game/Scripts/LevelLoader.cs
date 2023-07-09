using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


///<summary>
/// 
///</summary>
public class LevelLoader : MonoBehaviour
{
    #region Variables
    // Variables.
    public Animator transition;
    public float transitionLength = 1f;
    public static LevelLoader Instance { get; private set; }
    public Scenes nextScene;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        // R To reload level.
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex));
        }
    }

    #endregion

    #region Private Methods
    // Private Methods.

    #endregion

    #region Public Methods
    // Public Methods.
    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCoroutine((int)nextScene));
    }

    public IEnumerator LoadLevelCoroutine(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionLength);

        SceneManager.LoadSceneAsync(sceneIndex);
    }
    #endregion

    public enum Scenes
    {
        MainMenu = 0,
        Tutorial1 = 1,
        Tutorial2 = 2,
        Level1 = 3,
        Level2 = 4,
        Level3 = 5,
        EndScreen = 6
    };
}
