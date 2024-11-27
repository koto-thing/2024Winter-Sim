namespace Player
{
    public enum PlayerHorizontalMovementState
    {
        RIGHT,
        LEFT,
        STOP
    }
    
    public enum PlayerVerticalMovementState
    {
        JUMP,
        FALL, // ジャンプ後の落下
        DOWN, // 落下中
        GROUND
    }
}