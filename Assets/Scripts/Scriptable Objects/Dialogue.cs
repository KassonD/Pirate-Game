using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    // Variables
    public bool oneTime;

    [TextArea(5,10)]
    public string[] dialogue;
}
