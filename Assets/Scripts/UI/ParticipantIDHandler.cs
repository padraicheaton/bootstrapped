using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParticipantIDHandler : ModalWindow
{
    [Header("References")]
    [SerializeField] private TMP_InputField inputTxt;

    public void OnSubmitBtnPressed()
    {
        ServiceLocator.instance.GetService<GameManager>().SetParticipantID(inputTxt.text);

        Hide();
    }
}
