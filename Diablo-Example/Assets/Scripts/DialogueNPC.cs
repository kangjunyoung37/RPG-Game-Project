using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour , IInteractable
{
    [SerializeField]
    Dialogue dialogue;

    bool isStartDialogue = false;

    GameObject interactGO;


    [SerializeField]
    float distance = 2.0f;

    public float Distance => distance;

    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if(calcDistance -0.5f> distance)
        {
            return;
        }
        if(isStartDialogue)
        {
            return;
        }
        interactGO = other;

        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartDialogue = true;

        DialogueManager.Instance.StartDialogue(dialogue);
    }
    public void StopInteract(GameObject other)
    {
        isStartDialogue = false;

    }
    private void OnEndDialogue()
    {
        StopInteract(interactGO);
    }

}
