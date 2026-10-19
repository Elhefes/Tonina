using UnityEngine;

public class CamazoTextLines : MonoBehaviour
{
    public TextSubject camazoTextSubject;

    public string[] evo1Lines;
    public string[] evo2Lines;
    public string[] evo3Lines;
    public string[] evo4Lines;

    private string BonesNeededText()
    {
        return "Need " + "x" + " more bones to chew on...";
    }

    public void UpdateCamazoLines()
    {

    }
}
