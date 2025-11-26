using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;            
    [TextArea] public string[] resultLines;
    
    public DialogueEventType eventType;  
}

public enum DialogueEventType
{
    None,          
    OpenShop,   
    GiveItem, 
    StartBattle,
    CustomEvent
}