using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using UnityEngine.UI;
using TMPro;

// This is the UI control for a single option within a dialogue
public class BranchChoice : MonoBehaviour
{
    private Branch branch;
    private ArticyFlowPlayer flowPlayer;
    [SerializeField]
    TMP_Text buttonText;

    // Called when a button is created to represent a single branch
    public void AssignBranch(ArticyFlowPlayer aFlowPlayer, Branch aBranch)
    {
        branch = aBranch;
        flowPlayer = aFlowPlayer;
        IFlowObject target = aBranch.Target;
        buttonText.text = string.Empty;

        // Check if the object has a "MenuText" property.
        // If yes, set the button text to that "MenuText"
        var objectWithMenuText = target as IObjectWithMenuText;
        if (objectWithMenuText != null)
        {
            buttonText.text = objectWithMenuText.MenuText;
        }

        // If there is no text from a "MenuText" property set,
        // we place ">>>" on the button as a "Continue" symbol 
        if (string.IsNullOrEmpty(buttonText.text))
        {
            buttonText.text = "Next";
        }
    }

    // What happens when button is clicked
    public void OnBranchSelected()
    {
        flowPlayer.Play(branch);
    }
}
