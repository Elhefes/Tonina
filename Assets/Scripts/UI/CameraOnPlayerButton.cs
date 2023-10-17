using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraOnPlayerButton : MonoBehaviour
{
    public Sprite cameraOn;
    public Sprite cameraOff;
    public Image cameraImage;

    public void ChangeIconSprite(bool cameraOnPlayer)
    {
        if (cameraOnPlayer) cameraImage.sprite = cameraOn;
        else cameraImage.sprite = cameraOff;
    }
}
