using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogueInteract : MonoBehaviour, IInteractable
{
    [TextArea] public string[] dialogueLines;
    
    public DialogueChoice[] choices;

    public string GetPromptMessage()
    {
        return "按 E 開啟對話";
    }

    public void Interact()
    {
        RunDialogueEvent();
    }

    async void RunDialogueEvent()
    {
        // Step 1：先跑對話（若有）
        if (dialogueLines != null && dialogueLines.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(dialogueLines);
            while (DialogueManager.IsDialogueActive)
                await System.Threading.Tasks.Task.Yield();
        }

        // Step 2：若沒有選項就結束
        if (choices == null || choices.Length == 0)
            return;

        // Step 3：顯示選項
        int choice = await DialogueManager.Instance.ShowChoices(choices);

        // Step 4：處理選項結果
        DialogueChoice selected = choices[choice];

        // 若該選項有「選擇後的對話」
        if (selected.resultLines != null && selected.resultLines.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(selected.resultLines);
        }

        // Step 5：依照 eventType 執行事件
        switch (selected.eventType)
        {
            case DialogueEventType.OpenShop:
                if (TryGetComponent(out ShopInteract shop))
                    ShopManager.Instance.OpenShop(shop.itemPool);
                break;

            case DialogueEventType.GiveItem:
                Debug.Log("NPC 給玩家道具（未實作）");
                break;

            case DialogueEventType.StartBattle:
                Debug.Log("觸發戰鬥（未實作）");
                break;

            case DialogueEventType.CustomEvent:
                Debug.Log("自訂事件（未實作）");
                break;
        }
    }
}