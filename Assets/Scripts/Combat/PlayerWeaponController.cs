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
        if (GameManager.Instance.IsGamePaused) return;

        if (Input.GetButton("Fire1"))
        {
            playerGun.Fire();
            UpdateUI();
        }
        else if (Input.GetButtonUp("Reload"))
        {
            playerGun.ReloadClip(true);
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        UIManager.Instance.UpdatePlayerAmmo(playerGun.ClipAmmoLeft, playerGun.AmmoLeft);
    }
}
