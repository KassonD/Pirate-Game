using UnityEngine;

public class dialogueArea : MonoBehaviour
{
    // Variables
    public dialogueController dialogueScript;
    public Dialogue dialogueObj;

    private bool inRange = false;
    private bool interactable = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E) && interactable)
        {
            dialogueScript.StartDialogue(dialogueObj.dialogue, this);
            interactable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            inRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            inRange = false;
    }

    public void EnableInteraction()
    {
        interactable = true;
    }
}