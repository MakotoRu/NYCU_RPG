using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;

    [Header("Choices UI")]
    public GameObject choicesPanel;
    public Transform choiceContainer;
    public GameObject choicePrefab;
    
    private System.Threading.Tasks.TaskCompletionSource<int> choiceResult;

    private string[] currentLines;
    private int index;
    private bool isTyping;
    
    private InputSystem_Actions inputActions;
    private CanvasGroup canvasGroup;

    public static bool IsDialogueActive;

    private float endDelay = 0.2f;
    
    private TaskCompletionSource<int> choiceSource;
    private int selectedIndex = 0;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);

        inputActions = new InputSystem_Actions();
        canvasGroup = dialoguePanel.GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Move.performed += OnMove;
    }

    void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Move.performed -= OnMove;
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

        foreach (char c in currentLines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (!dialoguePanel.activeSelf) return;
        
        if (choicesPanel.activeSelf)
        {
            ConfirmChoice();
            return;
        }
        
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentLines[index];
            isTyping = false;
            return;
        }
        
        NextLine();
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
        float t = 0f;
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
    
    public async Task<int> ShowChoices(string[] choices)
    {
        // 清除舊選項
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        selectedIndex = 0;
        choiceSource = new TaskCompletionSource<int>();

        // 建立按鈕
        for (int i = 0; i < choices.Length; i++)
        {
            GameObject go = Instantiate(choicePrefab, choiceContainer);
            var item = go.GetComponentInChildren<TextMeshProUGUI>();
            item.text = choices[i];
        }

        HighlightChoice(0);

        choicesPanel.SetActive(true);

        // 等玩家選
        int result = await choiceSource.Task;

        choicesPanel.SetActive(false);
        return result;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        if (!choicesPanel.activeSelf) return;

        Vector2 input = ctx.ReadValue<Vector2>();

        if (input.y > 0.5f)
            MoveSelection(-1);
        else if (input.y < -0.5f)
            MoveSelection(+1);
    }

    void MoveSelection(int dir)
    {
        int count = choiceContainer.childCount;

        selectedIndex += dir;
        if (selectedIndex < 0) selectedIndex = count - 1;
        if (selectedIndex >= count) selectedIndex = 0;

        HighlightChoice(selectedIndex);
    }

    void HighlightChoice(int index)
    {
        for (int i = 0; i < choiceContainer.childCount; i++)
        {
            var text = choiceContainer.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            text.color = (i == index) ? Color.yellow : Color.white;
        }
    }

    void ConfirmChoice()
    {
        if (choiceSource != null && !choiceSource.Task.IsCompleted)
            choiceSource.SetResult(selectedIndex);
    }
    
    public async System.Threading.Tasks.Task<int> ShowChoices(DialogueChoice[] choices)
    {
        choicesPanel.SetActive(true);
        
        foreach (Transform t in choiceContainer)
            Destroy(t.gameObject);

        choiceResult = new System.Threading.Tasks.TaskCompletionSource<int>();

        for (int i = 0; i < choices.Length; i++)
        {
            int index = i;
            GameObject go = Instantiate(choicePrefab, choiceContainer);
            go.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;

            go.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                choicesPanel.SetActive(false);
                choiceResult.TrySetResult(index);
            });
        }

        return await choiceResult.Task;
    }
}
