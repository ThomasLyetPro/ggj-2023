using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject SFX_Add_block;
    [SerializeField] GameObject SFX_AmeDed;
    [SerializeField] GameObject SFX_AmeVictory;
    [SerializeField] GameObject SFX_BacktoGame;
    [SerializeField] GameObject SFX_BuyBlock;
    [SerializeField] GameObject SFX_Clic;
    [SerializeField] GameObject SFX_Pause;
    [SerializeField] GameObject SFX_Play;
    [SerializeField] GameObject SFX_StartTurn;
    [SerializeField] GameObject SFX_TurnBlock;

    public void Launch_Add_block(Vector3 position) { LaunchSFX(SFX_Add_block, position); }
    public void Launch_AmeDed(Vector3 position) { LaunchSFX(SFX_AmeDed, position); }
    public void Launch_AmeVictory(Vector3 position) { LaunchSFX(SFX_AmeVictory, position); }
    public void Launch_BacktoGame() { LaunchSFX(SFX_BacktoGame, Camera.main.transform.position); }
    public void Launch_BuyBlock(Vector3 position) { LaunchSFX(SFX_BuyBlock, position); }
    public void Launch_Clic() { LaunchSFX(SFX_Clic, Camera.main.transform.position); }
    public void Launch_Pause() { LaunchSFX(SFX_Pause, Camera.main.transform.position); }
    public void Launch_Play() { LaunchSFX(SFX_Play, Camera.main.transform.position); }
    public void Launch_StartTurn() { LaunchSFX(SFX_StartTurn, Camera.main.transform.position, false); }
    public void Launch_TurnBlock() { LaunchSFX(SFX_TurnBlock, Camera.main.transform.position); }

    // Start is called before the first frame update
    public void LaunchSFX(GameObject sfxPrefab, Vector3 position, bool destroy = true)
    {
        position.z = -10;
        var newPosition = Camera.main.transform.position + ((position - Camera.main.transform.position) / 2);
        GameObject instance = Instantiate(sfxPrefab, newPosition, Quaternion.identity);
        if(destroy)
            Destroy(instance, 4f);
    }
}
