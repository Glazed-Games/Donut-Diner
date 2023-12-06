using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;
    [SerializeField] private AudioClip voiceLine;

    public string[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0;
    
    public Response[] Responses => responses;
}

[System.Serializable]
public class Response
{
    [Header("Option is the text for optional responses \nand responseDialogue is the full body of the the response")]
    [SerializeField] private string option;
    [SerializeField] [TextArea] private string responeDialogue;
    [SerializeField] private DialogueObject nextDialogue;
    [SerializeField] private AudioClip voiceLine;

    public string Option => option;
    public string Dialogue => responeDialogue;


}