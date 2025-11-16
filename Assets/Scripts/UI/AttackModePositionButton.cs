using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackModePositionButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector3 positionToMoveTo;
    public MouseLook mouseLook;
    public AttackModeSpawnController attackModeSpawnController;
    public DoubleClickDetector doubleClickDetector;

    private int spawnNumber;

    private float clicked = 0;
    private float clickTime = 0;
    private float clickDelay = 0.67f;

    private bool buttonPressed;

    void Start()
    {
        spawnNumber = (int)Char.GetNumericValue(gameObject.name.ToString()[0]) - 1; // Array number of button retrieved from the GameObject name
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        attackModeSpawnController.UpdateSelectedSpawnNumber(spawnNumber);
        attackModeSpawnController.UpdateButtonsPosition(transform.localPosition.x);

        doubleClickDetector.AddClick();
        if (doubleClickDetector.DoubleClickDetected()) SelectPlayerSpawn();
    }

    IEnumerator StartDelayedButtonPress()
    {
        yield return new WaitForSeconds(0.4f);
        if (buttonPressed)
        {
            attackModeSpawnController.UpdateSelectedSpawnNumber(spawnNumber);
            attackModeSpawnController.UpdateButtonsPosition(transform.localPosition.x);
            SelectPlayerSpawn();
            StartMovingToPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        StartCoroutine(StartDelayedButtonPress());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        StopAllCoroutines();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        buttonPressed = false;
    }

    public void StartMovingToPosition()
    {
        mouseLook.StartMovingToPosition(positionToMoveTo);
    }

    private void SelectPlayerSpawn()
    {
        attackModeSpawnController.UpdatePlayerSpawnElements(spawnNumber, transform.localPosition.x);
    }
}
