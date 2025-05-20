using System;
using UnityEngine;
using Naninovel;

public class MiniGameController : MonoBehaviour
{
    [SerializeField] private GameObject miniGameUI;

    

    private void OnEnable()
    {
        MiniGameEvents.OnMiniGameStart.AddListener(ShowMiniGame);
        MiniGameEvents.OnMiniGameEnd.AddListener(HideMiniGame);
    }

    private void OnDisable()
    {
        MiniGameEvents.OnMiniGameStart.RemoveListener(ShowMiniGame);
        MiniGameEvents.OnMiniGameEnd.RemoveListener(HideMiniGame);
    }

    private void ShowMiniGame() => miniGameUI.SetActive(true);
    private void HideMiniGame() => miniGameUI.SetActive(false);
}