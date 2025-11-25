using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [Header("互動提示 UI")]
    public Text promptText;

    private IInteractable nearbyTarget;

    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Disable();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (nearbyTarget != null && !DialogueManager.IsDialogueActive)
        {
            nearbyTarget.Interact();
        }
    }

    void Start()
    {
        if (promptText != null)
            promptText.text = "";
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            nearbyTarget = interactable;
            if (promptText != null)
                promptText.text = interactable.GetPromptMessage();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable == nearbyTarget)
        {
            nearbyTarget = null;
            if (promptText != null)
                promptText.text = "";
        }
    }
}
