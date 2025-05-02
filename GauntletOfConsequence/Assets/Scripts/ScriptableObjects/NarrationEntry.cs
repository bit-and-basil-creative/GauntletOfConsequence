using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewNarrationEntry", menuName = "Narration/Entry")]

public class NarrationEntry : ScriptableObject
{
    public int sceneNumber;

    [TextArea(4, 10)]
    public List<string> narrationLines;

    public bool allowSceneAdvance; //to track whether this line of dialogue allows the scene to advance
    public bool isIntroSection; //to track if this dialogue is an intro section
}
