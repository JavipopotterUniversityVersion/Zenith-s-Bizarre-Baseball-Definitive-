public class PlayerLifesManager : LifesManager
{
    protected override void Die()
    {
        base.Die();
        GameManager.instance.ChangeGameState(GameState.GameOver);
    }
}