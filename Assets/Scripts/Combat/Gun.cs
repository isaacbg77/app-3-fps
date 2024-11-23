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

    private int ammo = 0;
    private int clipAmmo = 0;
    public int AmmoLeft => ammo;
    public int ClipAmmoLeft => clipAmmo;

    private Animator anim;
    private ParticleSystem muzzleFlash;
    private bool canFire = true;

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
        ReloadClip();
    }
    
    public void AddAmmo(int amount)
    {
        if (amount < 0) return;
        ammo += amount;
    }

    public void ReloadClip()
    {
        canFire = false;
        StartCoroutine(ReloadWithDelay());
    }
    
    private IEnumerator ReloadWithDelay()
    {
        if (ammo > 0 && clipAmmo < clipSize)
        {
            anim.Play("Reload");

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
        canFire = true;
    }

    public void Fire(LayerMask targetLayers)
    {
        if (canFire && clipAmmo > 0)
        {
            StartCoroutine(FireWithDelay(targetLayers));
            canFire = false;
        }
    }

    private IEnumerator FireWithDelay(LayerMask targetLayers)
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.cyan, 5f);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range, targetLayers))
        {
            if (hit.transform.gameObject.TryGetComponent(out Health health))
            {
                health.DecreaseHealth(damage);
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
                {
                    UIManager.Instance.UpdatePlayerHealth(health.CurrentHealth);
                }
            }
        }

        clipAmmo--;
        anim.Play("Fire");
        muzzleFlash.Play();

        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }
}
