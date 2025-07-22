using UnityEngine;
using UnityEngine.InputSystem;

public class FireAlarmTrigger : MonoBehaviour
{
    public InputActionReference interactAction; // Drag your "Interact" action here in the Inspector

    private bool playerInRange = false;
    private AudioSource alarmSound;

    private void Start()
    {
        alarmSound = GetComponent<AudioSource>();

        if (interactAction != null)
            interactAction.action.performed += OnInteract;
    }

    private void OnDestroy()
    {
        if (interactAction != null)
            interactAction.action.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && !alarmSound.isPlaying)
        {
            alarmSound.Play();
            Debug.Log("Fire alarm activated!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
