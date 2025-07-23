using UnityEngine;

public class IntroMovingCamera : MonoBehaviour
{
    public Animator introCameraAnimator;
    public LayerMask normalLayerMask;
    public LayerMask terrainLayerMask;
    public IntroSceneController introSceneController;
    private Camera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    public void SetBoolTrue(string boolName)
    {
        introCameraAnimator.SetBool(boolName, true);
    }

    public void EnablePlayerPivot() { introSceneController.EnableOnlyBattleModeObjects(); }

    public void IntroPopUpTexts() { introSceneController.introHUD_Controller.popUpTexts.SetActive(true); }
    public void EnableBattlefieldWarriors() { introSceneController.EnableBattlefieldWarriors(true); }

    // Placeable buildings are "built" on the battlefield
    public void BuildFillOkill() { introSceneController.BuildBuildingsOnBattlefield(0); }
    public void BuildKancho() { introSceneController.BuildBuildingsOnBattlefield(1); }
    public void BuildFences() { introSceneController.BuildBuildingsOnBattlefield(2); }
    public void BuildTower() { introSceneController.BuildBuildingsOnBattlefield(3); }

    public void FinalFade() { introSceneController.StartFinalFade(); }
    public void EndTexts() { introSceneController.StartEndTexts(); }

    public void SwitchToTerrainLayerMask() { camera.cullingMask = terrainLayerMask; }
    public void SwitchToNormalLayerMask() { camera.cullingMask = normalLayerMask; }
}
