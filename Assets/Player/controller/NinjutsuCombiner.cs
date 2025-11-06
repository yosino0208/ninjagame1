using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 複合技の判定と実行を担うマネージャー（シングルトン）
public class NinjutsuCombiner : MonoBehaviour
{
    private const int SET_SIZE = 3;

    // シングルトンインスタンス
    public static NinjutsuCombiner Instance { get; private set; }

    [System.Serializable]
    public class ComboDataEntry
    {
        public NinjutsuElement type;
        public CompositeNinjutsuData comboData;
    }

    [Header("複合技データ設定")]
    public List<ComboDataEntry> comboDataList;

    private Dictionary<NinjutsuElement, CompositeNinjutsuData> comboMap;


    void Awake()
    {
        // シングルトン初期化
        if (Instance == null)
        {
            Instance = this;
            // シーンを跨いで維持する場合: DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // リストを高速検索用のDictionaryに変換
        if (comboDataList != null)
        {
            comboMap = comboDataList.ToDictionary(e => e.type, e => e.comboData);
        }
        else
        {
            comboMap = new Dictionary<NinjutsuElement, CompositeNinjutsuData>();
            Debug.LogError("NinjutsuCombiner: Combo Data Listが設定されていません。");
        }
    }

    // 外部から静的に呼び出されるエントリポイント
    public static bool TryActivateCombo(
        NinjutsuElement[] elements,
        Vector3 originPosition,
        Vector3 direction,
        Transform caster)
    {
        if (Instance == null)
        {
            Debug.LogError("NinjutsuCombinerがシーンに存在しません。");
            return false;
        }

        // 1. 各要素の出現回数をカウント
        Dictionary<NinjutsuElement, int> counts = elements
            .Where(e => e != NinjutsuElement.None)
            .GroupBy(e => e)
            .ToDictionary(g => g.Key, g => g.Count());

        if (counts.Count == 0) return false;

        // A. 最高優先度: 単色3連 (Triple Combo)
        if (counts.Count == 1 && counts.First().Value == 3)
        {
            NinjutsuElement type = counts.First().Key;
            // インスタンスメソッドを呼び出す
            return Instance.TryActivateTripleCombo(type, originPosition, direction, caster);
        }

        return false;
    }


    private bool TryActivateTripleCombo(NinjutsuElement type, Vector3 originPosition, Vector3 direction, Transform caster)
    {
        if (comboMap.TryGetValue(type, out CompositeNinjutsuData comboData))
        {
            if (comboData != null)
            {
                comboData.Activate(originPosition, direction, caster);
                return true;
            }
        }

        Debug.LogWarning($"[NinjutsuCombiner] 複合技データが設定されていません: {type}");
        return false;
    }
}