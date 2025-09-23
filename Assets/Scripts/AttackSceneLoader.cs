using UnityEngine;

public class AttackSceneLoader : MonoBehaviour
{
    public void StartLoadingAttackScene(int pyramidNumber)
    {
        if (pyramidNumber == 1) SceneChangingManager.Instance.LoadScene("Attack1");
    }
}
