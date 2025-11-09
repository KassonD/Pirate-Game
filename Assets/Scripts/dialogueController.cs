using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class dialogueController : MonoBehaviour
{
    // Variables
    private GameObject content;
    private TextMeshProUGUI dialogueTMP;
    private string[] dialogue;
    private bool isReading = false;
    private bool moveOn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        content = transform.GetChild(0).gameObject;
        dialogueTMP = content.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        content.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isReading && Input.GetKeyDown(KeyCode.Space))
            moveOn = true;
    }

    public void StartDialogue(string[] dialogue, dialogueArea script)
    {
        this.dialogue = dialogue;
        isReading = true;
        content.SetActive(true);
        StartCoroutine(ReadDialogue(script));
    }

    private IEnumerator ReadDialogue(dialogueArea script)
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogueTMP.text = "";

            for (int j = 0; j < dialogue[i].Length; j++)
            {
                if (moveOn)
                {
                    dialogueTMP.text = dialogue[i];
                    moveOn = false;
                    break;
                }

                dialogueTMP.text += dialogue[i][j];

                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitUntil(() => moveOn);
            moveOn = false;
        }

        script.EnableInteraction();
        content.SetActive(false);
    }
}
