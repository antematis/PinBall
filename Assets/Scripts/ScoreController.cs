using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreTextObj;
    private Text scoreText;
    [SerializeField]
    private int smallStarScore = 10;
    [SerializeField]
    private int largeStarScore = 20;
    [SerializeField]
    private int smallCloudScore = 50;
    [SerializeField]
    private int largeCloudScore = 300;
    private int gameScore = 0;
    private const int maxScore = 999999999;

    private string smallStarTag = "SmallStarTag";
    private string largeStarTag = "LargeStarTag";
    private string smallCloudTag = "SmallCloudTag";
    private string largeCloudTag = "LargeCloudTag";

    private void Start()
    {
        this.gameScore = 0;
        if(scoreTextObj != null)
        {
            this.scoreText = this.scoreTextObj.GetComponent<Text>();
        }
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        this.scoreText.text = this.gameScore.ToString();
    }

    private void HitScore(string targetTag)
    {
        if (this.smallStarTag == targetTag) this.gameScore += this.smallStarScore;
        if (this.largeStarTag == targetTag) this.gameScore += this.largeStarScore;
        if (this.smallCloudTag == targetTag) this.gameScore += this.smallCloudScore;
        if (this.largeCloudTag == targetTag) this.gameScore += this.largeCloudScore;
    }

    private void OnCollisionEnter(Collision target)
    {
        HitScore(target.gameObject.tag);
        UpdateScoreText();
    }
}
