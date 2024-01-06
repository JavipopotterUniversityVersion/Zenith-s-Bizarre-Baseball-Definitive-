using UnityEngine;
public class OnTriggerInteractable : MonoBehaviour
{
    IInteraction onEnterInteraction;
    IInteraction mainInteraction;
    IInteraction onExitInteraction;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent(out InteractionHandler handler))
        {
            handler.interaction = mainInteraction;
            onEnterInteraction.Interact();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.TryGetComponent(out InteractionHandler handler))
        {
            onExitInteraction.Interact();
            
            if(handler.interaction == mainInteraction){
                handler.interaction = null;
            }
        }
    }
}