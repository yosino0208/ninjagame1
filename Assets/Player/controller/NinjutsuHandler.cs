using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NinjaGame;

public class NinjutsuHandler : MonoBehaviour
{
    // 【Inspectorで設定】プロジェクト内に作成した全忍術のデータアセットを格納
    [Header("全忍術データ")]
    public List<BaseNinjutsu> allNinjutsuList;

    [Header("現在使用可能な忍術セット")]
    public BaseNinjutsu[] currentNinjutsuSet = new BaseNinjutsu[3];
    private const int SET_SIZE = 3;

    // UI Managerへの参照
    private NinjutsuUIManager uiManager;

    // 次に使用する忍術スロットのインデックス
    [SerializeField]
    private int nextSlotIndex = 0;

    // 巻物使用のクールダウン（連打防止用）
    private float cooldownTime = 0.5f;
    private float lastUseTime = 0f;

    void Start()
    {
        // UI Managerの参照を取得 (同じGameObjectにアタッチされていることが前提)
        uiManager = GetComponent<NinjutsuUIManager>();
        if (uiManager == null)
        {
           // Debug.LogError("NinjutsuHandler: NinjutsuUIManagerが見つかりません。同じGameObjectにアタッチしてください。");
        }
    }

    // 【Eキー】複合技の判定を優先し、なければ単発技として処理する
    public void UseNinjutsuComboOrSingle()
    {
        if (Time.time < lastUseTime + cooldownTime) return; // クールダウンチェック

        // 1. 複合技が発動可能かチェック
        NinjutsuElement[] elements = currentNinjutsuSet
            .Select(n => n != null ? n.type : NinjutsuElement.None)
            .ToArray();

        Vector3 direction = new Vector3(transform.localScale.x, 0, 0);

        // 複合技が発動した場合
        if (NinjutsuCombiner.TryActivateCombo(elements, transform.position, direction, this.transform))
        {
            Debug.Log("一致");
            // 複合技が発動した場合、すべてのスロットをクリアする
            for (int i = 0; i < SET_SIZE; i++)
            {
                currentNinjutsuSet[i] = null;
            }
            nextSlotIndex = 0;
            lastUseTime = Time.time;

            return;
        }

        // 2. 複合技が発動しなかった場合、リストの先頭から単発忍術を消費する
        UseNextNinjutsu_SingleOnly();
    }

    // 単発技の消費ロジック (Eキーのフォールバック)
    private void UseNextNinjutsu_SingleOnly()
    {
        if (nextSlotIndex < 0 || nextSlotIndex >= currentNinjutsuSet.Length)
        {
            Debug.LogWarning("すべての忍術を使い切りました。Rキーで新しい巻物を生成してください。");
            return;
        }

        BaseNinjutsu usedNinjutsu = currentNinjutsuSet[nextSlotIndex];

        if (usedNinjutsu != null)
        {
            Vector3 direction = new Vector3(transform.localScale.x, 0, 0);
            usedNinjutsu.Activate(transform.position, direction, this.transform);

            currentNinjutsuSet[nextSlotIndex] = null;
            lastUseTime = Time.time;

            Debug.Log($"忍術発動！ Slot {nextSlotIndex + 1} ({usedNinjutsu.ninjutsuName}) を消費しました。");

            nextSlotIndex++;
            if (uiManager != null) uiManager.UpdateUI(currentNinjutsuSet, nextSlotIndex);
        }
        else
        {
            // 空スロットのスキップ
            nextSlotIndex++;
            if (uiManager != null) uiManager.UpdateUI(currentNinjutsuSet, nextSlotIndex);
            // スキップ後、次のスロットで再度処理を試行
            UseNextNinjutsu_SingleOnly();
        }
    }

    // 【Qキー/Wキーから移行した機能】リスト（配列）に残っている忍術をすべて使うメソッド
    public void UseAllRemainingNinjutsu()
    {
        if (Time.time < lastUseTime + cooldownTime) return; // クールダウンチェック

        // 1. 複合技が発動可能かチェック (UseNinjutsuComboOrSingleからロジックを移植)
        NinjutsuElement[] elements = currentNinjutsuSet
            .Select(n => n != null ? n.type : NinjutsuElement.None)
            .ToArray();

        Vector3 direction = new Vector3(transform.localScale.x, 0, 0);

        // 複合技が発動した場合
        if (NinjutsuCombiner.TryActivateCombo(elements, transform.position, direction, this.transform))
        {
            Debug.Log("【一斉発動】: 複合技が一致しました！");
            // 複合技が発動した場合、すべてのスロットをクリアする
            for (int i = 0; i < SET_SIZE; i++)
            {
                currentNinjutsuSet[i] = null;
            }
            nextSlotIndex = 0;
            lastUseTime = Time.time;

            // UIを更新し、すべて空になったことを表示
            if (uiManager != null)
            {
                uiManager.UpdateUI(currentNinjutsuSet, nextSlotIndex);
            }

            return; // 複合技が発動したため、単発の一斉発動は行わない
        }

        // 2. 複合技が発動しなかった場合、リスト（配列）に残っている忍術をすべて使う (元のロジック)

        Debug.Log("【一斉発動】: 複合技は不成立。残りの忍術をすべて単発で使います！");
        bool usedAtLeastOne = false;

        // nextSlotIndexから最後までをチェック
        for (int i = nextSlotIndex; i < SET_SIZE; i++)
        {
            BaseNinjutsu usedNinjutsu = currentNinjutsuSet[i];

            if (usedNinjutsu != null)
            {
                // 忍術の発動ロジックを呼び出す
                usedNinjutsu.Activate(transform.position, direction, this.transform);

                // 発動後、スロットをクリア
                currentNinjutsuSet[i] = null;
                usedAtLeastOne = true;

                Debug.Log($"一斉発動！ Slot {i + 1} ({usedNinjutsu.ninjutsuName}) を消費しました。");
            }
        }

        // 1つでも忍術を使ったら、クールダウンを更新し、インデックスとUIをリセット
        if (usedAtLeastOne)
        {
            lastUseTime = Time.time;

            // すべて使い切ったので、次スロットインデックスを SET_SIZE に設定
            nextSlotIndex = SET_SIZE;

            // UIを更新し、すべて空になったことを表示
            if (uiManager != null)
            {
                uiManager.UpdateUI(currentNinjutsuSet, nextSlotIndex);
            }
        }
        else
        {
            Debug.LogWarning("残りのスロットに使える忍術はありませんでした。");
        }
    }

    //public void UseAllRemainingNinjutsu()
    //{
    //    if (Time.time < lastUseTime + cooldownTime) return; // クールダウンチェック

    //    Debug.Log("【一斉発動】: 残りの忍術をすべて使います！");

    //    Vector3 direction = new Vector3(transform.localScale.x, 0, 0);
    //    bool usedAtLeastOne = false;

    //    // nextSlotIndexから最後までをチェック
    //    for (int i = nextSlotIndex; i < SET_SIZE; i++)
    //    {
    //        BaseNinjutsu usedNinjutsu = currentNinjutsuSet[i];

    //        if (usedNinjutsu != null)
    //        {
    //            // 忍術の発動ロジックを呼び出す
    //            usedNinjutsu.Activate(transform.position, direction, this.transform);

    //            // 発動後、スロットをクリア
    //            currentNinjutsuSet[i] = null;
    //            usedAtLeastOne = true;

    //            Debug.Log($"一斉発動！ Slot {i + 1} ({usedNinjutsu.ninjutsuName}) を消費しました。");
    //        }
    //    }

    //    // 1つでも忍術を使ったら、クールダウンを更新し、インデックスとUIをリセット
    //    if (usedAtLeastOne)
    //    {
    //        lastUseTime = Time.time;

    //        // すべて使い切ったので、次スロットインデックスを SET_SIZE に設定
    //        nextSlotIndex = SET_SIZE;

    //        // UIを更新し、すべて空になったことを表示
    //        if (uiManager != null)
    //        {
    //            uiManager.UpdateUI(currentNinjutsuSet, nextSlotIndex);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("残りのスロットに使える忍術はありませんでした。");
    //    }
    //}

    // 【Rキー】乱数を生成し、忍術セットを初期化する
    public void GenerateAndSetRandomNinjutsu()
    {
        if (allNinjutsuList == null || allNinjutsuList.Count < SET_SIZE)
        {
            Debug.LogError($"NinjutsuHandler: 忍術セットに必要なデータが不足しています。全忍術データ({allNinjutsuList?.Count ?? 0})が{SET_SIZE}個未満です。");
            return;
        }

        List<int> randomIndices = GenerateRandomIndices(allNinjutsuList.Count, SET_SIZE);

        // 1. 忍術セットの内容を決定し、currentNinjutsuSetを更新
        SetNinjutsuSetFromIndices(randomIndices);

        // 2. UI Managerに最終結果と全リストを渡し、★回転アニメーションを開始★
        if (uiManager != null)
        {
            // SetNinjutsuSetFromIndices で currentNinjutsuSet が更新されている
            uiManager.StartSlotSpin(currentNinjutsuSet, allNinjutsuList);
        }

        Debug.Log("新しい巻物セットが決定し、スロット回転が開始されました！");
    }


    // 内部用：重複を許して乱数インデックスを生成する関数
    private List<int> GenerateRandomIndices(int maxIndexExclusive, int count)
    {
        List<int> selectedIndices = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, maxIndexExclusive);
            selectedIndices.Add(randomIndex);
        }
        return selectedIndices;
    }

    // 内部用：乱数インデックスを受け取って忍術をセットするロジック (UI通知を含む)
    private void SetNinjutsuSetFromIndices(List<int> randomIndices)
    {
        if (randomIndices == null) return;

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

                if (randomIndex >= 0 && randomIndex < allNinjutsuList.Count)
                {
                    BaseNinjutsu originalNinjutsu = allNinjutsuList[randomIndex];

                    if (originalNinjutsu == null)
                    {
                        Debug.LogError($"[Randomizer ERROR] Index: {randomIndex} のデータが null です。");
                        continue;
                    }

                    // 複製してセット 
                    currentNinjutsuSet[i] = Instantiate(originalNinjutsu);

                    Debug.Log($"[Randomizer] Slot {i + 1} に {currentNinjutsuSet[i].ninjutsuName} が選ばれました。");
                }
            }
        }

        nextSlotIndex = 0;

        // UI Managerにセットの完了とインデックスを通知
        if (uiManager != null)
        {
            uiManager.UpdateUI(currentNinjutsuSet, nextSlotIndex);
        }
        Debug.Log("新しい巻物セットが完了しました！");
    }

    // --- デバッグ表示 ---

    void Update()
    {
        DisplayCurrentStockDebug();
    }


    private void DisplayCurrentStockDebug()
    {
        // ... (デバッグログのロジックは省略) ...
    }
}