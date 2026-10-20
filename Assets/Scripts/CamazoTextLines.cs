using UnityEngine;

public class CamazoTextLines : MonoBehaviour
{
    public TextSubject camazoTextSubject;
    public Camazo camazo;

    public string[] evo1Lines;
    public string[] evo2Lines;
    public string[] evo3Lines;
    public string[] evo4Lines;

    private string BonesNeededText(int bonesNeeded)
    {
        if (bonesNeeded == 1) return "Need " + bonesNeeded + " more bone to chew on...";
        return "Need " + bonesNeeded + " more bones to chew on...";
    }

    public void UpdateCamazoLines()
    {
        int evolutionStage = GameState.Instance.progressionData.camazoEvolutionStage;

        if (evolutionStage == 0)
        {
            evo1Lines[0] = BonesNeededText(BonesLeft(evolutionStage));
            camazoTextSubject.textLines = (string[])evo1Lines.Clone();
        }
        else if (evolutionStage == 1)
        {
            evo2Lines[0] = BonesNeededText(BonesLeft(evolutionStage));
            camazoTextSubject.textLines = (string[])evo2Lines.Clone();
        }
        else if (evolutionStage == 2)
        {
            evo3Lines[0] = BonesNeededText(BonesLeft(evolutionStage));
            camazoTextSubject.textLines = (string[])evo3Lines.Clone();
        }
        else if (evolutionStage == 3)
        {
            camazoTextSubject.textLines = (string[])evo4Lines.Clone();
        }
    }

    private int BonesLeft(int evolutionStage)
    {
        int bonesEaten = GameState.Instance.progressionData.bonesCamazoHasEaten;
        if (evolutionStage == 0) return camazo.bonesNeededFor1stEvo - bonesEaten;
        else if (evolutionStage == 1) return camazo.bonesNeededFor2ndEvo - bonesEaten;
        else if (evolutionStage == 2) return camazo.bonesNeededFor3rdEvo - bonesEaten;
        return 1;
    }
}
