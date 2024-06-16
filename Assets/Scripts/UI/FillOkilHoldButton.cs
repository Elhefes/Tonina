using UnityEngine;
using UnityEngine.EventSystems;

public class FillOkilHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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