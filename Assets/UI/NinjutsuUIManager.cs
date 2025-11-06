using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 


public class NinjutsuUIManager : MonoBehaviour
{
    [Header("スロット回転設定")]
    public float spinDuration = 0.8f; // 各スロットが回る基本時間
    public float stopDelay = 0.2f;

    // Handlerから参照を渡されるUI要素の配列
    [Header("UI要素の参照")]
    public Image[] ninjutsuIconSlots;

    // UIのハイライト色設定
    [Header("UI要素の色設定")]
    public Color defaultColor = Color.white;
    public Color activeColor = Color.yellow;


    // 追加: Handlerから呼ばれ、回転アニメーションを開始するメソッド
    public void StartSlotSpin(BaseNinjutsu[] finalSet, List<BaseNinjutsu> allList)
    {
        // 最終的なセット内容と、回転中にランダム表示するための全リストを受け取る
        StartCoroutine(SpinSlotsCoroutine(finalSet, allList));
    }

    private IEnumerator SpinSlotsCoroutine(BaseNinjutsu[] finalSet, List<BaseNinjutsu> allList)
    {
        int slotCount = ninjutsuIconSlots.Length;
        float elapsedTime = 0f;

        // 各スロットが止まるべき目標時間（スロットごとに遅延を設ける）
        float[] stopTimes = new float[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            stopTimes[i] = spinDuration + i * stopDelay;
        }

        // 最後のスロットが止まるまでループ
        while (elapsedTime < stopTimes[slotCount - 1])
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < slotCount; i++)
            {
                Image slotImage = ninjutsuIconSlots[i];
                if (slotImage == null) continue;

                if (elapsedTime < stopTimes[i])
                {
                    // 回転中: 全リストからランダムなアイコンを高速表示
                    if (allList != null && allList.Count > 0)
                    {
                        int randomIndex = Random.Range(0, allList.Count);
                        BaseNinjutsu randomNinjutsu = allList[randomIndex];

                        if (randomNinjutsu != null && randomNinjutsu.icon != null)
                        {
                            slotImage.sprite = randomNinjutsu.icon;
                            slotImage.enabled = true;
                            slotImage.color = activeColor; // 回転中はハイライト色で強調
                        }
                    }
                }
                else
                {
                    // 停止済み: 最終的に決定したアイコンを表示し、色をデフォルトに戻す
                    // finalSetに格納されている決定アイコンを表示
                    if (finalSet[i] != null && finalSet[i].icon != null)
                    {
                        slotImage.sprite = finalSet[i].icon;
                        slotImage.enabled = true;
                        slotImage.color = defaultColor;
                    }
                    // ただし、ランダムな結果でスロットが空になった場合も考慮
                    else
                    {
                        slotImage.sprite = null;
                        slotImage.enabled = false;
                        slotImage.color = defaultColor;
                    }
                }
            }

            yield return null; // 1フレーム待機
        }

        // すべての回転アニメーションが終了した後、最終結果としてUIを更新（nextIndex=0でハイライトも設定される）
        UpdateUI(finalSet, 0);
    }

    // ハンドラーがこのメソッドを呼び出す
    // 新しいセットが生成されたとき、または忍術が使用されたときに呼ばれる
    public void UpdateUI(BaseNinjutsu[] currentSet, int nextIndex)
    {
        // 1. 全スロットの初期化と更新
        for (int i = 0; i < currentSet.Length; i++)
        {
            // UIスロットが存在し、かつ参照がnullでないか確認
            if (i < ninjutsuIconSlots.Length && ninjutsuIconSlots[i] != null)
            {
                BaseNinjutsu ninjutsu = currentSet[i];
                Image slotImage = ninjutsuIconSlots[i];

                if (ninjutsu != null)
                {
                    // スプライトを設定
                    slotImage.sprite = ninjutsu.icon;
                    slotImage.enabled = true;
                    // ハイライトを設定
                    slotImage.color = (i == nextIndex) ? activeColor : defaultColor;
                }
                else
                {
                    // 消費または空のスロットの場合
                    slotImage.sprite = null;
                    slotImage.enabled = false;
                    slotImage.color = defaultColor;
                }
            }
        }

        // 2. すべて使い切った後の処理 (nextIndexが配列外になった場合)
        if (nextIndex >= currentSet.Length)
        {
            foreach (var slot in ninjutsuIconSlots)
            {
                if (slot != null)
                {
                    // すべての色をデフォルトに戻す（ハイライト解除）
                    slot.color = defaultColor;
                }
            }
        }
    }
}