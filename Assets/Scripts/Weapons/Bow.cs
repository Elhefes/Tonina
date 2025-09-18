using System.Collections;
using UnityEngine;

public class Bow : Projectile
{
    public Arrow arrow;
    public Rigidbody arrowRb;

    public float arrowPrimeTime;

    public AudioClip shootSound;

    public CreatureMovement creatureMovement;

    private bool primingArrow;
    private Coroutine primingCoroutine;

    private void OnEnable()
    {
        primingArrow = false;
        arrow.gameObject.SetActive(true);
    }

    public override void Attack(Animator animator)
    {
        if (!primingArrow)
        {
            primingCoroutine = StartCoroutine("StartAiming");
            animator.SetTrigger("BowAttack");
        }
    }

    private IEnumerator StartAiming()
    {
        primingArrow = true;
        yield return new WaitForSeconds(arrowPrimeTime);
        if (creatureMovement.target != null)
        {
            ShootArrow();
            if (shootSound != null) soundPlayer.PlayOneShot(shootSound, PlayerPrefs.GetFloat("soundVolume", 0.5f));
            yield return new WaitForSeconds(attackCooldown - arrowPrimeTime - 0.05f); // Short buffer time needed to prevent timing overlap bugs
            arrow.gameObject.SetActive(true);
        }
        primingArrow = false;
    }

    void ShootArrow()
    {
        arrow.directorComponent.gameObject.SetActive(true);
        arrow.canHit = true;

        if (!infinite) quantity--;
        if (quantity < 1) notAvailable = true;
        if (weaponWheelQuantityTMP != null) weaponWheelQuantityTMP.text = quantity.ToString();

        arrowRb.constraints = RigidbodyConstraints.None;

        Instantiate(arrow.gameObject, shootingPoint.position, shootingPoint.rotation);
        arrowRb.constraints = RigidbodyConstraints.FreezeAll;
        arrow.directorComponent.gameObject.SetActive(false);
        arrow.canHit = false;
        arrow.gameObject.SetActive(false);
    }
}
