using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisEvents : MonoBehaviour
{
    // Declare the delegate and event
    public delegate void LineClearedEvent(int linesCleared);
    public static event LineClearedEvent OnLineCleared;

    // Trigger the event
    public static void ClearLines(int linesCleared)
    {
        OnLineCleared?.Invoke(linesCleared);
    }
}
