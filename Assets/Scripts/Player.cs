using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    

    public static GameObject ScorringPrefab;
    
    private int playerScore = 0;
    private Difficulty playerDifficulty;
    private ScoreTextScript scoreScript;
    
    private static List<int> bombChance = new List<int>() {2,4,8,15};
    private static List<int> comboMultiplier = new List<int>() {1,3,8,25,80,200,600};
    private static List<float> comboDifficultyEffect = new List<float>() {1f,1.75f,0.80f,0.25f};
    public enum Difficulty {
        EASY,
        MEDIUM,
        HARD,
        VERYHARD,
    }
    
    public void SetDifficulty(Difficulty currentDiff) {
        playerDifficulty = currentDiff;
    }
    
    public int GetComboMultiplier(int level) {
        if (level > 6) {
            level = 6;
        }
        return (int)(comboMultiplier[level] * comboDifficultyEffect[(int) playerDifficulty]);
    }
    
    public int GetComboMultiplier() {
        int level = GameManager.GetCurrentCombo();
        if (level > 6) {
            level = 6;
        }
        //Debug.LogWarning("cM:"+((int)(comboMultiplier[level]) + "dificult ef:"+ comboDifficultyEffect [((int) playerDifficulty)]));
        return (int)(comboMultiplier[level] * comboDifficultyEffect[(int) playerDifficulty]);
    }
    
    public void Initilize () {
        scoreScript = GameObject.Find("TotalScoreTxt").GetComponent<ScoreTextScript>();
        ScorringPrefab = Resources.Load("prefabs/Scoring") as GameObject;
        scoreScript.Initilize();
    }
    
    public void AddScore(int score) {
        
        if (score > -1) {
            playerScore += score;
            scoreScript.AddScore(score);
        }
    }
    
    public void GameOverScore(int gameOverScore) {
        if (gameOverScore > PlayerPrefs.GetInt("C_BEST_SCORE")) {
            //TODO best score animation starting
            PlayerPrefs.SetInt("C_BEST_SCORE",gameOverScore);
        }
    }
    
    public void ResetPlayer() {
        playerScore = 0;
    }

    public bool isBomb() {
        int chance = Random.Range(1, 101);
        if (chance <= bombChance[(int)playerDifficulty]) {
            return true;
        }else{
            return false;
        }
    }
    
}
