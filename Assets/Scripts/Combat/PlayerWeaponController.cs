using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Gun playerGun;

    private void Awake()
    {
        UIManager.Instance.UpdatePlayerAmmo(playerGun.AmmoLeft);
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            playerGun.Fire(LayerMask.GetMask("Enemy"));
            UIManager.Instance.UpdatePlayerAmmo(playerGun.AmmoLeft);
        }
    }
}
