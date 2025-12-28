public class TowerBarricade : Barricade
{
    public Tower tower;

    public override void RestoreBarricade()
    {
        health = startingHealth;
        tower.health = tower.startingHealth;
        if (layerToDisappear != null && !layerToDisappear.activeSelf) layerToDisappear.SetActive(true);
    }

    public override void TakeDamage(int damage)
    {
        tower.health -= damage;
        if (tower.health <= tower.startingHealth / 2 && layerToDisappear != null) layerToDisappear.SetActive(false);

        if (tower.health <= 0)
        {
            tower.gameObject.SetActive(false);
        }
        print("Tower health: " + tower.health);
    }
}
