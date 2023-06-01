using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class SetCharacterFeature : MonoBehaviour
{

    public GameObject dogPrefab;
    public GameObject dog;
    public GameObject pomeLongPrefab;

    //declare variables for control fur length 
    public GameObject bodyfur_back, bodyfur_middle, bodyfur_front;
    public GameObject chinfur;
    public GameObject neckfur; 
    private Vector3 longFurValue_body, middleFurValue_body, shortFurValue_body, presentFurValue_body;
    private Vector3 longFurValue_neck, middleFurValue_neck, shortFurValue_neck, presentFurValue_neck;
    private Vector3 longFurValue_chin, middleFurValue_chin, shortFurValue_chin, presentFurValue_chin;

    private Material dogMaterial;
    
    public Material pomeLong_black;
    public Material pomeLong_black_deep;
    public Material pomeLong_black_light;




    private SkinnedMeshRenderer meshRenderer; 

    private String breed = "";
    private String petName = "";
    private String section1Color = "";
    private String section2Color = "";
    private AndroidJavaObject activityContext = null;

    void Awake()
    {
        Vector3 dogScale = new Vector3(2f, 2f, 2f);


            breed = "";
            petName = "";

            
            breed = "POME_LONG";
            section1Color = "000000";
            dogPrefab = pomeLongPrefab;

            meshRenderer = dogPrefab.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
            meshRenderer.material = pomeLong_black;


            dog = Instantiate(dogPrefab, GameObject.Find("Character").transform) as GameObject;
            dog.tag = "OwnerDog";
            dog.layer = 6;

            bodyfur_back = GameObject.Find("bodyfur_back");
            bodyfur_middle = GameObject.Find("bodyfur_middle");
            bodyfur_front = GameObject.Find("bodyfur_front");
            neckfur = GameObject.Find("neckfur");
            chinfur = GameObject.Find("chinfur");

            longFurValue_body = new Vector3(1.5f, 1.5f, 1.5f );
            middleFurValue_body = new Vector3(1.0f, 1.0f, 1.0f );
            shortFurValue_body = new Vector3(0.4f, 0.4f, 0.4f );

            longFurValue_neck = new Vector3(2.0f, 2.0f, 2.0f );
            middleFurValue_neck = new Vector3(1.0f, 1.0f, 1.0f );
            shortFurValue_neck = new Vector3(0.5f, 0.5f, 0.5f );

            longFurValue_chin = new Vector3(1.6f, 1.6f, 1.6f );
            middleFurValue_chin = new Vector3(1.0f, 1.0f, 1.0f );
            shortFurValue_chin = new Vector3(0.4f, 0.4f, 0.4f );   

            DontDestroyOnLoad(dog.transform.parent);
    }

    public void FurColorLight(bool isOn)
    {
        if (isOn)
        {
            meshRenderer = dog.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
            switch (breed)
            {
                case "POME_LONG":
                    meshRenderer.material = pomeLong_black_light;                 
                    break;
                                                    
                default:
                    dogPrefab = pomeLongPrefab;
                    break;
            }
        }
    }

    public void FurColorOrigin(bool isOn)
    {
        if (isOn)
        {
            meshRenderer = dog.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
            switch (breed)
            {
                case "POME_LONG":
                    meshRenderer.material = pomeLong_black;                 
                    break;
                                                    
                default:
                    dogPrefab = pomeLongPrefab;
                    break;
            }
        }
    }


    public void FurColorDeep(bool isOn)
    {
        if (isOn)
        {
            meshRenderer = dog.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
            switch (breed)
            {
                case "POME_LONG":
                    meshRenderer.material = pomeLong_black_deep;                    
                    break;
                                                  
                default:
                    dogPrefab = pomeLongPrefab;
                    break;
            }
        }
    }

    public void FurLengthShort(bool isOn)
    {
        if (isOn){
            bodyfur_back.transform.localScale = shortFurValue_body;
            bodyfur_middle.transform.localScale = shortFurValue_body;
            bodyfur_front.transform.localScale = shortFurValue_body;

            chinfur.transform.localScale = shortFurValue_chin;
            neckfur.transform.localScale = shortFurValue_neck;
        }
    }


    public void FurLengthMiddle(bool isOn)
    {
        if (isOn){
            bodyfur_back.transform.localScale = middleFurValue_body;
            bodyfur_middle.transform.localScale = middleFurValue_body;
            bodyfur_front.transform.localScale = middleFurValue_body;

            chinfur.transform.localScale = middleFurValue_chin;
            neckfur.transform.localScale = middleFurValue_neck;  
        }
    }

    public void FurLengthLong(bool isOn)
    {
        if (isOn){
            bodyfur_back.transform.localScale = longFurValue_body;
            bodyfur_middle.transform.localScale = longFurValue_body;
            bodyfur_front.transform.localScale = longFurValue_body;

            chinfur.transform.localScale = longFurValue_chin;
            neckfur.transform.localScale = longFurValue_neck;
        }
    }
}
