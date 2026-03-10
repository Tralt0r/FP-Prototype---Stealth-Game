using UnityEngine;
using UnityEngine.Events;

public class ExampleInteract : MonoBehaviour, IInteractable
{
    public CS_GameManager gameManager;
    public UnityEvent onButtonPress;
    public void Interact()
    {
        Debug.Log("Interacted with the item");
        gameManager.GameWon();
        onButtonPress?.Invoke();
    }
}