using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    string _currentTheme;
    public string currentTheme {
        get { return _currentTheme; }
        set {
            if(_currentTheme != value) {
                AudioManager.instance.Stop(_currentTheme);
                AudioManager.instance.Play(value);
                _currentTheme = value;
            }
        }
    }

    private void Start() {
        GameManager.instance.OnStateChange.AddListener(OnStateChange);
    }

    void OnStateChange(GameState oldState, GameState newState)
    {
        switch(newState) {
            case GameState.MainMenu:
                currentTheme = "MainMenu";
                break;
            case GameState.Paused:
                currentTheme = "PauseMenu";
                break;
            case GameState.GameOver:
                currentTheme = "GameOver";
                break;
        }
    }
}
