using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Defective.JSON;
using TMPro;

public class BreedSceneScript : MonoBehaviour
{
    private string breedUrl = "http://3.35.242.182:3000/image";
    [SerializeField] private RawImage rawImage;
    [SerializeField] private CanvasGroup currentCanvas, resultCanvas;
    [SerializeField] private TextMeshProUGUI breedText;
    [SerializeField] private GameObject breedBubble;
    public static string color1, breed;
    public void GetImage()
    {
        breedBubble.SetActive(false);
        NativeGallery.GetImageFromGallery((image) =>  //mobile gallery folder open using NativeGallery Plugin
        {
            FileInfo selectedImage = new FileInfo(image); //choose image from gallery folder

            if (!string.IsNullOrEmpty(image))
            { // if image is selected, start coroutine(load image)
                StartCoroutine(UploadImageToGetBreed(image));
                StartCoroutine(LoadImage(image));
            }
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
        var tempImage = File.ReadAllBytes(imagePath);

        //Fit image
        Texture2D texture = new Texture2D(1080, 1203);
        texture.LoadImage(tempImage);

        rawImage.texture = texture;
        rawImage.SetNativeSize();
        ImageSizeSetting(rawImage, 1080, 1203);


        CanvasUtil.hideCanvasGroup(currentCanvas);
        CanvasUtil.showCanvasGroup(resultCanvas);
        yield return null;
    }

    IEnumerator UploadImageToGetBreed(string imagePath)
    {
        WWWForm form = new WWWForm();

        var tempImage = File.ReadAllBytes(imagePath);
        form.AddBinaryData("image", tempImage, Path.GetFileName(imagePath));
        UnityWebRequest request = UnityWebRequest.Post(breedUrl, form);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Image upload failed: " + request.error);
        }
        else
        {
            // Extract the image data from the response
            JSONObject responseJson = new JSONObject(request.downloadHandler.text);
            breed = responseJson.GetField("breed").stringValue;
            color1 = responseJson.GetField("content").GetField("section1").stringValue;
            setBreedText(breed);
        }
    }

     void setBreedText(string breed)
    {
        string korean;
        switch (breed)
        {
            case "MALTESE":
                korean = "말티즈";
            break;
            case "POME_LONG":
            case "POME_SHORT":
                korean = "포메";
            break;
            case "CHIHUAHUA":
                korean = "치와와";
            break;
            case "SHIHTZU":
                korean = "시츄";
            break;
            case "BEAGLE":
                korean = "비글이";
            break;
            case "YORKSHIRE":
                korean = "요크셔";
            break;
            case "GOLDEN":
                korean = "리트리버";
            break;
            case "PUG":
                korean = "퍼그";
            break;
            case "POODLE":
                korean = "푸들이";
            break;
            default:
                korean = "포메";
            break;
        }

        breedText.text = "저는 " + korean + "에요!";
        breedBubble.SetActive(true);
    }
    void ImageSizeSetting(RawImage img, float x, float y)
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if (x / y > imgX / imgY) // if image height is longer than width 
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (y / imgY));
        }
        else // if image width is longer than height 
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (x / imgX));
        }
    }


}
