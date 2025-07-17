using UnityEngine;

public class IntroEnemyDeathEvent : MonoBehaviour
{
    public IntroSceneController introSceneController;

    private void OnDestroy()
    {
        introSceneController.TryToMoveToNextEvent();
    }
}
