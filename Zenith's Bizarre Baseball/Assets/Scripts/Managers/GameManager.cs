using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    MainMenu,
    GameOver,
    Restart,
    Playing,
    Paused,
    StartStage,
    Dialogue,
    oldState
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameState oldState;
    GameState gameState;
    public UnityEvent<GameState, GameState> OnStateChange = new UnityEvent<GameState, GameState>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ChangeGameState(GameState newState)
    {
        oldState = gameState;
        gameState = newState;

        OnStateChange?.Invoke(oldState, newState);
    }
}

namespace UnityEngine
{
    public static class VectorRotEqualizer
    {
        public static float VectorToRotation(Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static Vector2 RotationToVector(float rotation)
        {
            return new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
        }
    }
}