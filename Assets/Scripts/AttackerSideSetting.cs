using UnityEngine;

public class AttackerSideSetting : MonoBehaviour
{
    public static AttackerSideSetting Instance;
    public bool enemyIsDefender;

    private void Awake()
    {
        Instance = this;
    }
}
