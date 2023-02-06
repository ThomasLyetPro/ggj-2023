using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Test_dialogues_ggj2023;
using TMPro;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    [Header("UI")]
    // Reference to Dialog UI
    [SerializeField]
    GameObject dialogueWidget;
    // Reference to dialogue text
    [SerializeField]
    TMP_Text dialogueText;
    // Reference to speaker
    [SerializeField]
    TMP_Text dialogueSpeaker;
    // Reference to button layout
    [SerializeField]
    RectTransform branchLayoutPanel;
    // Reference to navigation button prefab
    [SerializeField]
    GameObject branchPrefab;
    // Reference to close button prefab
    [SerializeField]
    GameObject closePrefab;
    [SerializeField]
    SoundManager soundManager;

    // To check if we are currently showing the dialog ui interface
    public bool DialogueActive { get; set; }

    private ArticyFlowPlayer flowPlayer;

    void Start()
    {
        flowPlayer = GetComponent<ArticyFlowPlayer>();
        DialogueActive = true;
        dialogueWidget.SetActive(DialogueActive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDialogueBox();
        }
    }

    public void StartDialogue(IArticyObject aObject)
    {
        DialogueActive = true;
        dialogueWidget.SetActive(DialogueActive);
        flowPlayer.StartOn = aObject;
    }

    public void CloseDialogueBox()
    {
        DialogueActive = false;
        dialogueWidget.SetActive(DialogueActive);
        // Completely process current object before we end dialogue
        flowPlayer.FinishCurrentPausedObject();
    }

    // This is called every time the flow player reaches an object of interest
    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        //Clear data
        dialogueText.text = string.Empty;
        dialogueSpeaker.text = string.Empty;

        // If we paused on an object that has a "Text" property fetch this text and present it
        var objectWithText = aObject as IObjectWithText;
        if (objectWithText != null)
        {
            dialogueText.text = objectWithText.Text;
            dialogueText.text = Regex.Replace(dialogueText.text, "<size=\\d+>", "");
            dialogueText.text = Regex.Replace(dialogueText.text, "</size>", "");
        }

        // If the object has a "Speaker" property try to fetch the speaker
        var objectWithSpeaker = aObject as IObjectWithSpeaker;
        if (objectWithSpeaker != null)
        {
            // If the object has a "Speaker" property, fetch the reference
            // and ensure it is really set to an "Entity" object to get its "DisplayName"
            var speakerEntity = objectWithSpeaker.Speaker as Entity;
            if (speakerEntity != null)
            {
                dialogueSpeaker.text = speakerEntity.DisplayName;
                dialogueSpeaker.text = Regex.Replace(dialogueSpeaker.text, "<size=\\d+>", "");
                dialogueSpeaker.text = Regex.Replace(dialogueSpeaker.text, "</size>", "");
            }
        }

    }

    // Called every time the flow player encounters multiple branches,
    // or is paused on a node and wants to tell us how to continue
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        // Destroy buttons from previous use, will create new ones here
        ClearAllBranches();

        // Check if any branch leads to a DialogueFragment target
        // If so, the dialogue is not yet finished
        bool dialogueIsFinished = true;
        foreach (var branch in aBranches)
        {
            if (branch.Target is IDialogueFragment)
            {
                dialogueIsFinished = false;
                break;
            }
        }

        if (!dialogueIsFinished)
        {
            // If we have branches, create a button for each of them
            foreach (var branch in aBranches)
            {
                // Instantiate a button in the Dialogue UI
                GameObject nextbtn = Instantiate(branchPrefab, branchLayoutPanel);
                // Let the BranchChoice component fill the button content
                nextbtn.GetComponent<BranchChoice>().AssignBranch(flowPlayer, branch);
                nextbtn.GetComponent<Button>().onClick.AddListener(soundManager.Launch_Clic);
            }

            // Dialogue is finished, instantiate a close button
            GameObject btn = Instantiate(closePrefab, branchLayoutPanel);
            // Clicking this button will close the Dialogue UI
            var btnComp = btn.GetComponent<Button>();
            btnComp.onClick.AddListener(soundManager.Launch_Clic);
            btnComp.onClick.AddListener(CloseDialogueBox);
        }
        else
        {
            // Dialogue is finished, instantiate a close button
            GameObject btn = Instantiate(closePrefab, branchLayoutPanel);
            // Clicking this button will close the Dialogue UI
            var btnComp = btn.GetComponent<Button>();
            btnComp.onClick.AddListener(soundManager.Launch_Clic);
            btnComp.onClick.AddListener(CloseDialogueBox);
        }
    }

    // Delete buttons from previous branches
    void ClearAllBranches()
    {
        foreach (Transform child in branchLayoutPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveTutoFlag()
    {
        var GGJ_Variables = Articy.Test_dialogues_ggj2023.GlobalVariables.ArticyGlobalVariables.Default.GGJ_Variables;
        GGJ_Variables.treeTutoIsRead = true;
        GGJ_Variables.phasesTutoIsRead = true;
        GGJ_Variables.tilesTutoIsRead = true;
        GGJ_Variables.introTutoIsRead = true;
        GGJ_Variables.jadeTutoIsRead = true;
        GGJ_Variables.conditionsTutoIsRead = true;
    }
}
