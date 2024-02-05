using UnityEngine;

public abstract class StateManager : MonoBehaviour
{
    private string _state;
    public string state
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            animator.SetTrigger(value);
        }
    }
    
    Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        GameManager.instance.OnStateChange.AddListener(OnGameStateChange);
    }

    protected virtual void OnDestroy() {
        GameManager.instance.OnStateChange.RemoveListener(OnGameStateChange);
    }

    protected virtual void OnGameStateChange(GameState gameState, GameState newState)
    {
        switch(gameState)
        {
            case GameState.Paused:
                animator.SetBool("Paused", false);
                break;
        }

        switch(newState)
        {
            case GameState.Paused:
                animator.SetBool("Paused", true);
                break;
        }
    }

    protected virtual void ChangeState(string newState) => state = newState;
}
