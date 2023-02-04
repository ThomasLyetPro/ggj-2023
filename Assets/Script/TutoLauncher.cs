using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoLauncher : MonoBehaviour
{
    [SerializeField]
    DialogueManager dialogueManager;

    public void LaunchTuto()
    {
        dialogueManager.RemoveTutoFlag();
        dialogueManager.StartDialogue(GetComponent<ArticyReference>().reference.GetObject());
    }
}
