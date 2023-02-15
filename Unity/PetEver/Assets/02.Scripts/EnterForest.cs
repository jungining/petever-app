using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class EnterForest : MonoBehaviour
{
    
    GameObject ManCharacter;
    GameObject MainCanvas;
    GameObject MainEvent;
    private bool isEntered = false;

    void Start()
    {
        ManCharacter = GameObject.FindGameObjectWithTag("Owner");      
        MainCanvas = GameObject.FindGameObjectWithTag("UICanvas");
        MainEvent = GameObject.FindGameObjectWithTag("MainEventSystem");
    }
   
    private void Awake()
    {
    
    }

    IEnumerator<object> LoadYourAsyncScene()
    {
        string sceneName = "TherapyForest";
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();
 
        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
 
        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(ManCharacter, SceneManager.GetSceneByName(sceneName));
        SceneManager.MoveGameObjectToScene(MainEvent, SceneManager.GetSceneByName(sceneName));
        SceneManager.MoveGameObjectToScene(MainCanvas, SceneManager.GetSceneByName(sceneName));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Owner") {
            if (isEntered == false) {   
                    StartCoroutine(LoadYourAsyncScene());
                    isEntered = true;
            }
        }
    }
}