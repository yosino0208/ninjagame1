using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NinjutsuHandler : MonoBehaviour
{
    // 【Inspectorで設定】プロジェクト内に作成した全忍術のデータアセットを格納
    [Header("全忍術データ")]
    public List<BaseNinjutsu> allNinjutsuList;

    [Header("現在使用可能な忍術セット")]
    // ランダムに選ばれた3つの忍術を格納する配列 (ストック)
    public BaseNinjutsu[] currentNinjutsuSet = new BaseNinjutsu[3];

    // 次に使用する忍術スロットのインデックス (未使用だが互換性のため残す)
    [SerializeField]
    private int nextSlotIndex = 0;

    // 巻物使用のクールダウン（連打防止用）
    private float cooldownTime = 0.5f;
    private float lastUseTime = 0f;

    void Start()
    {
        // 乱数生成と初期セットは PlayerInput.cs 側に任せるため、ここでは特に処理なし
    }

    // 【PlayerInputから呼ばれる】乱数インデックスを受け取って忍術をセットする
    public void SetRandomNinjutsuSet(List<int> randomIndices)
    {
        if (allNinjutsuList == null || allNinjutsuList.Count == 0 || randomIndices == null)
        {
            Debug.LogError("NinjutsuHandler: セットに必要なデータが不足しています。");
            return;
        }

        // 既存のセットをクリア
        for (int i = 0; i < currentNinjutsuSet.Length; i++)
        {
            currentNinjutsuSet[i] = null;
        }

        // 取得したインデックスをもとに忍術をセット
        for (int i = 0; i < currentNinjutsuSet.Length; i++)
        {
            if (i < randomIndices.Count)
            {
                int randomIndex = randomIndices[i];

                // インデックスがリストの範囲内であることを確認
                if (randomIndex < allNinjutsuList.Count)
                {
                    // 複製してセット (オリジナルのScriptableObjectデータが変更されないように)
                    // Note: BaseNinjutsuクラスがScriptableObjectを継承している必要があります
                    currentNinjutsuSet[i] = Instantiate(allNinjutsuList[randomIndex]);

                    Debug.Log($"[Randomizer] Slot {i + 1} に Index: {randomIndex} ({currentNinjutsuSet[i].ninjutsuName}) が選ばれました。");
                }
            }
        }

        // インデックスは PlayerInput が管理するため、ここではリセットのみ
        nextSlotIndex = 0;
        Debug.Log("新しい巻物セットが完了しました！");
    }

    // 【PlayerInputから呼ばれる】指定されたスロットの忍術を発動する
    public void UseNinjutsu(int index)
    {
        // クールダウンチェック
        if (Time.time < lastUseTime + cooldownTime) return;

        if (index < 0 || index >= currentNinjutsuSet.Length)
        {
            Debug.LogError("無効な忍術スロットが指定されました。");
            return;
        }

        BaseNinjutsu usedNinjutsu = currentNinjutsuSet[index];

        if (usedNinjutsu != null)
        {
            // 忍術の発動ロジックを呼び出す
            Vector3 direction = new Vector3(transform.localScale.x, 0, 0);
            usedNinjutsu.Activate(transform.position, direction, this.transform);

            // 発動後、使用したスロットをクリア
            currentNinjutsuSet[index] = null;
            lastUseTime = Time.time;

            Debug.Log($"忍術発動！ Slot {index + 1} ({usedNinjutsu.ninjutsuName}) を消費しました。");
        }
    }

    // --- デバッグ表示 ---

    // 現在のストック内容をデバッグ表示するためのメソッド
    void Update()
    {
        DisplayCurrentStockDebug();
    }

    private void DisplayCurrentStockDebug()
    {
        List<string> stockNames = new List<string>();

        for (int i = 0; i < currentNinjutsuSet.Length; i++)
        {
            BaseNinjutsu ninjutsu = currentNinjutsuSet[i];

            if (ninjutsu != null)
            {
                stockNames.Add($"Slot {i + 1}: {ninjutsu.ninjutsuName}");
            }
            else
            {
                stockNames.Add($"Slot {i + 1}: (USED)");
            }
        }

        string nextSlotInfo = $"\n--- Total Slots: {currentNinjutsuSet.Count()} ---";

        // Warningログとして出力し、コンソールで見分けやすくする
        Debug.LogWarning($"[MKMN Stock] {string.Join(" | ", stockNames)}{nextSlotInfo}");
    }

    // 外部からリセットを強制する関数（Rキー入力に対応）
    public void ForceReset()
    {
        // この関数は、PlayerInput側で GenerateAndSetRandomNinjutsu() が呼ばれることで代替される
        Debug.Log("NinjutsuHandler: ForceResetはPlayerInput側で処理されます。");
    }
}