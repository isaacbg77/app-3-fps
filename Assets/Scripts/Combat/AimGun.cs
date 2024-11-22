using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimGun : MonoBehaviour
{
    [SerializeField] private Transform defaultPosition;
    [SerializeField] private Transform aimPosition;

    [SerializeField] private float aimSpeed = 10f;

    private bool isAiming = false;

    void Update()
    {
        if (Input.GetButtonUp("Fire2")) { isAiming = !isAiming; }

        if (isAiming && transform.localPosition != aimPosition.localPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition.localPosition, aimSpeed * Time.deltaTime);
        }
        else if (!isAiming && transform.localPosition != defaultPosition.localPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition.localPosition, aimSpeed * Time.deltaTime);
        }
    }
}
