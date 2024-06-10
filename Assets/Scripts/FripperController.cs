using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FripperController : MonoBehaviour
{
    //キーボード入力の判定
    private struct InputKeyFripper
    {
        //フリッパーのタグ
        private string tag;
        //フリッパーのタグプロパティ
        public string Tag
        {
            private set { this.tag = value; }
            get { return this.tag; }
        }
        //フリッパーを操作するKeyCodeリスト
        private List<KeyCode> keyCodes;
        //コンストラクタ
        public InputKeyFripper(string tag, KeyCode[] keyCodes)
        {
            this.tag = tag;
            this.keyCodes = new List<KeyCode>(keyCodes);
            for (int i = 0; i < keyCodes.Length; i++) this.keyCodes.Add(keyCodes[i]);
        }
        //フリッパーのキー入力判定
        public bool IsKeyFripper(Func<KeyCode, bool> func)
        {
            bool isInputKey = false;
            for(int i = 0; i < this.keyCodes.Count; i++)
            {
                if (func(this.keyCodes[i]))
                {
                    isInputKey = true;
                }
            }
            return isInputKey;
        }
    }
    //HingeJointコンポーネント
    private HingeJoint myHingeJoint;
    //初期の傾き
    private float defaultAngle = 20;
    //弾いた時の傾き
    private float flickAngle = -20;
    //フリッパーのキー入力判定インスタンス
    private InputKeyFripper leftKeyFripper;
    private InputKeyFripper rightKeyFripper;
    //フリッパーの左右のタグ列挙
    private enum FripperTag { LeftFripperTag, RightFripperTag }
    //フリッパーの左右のキーコード配列
    private KeyCode[] leftKeys = { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.A, KeyCode.S};
    private KeyCode[] rightKeys = { KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.D, KeyCode.S};

    delegate bool InputFripperKeyDown(KeyCode key);

    // Start is called before the first frame update
    void Start()
    {
        //HingeJointコンポーネント取得
        this.myHingeJoint = GetComponent<HingeJoint>();
        //フリッパーの傾きを設定
        SetAngle(this.defaultAngle);
        //フリッパーのキー入力インスタンス生成
        leftKeyFripper = new InputKeyFripper(FripperTag.LeftFripperTag.ToString(), leftKeys);
        rightKeyFripper = new InputKeyFripper(FripperTag.RightFripperTag.ToString(), rightKeys);
    }

    // Update is called once per frame
    void Update()
    {
        //左右のフリッパーを動かす判定処理
        SetFripper(leftKeyFripper.IsKeyFripper(Input.GetKeyDown), leftKeyFripper.Tag, this.flickAngle);
        SetFripper(rightKeyFripper.IsKeyFripper(Input.GetKeyDown), rightKeyFripper.Tag, this.flickAngle);

        //左右のフリッパーを元に戻す判定処理
        SetFripper(!leftKeyFripper.IsKeyFripper(Input.GetKey), leftKeyFripper.Tag, this.defaultAngle);
        SetFripper(!rightKeyFripper.IsKeyFripper(Input.GetKey), rightKeyFripper.Tag, this.defaultAngle);
    }

    //フリッパーを動かす
    private void SetFripper(bool isActiveInputKey, string tag, float angle)
    {
        if (!isActiveInputKey || tag != this.tag) return;
        SetAngle(angle);
    }

    //フリッパーの傾きを設定
    public void SetAngle(float angle)
    {
        JointSpring jointSpr = this.myHingeJoint.spring;
        jointSpr.targetPosition = angle;
        this.myHingeJoint.spring = jointSpr;
    }
}
