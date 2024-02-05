using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CanvasType
{
    PauseMenu,
    GameUI,
    GameOver,
    Shop
}

public class CanvasManager : MonoBehaviour
{
    CanvasManager instance;
    CanvasController[] canvasControllers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start() {
        GameManager.instance.OnStateChange.AddListener(OnStateChange);

        canvasControllers = GetComponentsInChildren<CanvasController>();
    }

    void OnStateChange(GameState oldState, GameState newState)
    {
        switch(newState) {
            case GameState.MainMenu:
                SetCanvas(CanvasType.PauseMenu);
                break;
            case GameState.Paused:
                SetCanvas(CanvasType.PauseMenu);
                break;
            case GameState.GameOver:
                SetCanvas(CanvasType.GameOver);
                break;
            case GameState.Playing:
                SetCanvas(CanvasType.GameUI);
                break;
            case GameState.Shop:
                SetCanvas(CanvasType.Shop);
                break;
        }
    }

    void SetCanvas(CanvasType canvasType) {
        foreach(CanvasController canvasController in canvasControllers) {
            canvasController.gameObject.SetActive(canvasController.CanvasType == canvasType);
        }
    }
}
