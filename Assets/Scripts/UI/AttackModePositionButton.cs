using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackModePositionButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Vector3 positionToMoveTo;
    public MouseLook mouseLook;
    public AttackModeSpawnController attackModeSpawnController;

    int spawnNumber;

    float clicked = 0;
    float clickTime = 0;
    float clickDelay = 0.67f;

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

    public void StartMovingToPosition()
    {
        mouseLook.StartMovingToPosition(positionToMoveTo);
    }

    private void SelectPlayerSpawn()
    {
        attackModeSpawnController.UpdatePlayerSpawnElements(spawnNumber, transform.localPosition.x);
    }
}
