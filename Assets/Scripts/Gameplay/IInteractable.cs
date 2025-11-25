public interface IInteractable
{
    string GetPromptMessage(); // 顯示給玩家的提示文字（例如「按E互動」）
    void Interact();           // 玩家互動時呼叫的行為
}