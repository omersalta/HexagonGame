using System;
using UnityEngine;
using TMPro;

public class ScoreTextScript : MonoBehaviour {
    private int Score;
    private Animator myAnim;
    private TextMeshProUGUI myText;
    public float delay = 0f;


    public void Initilize() {
        Score = 0;
        myAnim = GetComponent<Animator>();
        myText = GetComponent<TextMeshProUGUI>();
    }
    
    public void AddScore(int adding) {
        Score += adding;
        myText.text = Score.ToString();
        myAnim.SetTrigger("change");
    }
}
