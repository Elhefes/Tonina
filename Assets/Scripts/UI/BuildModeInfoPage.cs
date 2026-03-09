using UnityEngine;
using UnityEngine.UI;

public class BuildModeInfoPage : MonoBehaviour
{
    public Animator animator;
    public GameObject[] descriptions;
    public Button infoButton;
    
    public void EnablePlaceableDescription(int index)
    {
        foreach (GameObject desc in descriptions) desc.SetActive(false);
        descriptions[index].SetActive(true);
    }

    public void ClosePage()
    {
        animator.SetTrigger("Close");
    }

    public void SetButtonToInteractable()
    {
        // When closing info page
        infoButton.interactable = true;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        infoButton.interactable = false;
        animator.SetTrigger("Open");
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
