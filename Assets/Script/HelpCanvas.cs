using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject dialogBox;

    void Update()
    {
        // TODO: In the end we can remove the test on dialogBox but I need to test smthing before it gets fully integrated
        if (dialogBox && dialogBox.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }
}
