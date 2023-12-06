using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool buttonPressed;
    int minimapWidth = 324;
    int minimapHeight = 243;

    Vector3 distanceFromMinimapCenter;

    void Update()
    {
        if (buttonPressed)
        {
            float dx = Mathf.Clamp(Input.mousePosition.x - minimapWidth / 2, -minimapWidth / 2, minimapWidth / 2) / (minimapWidth / 2);
            float dy = Mathf.Clamp(Screen.height - Input.mousePosition.y - minimapHeight / 2, -minimapHeight / 2, minimapHeight / 2) / -(minimapHeight / 2);
            distanceFromMinimapCenter = new Vector3(dx, dy, 0f);
        }
        else
        {
            distanceFromMinimapCenter = new Vector3(0, 0, 0);
        }
    }

    public Vector3 GetMinimapInput()
    {
        if (distanceFromMinimapCenter.x != 0 && distanceFromMinimapCenter.y != 0)
        {
            return distanceFromMinimapCenter;
        }
        return new Vector3(0, 0, 0);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

}