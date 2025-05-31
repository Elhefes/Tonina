using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;

    private Vector3 distanceFromClick;
    private Vector3 initialClickPosition;
    public float distanceForMaxSpeed;

    public GameObject minimapIndicators;

    void Update()
    {
        if (buttonPressed)
        {
            distanceFromClick = new Vector3(Mathf.Clamp(Input.mousePosition.x - initialClickPosition.x, -distanceForMaxSpeed, distanceForMaxSpeed) / distanceForMaxSpeed,
            Mathf.Clamp(Input.mousePosition.y - initialClickPosition.y, -distanceForMaxSpeed, distanceForMaxSpeed) / distanceForMaxSpeed, 0f);
        }
        else
        {
            distanceFromClick = new Vector3(0, 0, 0);
        }
    }

    public Vector3 GetMinimapInput()
    {
        if (distanceFromClick.x != 0 || distanceFromClick.y != 0)
        {
            return distanceFromClick;
        }
        return new Vector3(0, 0, 0);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        initialClickPosition = Input.mousePosition;
        minimapIndicators.SetActive(false);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        minimapIndicators.SetActive(true);
    }

}