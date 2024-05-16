using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    //Scoreに加算されるルールを扱う構造体
    private struct AddScoreRule
    {
        public string tag;
        public int score;
        public AddScoreRule(string tag, int score)
        {
            this.tag = tag;
            this.score = score;
        }
    }
    //ScoreTextのGameObject
    [SerializeField]
    private GameObject scoreTextObj;
    //ScoreTextのText
    private Text scoreText;
    //各Score関連オブジェクトの加算するScore値
    [SerializeField]
    private int smallStarScore = 10;
    [SerializeField]
    private int largeStarScore = 20;
    [SerializeField]
    private int smallCloudScore = 50;
    [SerializeField]
    private int largeCloudScore = 300;
    //現在のゲームスコア
    private int gameScore = 0;
    //Scoreの最大値
    private const int maxScore = 999999999;

    //各Score関連オブジェクトのタグ
    private string smallStarTag = "SmallStarTag";
    private string largeStarTag = "LargeStarTag";
    private string smallCloudTag = "SmallCloudTag";
    private string largeCloudTag = "LargeCloudTag";
    List<AddScoreRule> addScoreRules;

    private void Start()
    {
        Init();
    }

    //初期化
    private void Init()
    {
        //現在のゲームスコアを0で初期化
        this.gameScore = 0;
        //scoreTextにscoreTextObjからTextコンポーネントのインスタンスを取得
        if (scoreTextObj != null)
        {
            this.scoreText = this.scoreTextObj.GetComponent<Text>();
        }
        //現在のゲームスコア表示を更新
        UpdateScoreText();
        //Scoreの加算ルールを初期化
        InitAddScoreRule();
    }

    //Scoreの加算ルールを初期化設定する
    private void InitAddScoreRule()
    {
        //構造体リストaddScoreRulesにAddScoreRuleインスタンスを初期化して追加していく
        addScoreRules = new List<AddScoreRule>();
        addScoreRules.Add(new AddScoreRule(this.smallStarTag, this.smallStarScore));
        addScoreRules.Add(new AddScoreRule(this.largeStarTag, this.largeStarScore));
        addScoreRules.Add(new AddScoreRule(this.smallCloudTag, this.smallCloudScore));
        addScoreRules.Add(new AddScoreRule(this.largeCloudTag, this.largeCloudScore));
    }

    //ゲームスコアの表示を更新する
    private void UpdateScoreText()
    {
        this.scoreText.text = this.gameScore.ToString();
    }

    //ゲームスコアを加算する
    private void AddScore(int addScore)
    {
        this.gameScore = Mathf.Min(Mathf.Max(this.gameScore + addScore, 0), maxScore);
    }

    //Scoreオブジェクトの加算スコア判定処理
    private void HitScore(string targetTag)
    {
        for(int i = 0; i < this.addScoreRules.Count; i++)
        {
            if (this.addScoreRules[i].tag != targetTag) continue;

            AddScore(this.addScoreRules[i].score);
            break;
            
        }
    }

    //当たり判定処理
    private void OnCollisionEnter(Collision target)
    {
        HitScore(target.gameObject.tag);
        UpdateScoreText();
    }
}
