using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCInteract : MonoBehaviour, IInteractable
{
    [TextArea] public string[] dialogueLines;

    public string GetPromptMessage()
    {
        return "按 E 與 NPC 對話";
    }

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(dialogueLines);
    }
}
