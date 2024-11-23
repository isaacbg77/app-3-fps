using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Gun playerGun;

    private void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            playerGun.Fire(LayerMask.GetMask("Enemy"));
            UpdateUI();
        }
        else if (Input.GetButtonUp("Reload"))
        {
            playerGun.ReloadClip();
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdatePlayerAmmo(playerGun.ClipAmmoLeft, playerGun.AmmoLeft);
    }
}
