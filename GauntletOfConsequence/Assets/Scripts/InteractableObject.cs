using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public string interactID; //what object was clicked
    public bool isEnabled = false; //to turn interactivity on or off
    public UnityEvent onClickAction; //to allow custom actions per scene/interactable

    public void EnableInteraction()
    {
        isEnabled = true;
    }
    public void DisableInteraction()
    {
        isEnabled = false;
    }
    private void OnMouseDown()
    {
        if (!isEnabled)
        {
            return;
        }

        onClickAction?.Invoke();
    }
}
