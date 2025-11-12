using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // ToList() のために必要
using NinjaGame;

public class NinjutsuUIManager : MonoBehaviour
{
    [Header("スロット回転設定")]
    public float spinDuration = 0.8f;
    public float stopDelay = 0.2f;
    public int spinFramesPerSlot = 15; // スロット一つあたりに切り替わるアイコンの回数

    [Header("UI要素の参照")]
    public Image[] ninjutsuIconSlots;

    [Header("UI要素の色設定")]
    public Color defaultColor = Color.white;
    public Color activeColor = Color.yellow;


    // Handlerから呼ばれ、回転アニメーションを開始するメソッド
    public void StartSlotSpin(BaseNinjutsu[] finalSet, List<BaseNinjutsu> allList)
    {
        StopAllCoroutines();

        // 念のため、最終セットと全リストからNull要素を除去
        List<BaseNinjutsu> cleanAllList = allList.Where(n => n != null).ToList();

        StartCoroutine(SpinSlotsCoroutine(finalSet, cleanAllList));
    }

    private IEnumerator SpinSlotsCoroutine(BaseNinjutsu[] finalSet, List<BaseNinjutsu> cleanAllList)
    {
        int slotCount = ninjutsuIconSlots.Length;
        float elapsedTime = 0f;

        if (cleanAllList.Count == 0)
        {
            Debug.LogError("NinjutsuUIManager: 回転用の有効な忍術データがありません。");
            UpdateUI(finalSet, 0);
            yield break;
        }

        // 【安全な回転ロジック】
        // 1. 回転中に表示する全アイコンのリストを事前に作成
        List<BaseNinjutsu> spinDisplayList = new List<BaseNinjutsu>();
        for (int i = 0; i < slotCount * spinFramesPerSlot; i++)
        {
            // ここでランダムアクセスを一度だけ行い、コルーチン内での再アクセスを防ぐ
            int randomIndex = Random.Range(0, cleanAllList.Count);
            spinDisplayList.Add(cleanAllList[randomIndex]);
        }

        float totalSpinTime = spinDuration + (slotCount - 1) * stopDelay;
        int currentIconIndex = 0; // spinDisplayListの現在のインデックス

        // --- コルーチンのループ ---

        while (elapsedTime < totalSpinTime)
        {
            elapsedTime += Time.deltaTime;

            // アイコンを切り替えるフレームを計算 (高速回転)
            // 速度に比例してインデックスを進める
            currentIconIndex = Mathf.FloorToInt(elapsedTime * (spinDisplayList.Count / totalSpinTime));

            for (int i = 0; i < slotCount; i++)
            {
                Image slotImage = ninjutsuIconSlots[i];
                if (slotImage == null) continue;

                // 各スロットの停止時間をチェック
                if (elapsedTime < spinDuration + i * stopDelay)
                {
                    // spinDisplayListから安全にアイコンを取得
                    int displayIndex = (currentIconIndex + i) % spinDisplayList.Count;
                    BaseNinjutsu ninjutsu = spinDisplayList[displayIndex];

                    if (ninjutsu != null && ninjutsu.icon != null)
                    {
                        slotImage.sprite = ninjutsu.icon;
                        slotImage.enabled = true;
                        slotImage.color = activeColor;
                    }
                }
                else
                {
                    // 停止済み: 最終結果を表示
                    if (finalSet[i] != null && finalSet[i].icon != null)
                    {
                        slotImage.sprite = finalSet[i].icon;
                        slotImage.enabled = true;
                        slotImage.color = defaultColor;
                    }
                    else
                    {
                        slotImage.sprite = null;
                        slotImage.enabled = false;
                        slotImage.color = defaultColor;
                    }
                }
            }

            yield return null;
        }

        // アニメーション終了
        UpdateUI(finalSet, 0);
    }

    // Handlerがこのメソッドを呼び出す
    public void UpdateUI(BaseNinjutsu[] currentSet, int nextIndex)
    {
        for (int i = 0; i < currentSet.Length; i++)
        {
            if (i < ninjutsuIconSlots.Length && ninjutsuIconSlots[i] != null)
            {
                BaseNinjutsu ninjutsu = currentSet[i];
                Image slotImage = ninjutsuIconSlots[i];

                if (ninjutsu != null)
                {
                    slotImage.sprite = ninjutsu.icon;
                    slotImage.enabled = true;
                    slotImage.color = (i == nextIndex) ? activeColor : defaultColor;
                }
                else
                {
                    slotImage.sprite = null;
                    slotImage.enabled = false;
                    slotImage.color = defaultColor;
                }
            }
        }

        // すべて使い切った後の処理
        if (nextIndex >= currentSet.Length)
        {
            foreach (var slot in ninjutsuIconSlots)
            {
                if (slot != null)
                {
                    slot.color = defaultColor;
                }
            }
        }
    }
}