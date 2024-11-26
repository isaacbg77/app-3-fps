using UnityEngine;

public class PlayerInteractor : Interactor
{
    private void Update()
    {
        if (Input.GetButtonUp("Interact"))
        {
            DoInteract();
        }
    }
}
