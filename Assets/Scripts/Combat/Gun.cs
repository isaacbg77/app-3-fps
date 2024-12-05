using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private int startingAmmo;
    [SerializeField] private int clipSize;
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireDelay = 1f;
    [SerializeField] private float reloadDelay = 1f;
    [Space]

    [SerializeField] private AudioSource[] fireSounds;
    [SerializeField] private AudioSource reloadSound;

    private int ammo = 0;
    private int clipAmmo = 0;

    private Animator anim;
    private ParticleSystem muzzleFlash;
    private bool canFire = true;
    private bool reloading = false;
    
    public int AmmoLeft => ammo;
    public int ClipAmmoLeft => clipAmmo;
    public bool Reloading => reloading;

    private void Awake()
    {
        if (TryGetComponent(out Animator animator))
        {
            anim = animator;
        }
        else
            Debug.LogError("Gun is missing an animator!");

        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        if (muzzleFlash == null)
            Debug.LogError("Gun is missing a particle system!");
        
        ammo = startingAmmo;
        ReloadClip(false);
    }
    
    public void AddAmmo(int amount)
    {
        if (amount < 0) return;

        ammo += amount;
        reloadSound.Play();
    }

    public void ReloadClip(bool playAnimation)
    {
        reloading = true;
        StartCoroutine(ReloadWithDelay(playAnimation));
    }
    
    private IEnumerator ReloadWithDelay(bool playAnimation)
    {
        if (ammo > 0 && clipAmmo < clipSize)
        {
            if (playAnimation)
            {
                anim.Play("Reload");
                reloadSound.Play();
            }

            int diff = clipSize - clipAmmo;
            if (ammo < diff)
            {
                clipAmmo += ammo;
                ammo = 0;
            }
            else
            {
                clipAmmo += diff;
                ammo -= diff;
            }

        }

        yield return new WaitForSeconds(reloadDelay);
        reloading = false;
    }

    public void Fire()
    {
        if (canFire && !reloading && clipAmmo > 0)
        {
            StartCoroutine(FireWithDelay());
            canFire = false;
        }
    }

    private IEnumerator FireWithDelay()
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.cyan, 5f);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
        {
            //Debug.Log(hit.transform.gameObject);
            if (hit.transform.gameObject.TryGetComponent(out Health health))
            {
                if (health is PlayerHealth playerHealth)
                {
                    playerHealth.UpdateUI();
                }
                else if (health is EnemyHealth enemyHealth)
                {
                    // Get enemy to react if they are hit
                    if (enemyHealth.TryGetComponent(out AIBehaviour ai) && ai.GetState() != AIState.Chase)
                    {
                        ai.ChangeState(AIState.Chase);
                    }
                    // Hit marker
                    if (enemyHealth.CurrentHealth > 0)
                        UIManager.Instance.ShowHitMarker();
                }
                
                if (health.enabled)
                    health.DecreaseHealth(damage);
            }
        }

        clipAmmo--;
        anim.Play("Fire");
        muzzleFlash.Play();
        fireSounds[Random.Range(0, fireSounds.Length)].Play();

        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }
}
