using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;

    private string[] currentLines;
    private int index;
    private bool isTyping;
    
    private InputSystem_Actions inputActions;

    public static bool IsDialogueActive;

    private float endDelay = 0.2f;
    
    private CanvasGroup canvasGroup;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        inputActions = new InputSystem_Actions();
        canvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
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

    public void StartDialogue(string[] lines)
    {
        IsDialogueActive = true;
        currentLines = lines;
        index = 0;
        
        canvasGroup.alpha = 1;
        dialoguePanel.SetActive(true);
        
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in currentLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void Update()
    {
        
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (!dialoguePanel.activeSelf) return;

        if (isTyping)  // 若還在打字，直接顯示完整句
        {
            StopAllCoroutines();
            dialogueText.text = currentLines[index];
            isTyping = false;
        }
        else NextLine();
    }

    void NextLine()
    {
        index++;
        if (index < currentLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        StartCoroutine(EndDialogueCoroutine());
    }

    IEnumerator EndDialogueCoroutine()
    {
        float t = 0;
        const float startAlpha = 1f;

        while (t < endDelay)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / endDelay);
            yield return null;
        }

        dialoguePanel.SetActive(false);
        IsDialogueActive = false;
    }
}
