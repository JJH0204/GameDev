public enum NoteType
{
    Apple,
    GoldApple,
    RottenApple,
    RainbowApple,
}

public enum InputType
{
    Throw,  // 버리기
    Catch,  // 담기
}

public enum SceneType
{
    None,   // 없음
    Init,   // 초기화 씬
    Title,  // 타이틀 씬
    Game,   // 게임 씬
}

public enum GameState
{
    None,       // 없음
    Init,       // 초기화
    Ready,      // 준비
    Playing,    // 플레이 중
    Pause,      // 일시 정지
    GameOver,   // 게임 오버
}