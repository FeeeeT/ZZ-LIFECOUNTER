using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    // UI関連
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject diceUI;
    [SerializeField] GameObject manualUI;

    // テキストボックス
    [SerializeField] GameObject textBox;
    private Animator textAnim;
    private Text textLog;

    // プレイヤー関連
    [SerializeField] GameObject player;
    [SerializeField] Image playerImage;
    [SerializeField] Text playerLifeText;

    [SerializeField] GameObject playerButtons;

    // フォース1関連
    [SerializeField] GameObject forceOne;
    [SerializeField] Image forceOneImage;
    [SerializeField] Text forceOneLifeText;

    // フォース2関連
    [SerializeField] GameObject forceTwo;
    [SerializeField] Image forceTwoImage;
    [SerializeField] Text forceTwoLifeText;

    // セットボタン関連
    [SerializeField] GameObject setButton;
    [SerializeField] GameObject resetButton;

    // セッティングテキスト
    [SerializeField] GameObject settingText;

    // リターンボタン
    [SerializeField] GameObject returnButton;

    // ライフ関連
    [SerializeField] private uint allLifeMax = 12;  // 全てのライフの合計上限
    [SerializeField] private uint lifeMax = 10;     // ライフの最大値
    private uint playerLifeMax = 10;    // プレイヤーのライフ最大値（セット時使用）

    private uint playerLife = 10;       // プレイヤーのライフ
    private uint forceOneLife = 1;      // フォース1のライフ
    private uint forceTwoLife = 1;      // フォース2のライフ

    private bool setBool;  // 設定モードの切り替え

    // フレームレート制御関連
    private float countDown;    // FPS切り替えカウント
    private bool diceBool;      // ダイスモード判定
    private bool manualBool;    // マニュアルモード判定S

    void Start()
    {
        // バー表示
        ApplicationChrome.statusBarState = ApplicationChrome.States.Visible;
        ApplicationChrome.navigationBarState = ApplicationChrome.States.Hidden;

        // 垂直同期解除
        QualitySettings.vSyncCount = 0;

        // FPS設定カウントセット
        countDown = 0.2f;

        // スクリーンスリープをオフ
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // テキストボックス設定
        textAnim = textBox.GetComponent<Animator>();
        textLog = textBox.GetComponentInChildren<Text>();

        // UI初期化
        mainUI.SetActive(true);
        diceUI.SetActive(false);
        manualUI.SetActive(false);
        resetButton.SetActive(false);
        returnButton.SetActive(false);

        // フラグ初期化
        setBool = true;
        diceBool = false;

        // 数値テキスト初期化
        playerLifeText.text = playerLife.ToString();
        forceOneLifeText.text = forceOneLife.ToString();
        forceTwoLifeText.text = forceTwoLife.ToString();
    }

    void Update()
    {
        // カウントダウン
        if (countDown > 0.0f || setBool || diceBool || manualBool)
        {
            countDown -= Time.deltaTime;
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 15;
        }

        // 数値テキスト更新
        playerLifeText.text = playerLife.ToString();
        forceOneLifeText.text = forceOneLife.ToString();
        forceTwoLifeText.text = forceTwoLife.ToString();

        // セット時は、プレイヤーのライフ上限を自動調整
        if (setBool)
        {
            playerButtons.SetActive(false);
            playerLifeMax = allLifeMax - (forceOneLife + forceTwoLife);
            playerLife = playerLifeMax;
        }
        else
        {
            playerButtons.SetActive(true);
        }

        // ライフ0の時にグレーアウト
        if (playerLife == 0)
        {
            playerImage.CrossFadeAlpha(0.1f, 0.1f, true);
        }
        else
        {
            playerImage.CrossFadeAlpha(1.0f, 0.1f, true);
        }

        if (forceOneLife == 0)
        {
            forceOneImage.CrossFadeAlpha(0.1f, 0.1f, true);
        }
        else
        {
            forceOneImage.CrossFadeAlpha(1.0f, 0.1f, true);
        }

        if (forceTwoLife == 0)
        {
            forceTwoImage.CrossFadeAlpha(0.1f, 0.1f, true);
        }
        else
        {
            forceTwoImage.CrossFadeAlpha(1.0f, 0.1f, true);
        }
    }

    // テキストボックス処理(バグがあるので未実装)
    /*
    private void textLogOut(string text = null)
    {
        textLog.text = text;
        textAnim.SetTrigger("textTrigger");
    }
    */

    // 値の増減メソッド（アニメーション）
    private void UpDown(GameObject target, string order)
    {
        if (!setBool)
        {
            target.GetComponent<Animator>().SetTrigger(order);
        }
    }

    // ボタン操作
    private void OnClick(string order)
    {
        countDown = 3.0f;

        switch (order)
        {
            case "pUP":
                if (playerLife < lifeMax)
                {
                    playerLife++;
                    UpDown(player, "up");
                }
                else
                {
                    //textLogOut("上限に達しています");
                }
                break;
            case "pDOWN":
                if (playerLife > 0)
                {
                    playerLife--;
                    UpDown(player, "down");
                }
                break;

            case "f1UP":
                if (forceOneLife < lifeMax && playerLife > 0)
                {
                    forceOneLife++;
                    UpDown(forceOne, "up");
                }
                else
                {
                    //textLogOut("上限に達しています");
                }
                break;
            case "f1DOWN":
                if (forceOneLife > 0)
                {
                    forceOneLife--;
                    UpDown(forceOne, "down");
                }
                break;

            case "f2UP":
                if (forceTwoLife < lifeMax && playerLife > 0)
                {
                    forceTwoLife++;
                    UpDown(forceTwo, "up");
                }
                else
                {
                    //textLogOut("上限に達しています");
                }
                break;
            case "f2DOWN":
                if (forceTwoLife > 0)
                {
                    forceTwoLife--;
                    UpDown(forceTwo, "down");
                }
                break;

            case "reset":
                playerLife = 10;
                forceOneLife = 1;
                forceTwoLife = 1;
                //textLogOut("リセット");
                setBool = true;
                resetButton.SetActive(false);
                setButton.SetActive(true);
                settingText.SetActive(true);
                break;

            case "set":
                if (setBool)
                {
                    //textLogOut("設定完了");
                    setButton.SetActive(false);
                    resetButton.SetActive(true);
                    settingText.SetActive(false);
                    setBool = false;
                }
                else
                {
                    setBool = true;
                }
                break;

            case "dice":
                mainUI.SetActive(false);
                diceUI.SetActive(true);
                returnButton.SetActive(true);
                diceBool = true;
                break;

            case "manual":
                mainUI.SetActive(false);
                manualUI.SetActive(true);
                returnButton.SetActive(true);
                manualBool = true;
                break;

            case "return":
                mainUI.SetActive(true);
                if (diceBool == true)
                {
                    diceUI.SetActive(false);
                    diceBool = false;
                }
                if (manualBool == true)
                {
                    manualUI.SetActive(false);
                    manualBool = false;
                }
                returnButton.SetActive(false);
                break;


        }
    }
}
