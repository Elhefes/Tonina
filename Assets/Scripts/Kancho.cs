using UnityEngine;

public class Kancho : Placeable
{
    public int snakeDamage;
    private bool snakeOnCooldown;
    public GameObject snakeRotator;
    public GameObject target;
    public Animator snakeAnimator;
    public AudioSource soundPlayer;

    private void Start()
    {
        target = null;
    }

    void Update()
    {
        if (snakeOnCooldown) return;
        if (target == null) return;

        if (Vector3.Distance(transform.position, target.transform.position) <= 2f)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, snakeRotator.transform.position);
            if (target.transform.position.x < snakeRotator.transform.position.x)
            {
                angle *= -1;
            }
            snakeRotator.transform.rotation = Quaternion.Euler(-90f, 0f, -angle + 180f);
        }
        if (target != null && !snakeOnCooldown && Vector3.Distance(transform.position, target.transform.position) <= 1.75f) SnakeAttack(target);
    }

    void SnakeAttack(GameObject obj)
    {
        snakeAnimator.SetTrigger("SnakeAttack");
        soundPlayer.PlayOneShot(soundPlayer.clip, PlayerPrefs.GetFloat("soundVolume", 0.5f));
        obj.GetComponent<Enemy>()?.TakeDamage(snakeDamage);
        snakeOnCooldown = true;
        target = null;
    }

    public void EndCooldown()
    {
        snakeOnCooldown = false;
    }

    public void HandleCollision(GameObject obj)
    {
        if (obj.CompareTag("Enemy"))
        {
            if (!snakeOnCooldown) target = obj;
        }
    }
}
