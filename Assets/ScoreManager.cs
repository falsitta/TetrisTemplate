using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreDisplay;
    public void AddScore(int linesCleared)
    {
        switch (linesCleared)
        {
            case 1: score += 100; break;
            case 2: score += 300; break;
            case 3: score += 500; break;
            case 4: score += 800; break;
        }
        Debug.Log($"Score: {score}");
        scoreDisplay.text = "Score: " + score.ToString();
    }
}
