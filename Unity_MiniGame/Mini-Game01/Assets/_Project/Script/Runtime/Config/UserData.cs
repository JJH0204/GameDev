public class UserData
{
    public int bestScore { get; set; }  // 최고 점수
    public int bestCombo { get; set; }  // 최고 콤보 
    public int highCombo { get; set; }          // 최고 콤보 (콤보가 끊겼을 때 저장)
    public int currentScore { get; set; }       // 현재 점수
    public int currentCombo { get; set; }       // 현재 콤보

    public void Init(HistoryData instanceHistoryData)
    {
        bestScore = instanceHistoryData.BestScore;
        bestCombo = instanceHistoryData.BestCombo;
    }
}