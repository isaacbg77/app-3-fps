using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Interactable
{
    [SerializeField] private Health target;

    public override void Interact()
    {
        // If target is eliminated, end the level
        if (target.CurrentHealth <= 0)
        {
            GameManager.Instance.EndGame(true);
        }
    }
}
