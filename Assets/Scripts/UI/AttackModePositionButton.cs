using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackModePositionButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector3 positionToMoveTo;
    public MouseLook mouseLook;
    public AttackModeSpawnController attackModeSpawnController;

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

        // Detecting double click
        clicked++;

        if (clicked == 1)
            clickTime = Time.time;

        if (clicked > 1 && Time.time - clickTime < clickDelay)
        {
            // Double click detected
            SelectPlayerSpawn();
            clicked = 0;
            clickTime = 0;
        }
        else if (clicked > 2 || Time.time - clickTime > 1)
            clicked = 0;
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
