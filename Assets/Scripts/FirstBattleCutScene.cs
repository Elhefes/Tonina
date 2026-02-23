using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FirstBattleCutScene : MonoBehaviour
{
    public Animator cameraAnimator;
    public MouseLook mouseLook;
    public GameObject blackFader;
    public GameObject overworldUI;
    public GameObject clickBlocker;
    public GameObject textBox;
    public NavMeshAgent[] cutSceneEnemies;

    public void StartFirstBattleCutScene()
    {
        StartCoroutine(BattleCutScene());
    }

    IEnumerator BattleCutScene()
    {
        mouseLook.CameraOnPlayerOff();
        mouseLook.inCutScene = true;
        overworldUI.SetActive(false);
        blackFader.SetActive(true);
        clickBlocker.SetActive(true);

        yield return new WaitForSeconds(0.33f);

        foreach (NavMeshAgent agent in cutSceneEnemies) agent.gameObject.SetActive(true);
        mouseLook.notCastingRays = true;
        cameraAnimator.SetTrigger("FirstBattle");

        yield return new WaitForSeconds(5f);

        foreach (NavMeshAgent agent in cutSceneEnemies) agent.speed = 3.5f;

        yield return new WaitForSeconds(10f);

        foreach (NavMeshAgent agent in cutSceneEnemies) agent.gameObject.SetActive(false);

        mouseLook.ToggleCameraOnPlayer();
        mouseLook.inCutScene = false;
        overworldUI.SetActive(true);
        clickBlocker.SetActive(false);
        mouseLook.notCastingRays = false;
    }
}
