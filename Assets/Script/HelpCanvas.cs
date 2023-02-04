using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject dialogBox;

    void Update()
    {
        if (dialogBox.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }
}
