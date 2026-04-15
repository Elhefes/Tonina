using UnityEngine;

public class IntroEndTexts : MonoBehaviour
{
    public void EndIntro()
    {
        SceneChangingManager.Instance.LoadScene("Jadea");
    }
}
