using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour , IInteractable
{
    public QuestObject questObject;

    public Dialogue readyDialogue;
    public Dialogue acceptedDialogue;
    public Dialogue completedDialogue;

    bool isStartQuestDialogue = false;

    GameObject interactGO = null;
    void Start()
    {
        QuestManager.Instance.OnCompletedQuest += OnCompletedQuest;
    }
    public float distance = 2.0f;
    public float Distance => distance;

    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if (calcDistance < distance)
        {
            return;
        }
        if (isStartQuestDialogue)
        {
            return;
        }
        this.interactGO = other;

        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartQuestDialogue = true;

        if(questObject.status == QuestStatus.None)
        {
            DialogueManager.Instance.StartDialogue(readyDialogue);
            questObject.status = QuestStatus.Accepted;
        }
        else if(questObject.status ==QuestStatus.Accepted)
        {
            DialogueManager.Instance.StartDialogue(acceptedDialogue);
        }
        else if(questObject.status == QuestStatus.Completed)
        {
            DialogueManager.Instance.StartDialogue(completedDialogue);
            questObject.status = QuestStatus.Rewareded;
        }
    }
    public void StopInteract(GameObject other)
    {
        isStartQuestDialogue = false;

    }
    private void OnEndDialogue()
    {
        StopInteract(interactGO);
    }
    private void OnCompletedQuest(QuestObject questObject)
    {
        if(questObject.data.id == this.questObject.data.id && questObject.status == QuestStatus.Completed)
        {

        }
    }
}
