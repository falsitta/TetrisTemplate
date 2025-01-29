using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisManager : MonoBehaviour
{
    public int score = 0;

    public void AddScore(int linesCleared)
    {
        score += linesCleared * 100;
        Debug.Log($"Score: {score}");
    }

    public int CalculateScore(int linesCleared)
    {
        switch (linesCleared)
        {
            case 1: return 100;
            case 2: return 300;
            case 3: return 500;
            case 4: return 800;
            default: return 0;
        }
    }


}