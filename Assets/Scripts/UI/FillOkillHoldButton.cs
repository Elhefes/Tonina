using UnityEngine;
using UnityEngine.EventSystems;

public class FillOkillHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }

    void OnDisable() { buttonPressed = false; }
}