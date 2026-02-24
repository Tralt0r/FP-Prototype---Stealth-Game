using UnityEngine;
using TMPro;

public class CS_PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 3f;
    public Transform cameraTransform;

    [Header("UI")]
    public TextMeshProUGUI interactText;

    private void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactText.text = "Press E to interact";
                interactText.gameObject.SetActive(true);
                return;
            }
        }

        interactText.gameObject.SetActive(false);
    }

    void TryInteract()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}