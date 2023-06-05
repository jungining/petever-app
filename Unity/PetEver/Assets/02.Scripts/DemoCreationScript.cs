using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoCreationScript : MonoBehaviour
{
    CanvasGroup DemoPicCanvas;
    CanvasGroup DemoCreationCanvas;
    CanvasGroup MySpaceCanvas;

    CanvasGroup StartCanvas;

    void Start() {
        StartCanvas = GameObject.Find("DemoIntroCanvas").GetComponent<CanvasGroup>();
        DemoPicCanvas = GameObject.Find("DemoPicCavnas").GetComponent<CanvasGroup>();
        HideGroup(DemoPicCanvas);

        DemoCreationCanvas= GameObject.Find("DemoCreationCanvas").GetComponent<CanvasGroup>();
        HideGroup(DemoCreationCanvas);

        MySpaceCanvas= GameObject.Find("MySpaceCanvas").GetComponent<CanvasGroup>();
        HideGroup(MySpaceCanvas);
    }

    public void goCreationPage() {
        DemoPicCanvas= GameObject.Find("DemoPicCavnas").GetComponent<CanvasGroup>();
        HideGroup(DemoPicCanvas);

        DemoCreationCanvas= GameObject.Find("DemoCreationCanvas").GetComponent<CanvasGroup>();
        ShowGroup(DemoCreationCanvas);
    }

    public void goMySpace() {
        DemoCreationCanvas= GameObject.Find("DemoCreationCanvas").GetComponent<CanvasGroup>();
        HideGroup(DemoCreationCanvas);

        MySpaceCanvas= GameObject.Find("MySpaceCanvas").GetComponent<CanvasGroup>();
        ShowGroup(MySpaceCanvas);
    }

    public void nextStartPage()
    {
        StartCanvas = GameObject.Find("DemoIntroCanvas").GetComponent<CanvasGroup>();
        HideGroup(StartCanvas);

        DemoPicCanvas = GameObject.Find("DemoPicCavnas").GetComponent<CanvasGroup>();
        ShowGroup(DemoPicCanvas);
    }

        private void ShowGroup(CanvasGroup gr)
    {
        gr.alpha = 1;
        gr.interactable = true;
        gr.blocksRaycasts = true;
    }

    private void HideGroup(CanvasGroup gr)
    {
        gr.alpha = 0;
        gr.interactable = false;
        gr.blocksRaycasts = false;
    }
}
