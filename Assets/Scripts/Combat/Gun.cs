using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private int startingAmmo;
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireDelay = 1f;

    private int ammo = 0;
    public int AmmoLeft => ammo;

    private Animator anim;
    private bool canFire = true;

    private void Awake()
    {
        ammo = startingAmmo;

        if (TryGetComponent(out Animator animator))
        {
            anim = animator;
        }
        else
            Debug.LogError("Gun is missing an animator!");
    }
    
    public void IncreaseAmmo(int amount)
    {
        if (amount < 0) return;
        ammo += amount;
    }

    public void DecreaseAmmo(int amount)
    {
        if (amount < 0) return;
        ammo -= amount;
    }

    public void Fire(LayerMask targetLayers)
    {
        if (canFire)
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

        DecreaseAmmo(1);
        anim.Play("Fire");

        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }
}
