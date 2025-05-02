using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroRoomDoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private GameManager gameManager;

    public void OpenDoor()
    {
        doorAnimator.SetBool("isClicked", true);
        StartCoroutine(LoadRoomAfterDelay());
    }

    private IEnumerator LoadRoomAfterDelay()
    {
        yield return new WaitForSeconds(2.0f); // or animation.length
        gameManager.LoadNextRoom();
    }
}
