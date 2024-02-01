using UnityEngine;

public class BuildingRoof : MonoBehaviour
{
    public MeshRenderer[] roofMeshRenderers;

    public void MakeRoofInvisible()
    {
        foreach (var obj in roofMeshRenderers)
        {
            obj.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    public void MakeRoofVisible()
    {
        foreach (var obj in roofMeshRenderers)
        {
            obj.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
