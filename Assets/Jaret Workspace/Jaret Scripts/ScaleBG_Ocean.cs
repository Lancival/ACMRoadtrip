using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBG_Ocean : MonoBehaviour
{
  public GameObject backgroundImage;
  public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        scaleBackgroundImageFitScreenSize();
    }

    private void scaleBackgroundImageFitScreenSize() {
      //Get Device Screen Aspect
      Vector2 deviceScreenResolution = new Vector2(Screen.width, Screen.height);
      Debug.Log (deviceScreenResolution);

      float srcHeight = Screen.height;
      float srcWidth = Screen.width;
      float DEVICE_SCREEN_ASPECT = srcWidth / srcHeight;
      Debug.Log("DEVICE_SCREEN_ASPECT: " + DEVICE_SCREEN_ASPECT.ToString());

      //Set Main Camera's aspect = Device's aspect
      mainCam.aspect = DEVICE_SCREEN_ASPECT;

      //Scale Background Image to fit camera's size
      float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
      float camWidth = camHeight * DEVICE_SCREEN_ASPECT;
      Debug.Log("camHeight: " + camHeight.ToString());
      Debug.Log("camWidth: " + camWidth.ToString());

        //Get background image size
      SpriteRenderer backgroundImageSR = backgroundImage.GetComponent<SpriteRenderer>();
      float bgImgH = backgroundImageSR.sprite.rect.height;
      float bgImgW = backgroundImageSR.sprite.rect.width;
      Debug.Log("bgImgH: " + bgImgH.ToString());
      Debug.Log("bgImgW: " + bgImgW.ToString());

        //Calculate Ratio for scaling 
      float bgImg_scale_ratio_Height = camHeight / bgImgH;
      float bgImg_scale_ratio_Width = camWidth / bgImgW;

      backgroundImage.transform.localScale = new Vector3(bgImg_scale_ratio_Width, bgImg_scale_ratio_Height, 1);
    }
}
