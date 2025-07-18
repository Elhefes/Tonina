using UnityEngine;

public class IntroEnemyDeathEvent : MonoBehaviour
{
    public IntroSceneController introSceneController;
    public int eventIndex;

    private void OnDestroy()
    {
        introSceneController.TryToMoveToNextEvent(eventIndex);
    }
}
