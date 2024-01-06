using UnityEngine;
public class InteractionHandler : MonoBehaviour
{
    IInteraction _interaction;
    public IInteraction interaction
    {
        get
        {
            return _interaction;
        }
        set
        {
            _interaction = value;
        }
    }

    public void Interact()
    {
        if(interaction != null) interaction.Interact();
    }
}