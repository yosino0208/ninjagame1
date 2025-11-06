using UnityEngine;
using System.Collections.Generic;

// 忍術のランダム選択ロジックを提供する静的ヘルパークラス
public static class NinjutsuRandomizer
{
    private const int NUMBER_OF_SLOTS = 3;

    /// <summary>
    /// 全忍術リストのサイズに基づき、3つのランダムなインデックスを生成する
    /// </summary>
    /// <param name="maxIndex">乱数生成の最大値（全忍術リストのCount - 1）</param>
    /// <returns>生成された3つのランダムなインデックスのリスト</returns>
    public static List<int> GenerateRandomNinjutsuIndices(int maxIndex)
    {
        if (maxIndex < 0)
        {
            // 静的クラスなのでDebug.LogErrorを使用
            Debug.LogError("NinjutsuRandomizer: ランダム生成の最大インデックスが不正です。");
            return new List<int>();
        }

        List<int> randomIndices = new List<int>();

        for (int i = 0; i < NUMBER_OF_SLOTS; i++)
        {
            // Random.Range(min, max) は max を含まないため、+1 するか maxIndex をそのまま渡す
            int randomIndex = Random.Range(0, maxIndex + 1);
            randomIndices.Add(randomIndex);
        }

        return randomIndices;
    }
}