using UnityEngine;
using UnityEngine.SceneManagement;
using Articy.Unity;
//using Articy.UnityImporterTutorial.GlobalVariables;

public class PlayerController : MonoBehaviour
{
    private float speed = 15f;
    
    private Rigidbody playerRB;
    private DialogueManager dialogueManager;
    private ArticyObject availableDialogue;
 
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        PlayerInteraction();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    // Simple player movement
    void PlayerMovement()
    {
        // Remove movement control while in dialogue
        if (dialogueManager.DialogueActive)
            return;

        playerRB.velocity = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
    }

    // All interactions and key inputs player can use
    void PlayerInteraction()
    {
        // Key option to start dialogue when near NPC
        if (Input.GetKeyDown(KeyCode.Space) && availableDialogue)
        {
            dialogueManager.StartDialogue(availableDialogue);
        }

        // Key option to abort dialogue
        if (dialogueManager.DialogueActive && Input.GetKeyDown(KeyCode.Escape))
        {
            dialogueManager.CloseDialogueBox();
        }

        // Key option to reset entire scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    // Simple scene restart for testing purposes
    void RestartScene()
    {
        //ArticyGlobalVariables.Default.ResetVariables();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Trigger Enter/Exit used to determine if interaction with NPC is possible
    void OnTriggerEnter(Collider aOther)
    {
        var articyReferenceComp = aOther.GetComponent<ArticyReference>();
        if (articyReferenceComp)
        {
            availableDialogue = articyReferenceComp.reference.GetObject();
        }
    }

    void OnTriggerExit(Collider aOther)
    {
        if (aOther.GetComponent<ArticyReference>() != null)
        {
            availableDialogue = null;
        }
    }
}
