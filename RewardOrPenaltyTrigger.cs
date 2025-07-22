using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RewardOrPenaltyTrigger : MonoBehaviour
{
    public enum TriggerType { Reward, Penalty }
    public TriggerType triggerType;
    public InputActionReference interactAction;
    public GameObject feedbackUI; // UI with CanvasGroup
    public float timeAdjustment = 10f;
    public float fadeDuration = 1.5f;
    public float displayTime = 1.5f;

    private bool playerInRange = false;
    private bool hasBeenUsed = false;
    private TimerManager timerManager;

    private void Start()
    {
        if (interactAction != null)
            interactAction.action.performed += OnInteract;

        timerManager = FindObjectOfType<TimerManager>();
    }

    private void OnDestroy()
    {
        if (interactAction != null)
            interactAction.action.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && !hasBeenUsed)
        {
            hasBeenUsed = true;

            if (timerManager != null)
                timerManager.AdjustTime(triggerType == TriggerType.Reward ? -timeAdjustment : timeAdjustment);

            if (feedbackUI != null)
                StartCoroutine(FadeFeedback(feedbackUI));

            Debug.Log(triggerType == TriggerType.Reward ? "Reward triggered!" : "Penalty triggered!");
        }
    }

    private IEnumerator FadeFeedback(GameObject ui)
    {
        ui.SetActive(true);
        CanvasGroup canvasGroup = ui.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup missing on feedback UI.");
            yield break;
        }

        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        ui.SetActive(false);
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
