using UnityEngine;

public class ExampleInteract : MonoBehaviour, IInteractable
{
    public CS_GameManager gameManager;
    public void Interact()
    {
        Debug.Log("Interacted with the item");
        gameManager.GameWon();
    }
}