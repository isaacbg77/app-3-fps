using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private int ammoAmount = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerWeaponController weaponController))
        {
            Gun playerGun = other.GetComponentInChildren<Gun>();
            if (playerGun != null)
            {
                playerGun.AddAmmo(ammoAmount);
                weaponController.UpdateUI();
                gameObject.SetActive(false);
            }
        }
    }
}
