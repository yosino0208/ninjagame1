// IPlayerMoverStatus.cs
public interface IPlayerMoverStatus
{
    // 移動速度を取得するためのプロパティ
    float MoveSpeed { get; }

    // ジャンプ力を取得するためのプロパティ
    float JumpPower { get; }
}