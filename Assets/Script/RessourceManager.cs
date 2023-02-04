using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RessourceManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text lostSoulText;

    [SerializeField]
    TMP_Text jadeText;
    int jade = 0;

    void AddJade(int i)
    {
        jade -= i;
        UpdateUI();
    }

    void RemoveJade(int i)
    {
        jade -= i;
        UpdateUI();
    }


    [SerializeField]
    TMP_Text savedSoulText;
    int savedSoul = 0;

    void AddSavedSoul(int i)
    {
        savedSoul += i;
        UpdateUI();
    }

    void RemoveSavedSoul(int i)
    {
        jade -= i;
        UpdateUI();
    }

    int lostSoul = 0;

    void AddLostSoul(int i)
    {
        lostSoul -= i;
        UpdateUI();
    }

    void RemoveLostSoul(int i)
    {
        lostSoul -= i;
        UpdateUI();
    }




    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        jadeText.text = jade.ToString();
        savedSoulText.text = savedSoul.ToString();
        lostSoulText.text = lostSoul.ToString();
    }

}
