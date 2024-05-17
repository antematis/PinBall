using System.Collections;
using System.Collections.Generic;
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
        //フリッパーのキー入力されている判定
        public bool IsKeyFripperOn()
        {
            for(int i = 0; i < this.keyCodes.Count; i++) if (Input.GetKeyDown(this.keyCodes[i])) return true;
            return false;
        }
        //フリッパーのキーが入力されていない判定
        public bool IsKeyFripperOff()
        {
            for (int i = 0; i < this.keyCodes.Count; i++) if (Input.GetKeyUp(this.keyCodes[i])) return true;
            return false;
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
    private KeyCode[] leftKeys = { KeyCode.LeftArrow, KeyCode.A, KeyCode.S};
    private KeyCode[] rightKeys = { KeyCode.RightArrow, KeyCode.D, KeyCode.S};

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
        SetFripper(leftKeyFripper.IsKeyFripperOn(), leftKeyFripper.Tag, this.flickAngle);
        SetFripper(rightKeyFripper.IsKeyFripperOn(), rightKeyFripper.Tag, this.flickAngle);

        //左右のフリッパーを元に戻す判定処理
        SetFripper(leftKeyFripper.IsKeyFripperOff(), leftKeyFripper.Tag, this.defaultAngle);
        SetFripper(rightKeyFripper.IsKeyFripperOff(), rightKeyFripper.Tag, this.defaultAngle);
    }

    //フリッパーを動かす
    private void SetFripper(bool isActive, string tag, float angle)
    {
        if (!isActive || tag != this.tag) return;
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
