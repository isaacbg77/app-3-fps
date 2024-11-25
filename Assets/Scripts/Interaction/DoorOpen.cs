using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : Interactable
{
    [SerializeField] private float delay = 5f;

    private AudioSource fx;
    private Animator anim;
    private bool canOpen = true;

    private void Awake()
    {
        if (TryGetComponent(out AudioSource fx))
        {
            this.fx = fx;
        }
        else
            Debug.LogError("Door missing audio source!");

        if (TryGetComponent(out Animator anim))
        {
            this.anim = anim;
        }
        else
            Debug.LogError("Door missing animator!");
    }

    public override void Interact()
    {
        if (canOpen)
        {
            canOpen = false;
            fx.Play();
            anim.Play("DoorOpen");

            StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(delay);

        fx.Play();
        anim.Play("DoorClose");

        // Wait for animation to finish
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        canOpen = true;
    }
}
