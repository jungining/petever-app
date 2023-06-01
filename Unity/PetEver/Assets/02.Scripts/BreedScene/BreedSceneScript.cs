using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class BreedSceneScript : MonoBehaviour
{

    [SerializeField] private RawImage rawImage;
    [SerializeField] private CanvasGroup currentCanvas, resultCanvas;
    public void GetImage()
    {
        NativeGallery.GetImageFromGallery((image) =>  //mobile gallery folder open using NativeGallery Plugin
        {
            FileInfo selectedImage = new FileInfo(image); //choose image from gallery folder

            if (!string.IsNullOrEmpty(image)) // if image is selected, start coroutine(load image)
                StartCoroutine(LoadImage(image));

        });
    }

    public void OnClickRetry()
    {

        CanvasUtil.hideCanvasGroup(resultCanvas);
        CanvasUtil.showCanvasGroup(currentCanvas);
    }

    public void OnClickContinue()
    {
        SceneManager.LoadSceneAsync("CreationScene", LoadSceneMode.Single);
    }


    IEnumerator LoadImage(string imagePath)
    {
        //byte[] imageData = File.ReadAllBytes(imagePath);
        //string imageName = Path.GetFileName(imagePath).Split('.')[0];
        //string saveImagePath = Application.persistentDataPath + "/Image";

        //File.WriteAllBytes(saveImagePath + imageName + ".jpg", imageData);

        var tempImage = File.ReadAllBytes(imagePath);

        // TODO : Fit image
        Texture2D texture = new Texture2D(1080, 1440);
        texture.LoadImage(tempImage);

        rawImage.texture = texture;
        CanvasUtil.hideCanvasGroup(currentCanvas);
        CanvasUtil.showCanvasGroup(resultCanvas);
        yield return null;
    }
    
}
