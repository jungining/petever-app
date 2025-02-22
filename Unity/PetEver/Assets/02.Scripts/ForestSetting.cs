using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestSetting : MonoBehaviour
{
    GameObject forestCanvas;

    GameObject manCharacter;
    GameObject mainEvent;
    GameObject mainCanvas;
    GameObject popupBack;

    void Start() {
        manCharacter = GameObject.FindGameObjectWithTag("Owner");
        mainEvent = GameObject.FindGameObjectWithTag("MainEventSystem");
        mainCanvas = GameObject.FindGameObjectWithTag("UICanvas");
        if (mainCanvas) {
            mainCanvas.SetActive(false);
        }

        popupBack = GameObject.FindGameObjectWithTag("WalkTag");
        if (popupBack) {
            popupBack.SetActive(false);
        }

        forestCanvas = GameObject.FindGameObjectWithTag("ForestCanvas");
    }

    void SetBackPos() {
        PlayerInput.InitJoystick();
        PlayerController.isForest = false;
        Vector3 backOffset = new Vector3(-25.3f, 0.81f, -94.53f);
        manCharacter.transform.position = backOffset;

        Transform Target =  GameObject.Find("WalkLine").transform;
        manCharacter.transform.RotateAround(Target.position, Vector3.up, 0.0f);
    }

    IEnumerator<object> GoWorldScene(string SceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
 
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SetBackPos();

        SceneManager.MoveGameObjectToScene(manCharacter, SceneManager.GetSceneByName(SceneName));
        SceneManager.MoveGameObjectToScene(mainEvent, SceneManager.GetSceneByName(SceneName));
        SceneManager.MoveGameObjectToScene(mainCanvas, SceneManager.GetSceneByName(SceneName));
        forestCanvas.SetActive(false);
        SceneManager.UnloadSceneAsync("TherapyForest");
    }

    public void onForestBackClick()
    {
        popupBack.SetActive(true);
    }

    public void onForestQuitClick()
    {
        mainCanvas.SetActive(true);
        popupBack.SetActive(false);
        StartCoroutine(GoWorldScene("WorldScene"));
    }

    public void onForestGoingClick()
    {
        popupBack.SetActive(false);
    }
}
