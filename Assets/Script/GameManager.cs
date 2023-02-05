using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using AspectGgj2023.Gameboard;

public class GameManager : MonoBehaviour
{
    public enum GamePhase { Phase1, Phase2, Pause, Defeat, Victory }
    public GamePhase currentGamePhase = GamePhase.Phase1;

    [Header("Game Loop")]
    [SerializeField]
    GameObject tileButtons;
    [SerializeField]
    GameObject startPhaseButton;
    [SerializeField]
    GameObject helpButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TriggerVictory();
        else if (Input.GetKeyDown(KeyCode.M))
            TriggerDefeat();
    }

    private void SetUIButtonsActive(bool active)
    {
        tileButtons.SetActive(active);
        startPhaseButton.SetActive(active);
        helpButton.SetActive(active);
    }

    public void TriggerPhase1()
    {
        currentGamePhase = GamePhase.Phase1;

        helpCanvas.SetActive(false);

        // Enable button
        SetUIButtonsActive(true);
    }

    public void TriggerPhase2()
    {
        currentGamePhase = GamePhase.Phase2;

        // Disable button
        SetUIButtonsActive(false);

        // Trigger soul's journey
        StartCoroutine(DelayTriggerPhase1());
    }

    IEnumerator DelayTriggerPhase1()
    {
        yield return new WaitForSeconds(2);
        TriggerPhase1();
    }

    [SerializeField]
    GameObject helpCanvas;

    public void TriggerPause()
    {
        currentGamePhase = GamePhase.Pause;

        helpCanvas.SetActive(true);
    }

    [Header("Game Over")]
    [SerializeField]
    GameObject informationUI;

    [SerializeField]
    GameObject gameOver;
    [SerializeField]
    GameObject victory;
    [SerializeField]
    GameObject defeat;

    public void TriggerVictory()
    {
        currentGamePhase = GamePhase.Victory;

        // Disable button
        SetUIButtonsActive(false);

        // Disable UI
        informationUI.SetActive(false);

        // Show victory screen
        gameOver.SetActive(true);
        victory.SetActive(true);
    }

    public void TriggerDefeat()
    {
        currentGamePhase = GamePhase.Defeat;

        // Disable Button
        SetUIButtonsActive(false);

        // Disable UI
        informationUI.SetActive(false);

        // Show victory screen
        gameOver.SetActive(true);
        defeat.SetActive(true);

    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);

        DestinationTreeTile.ResetWinningCondition();
    }
}
