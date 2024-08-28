using Gameplay.Rewards;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class SummaryView : BaseView
{
    [SerializeField] private Text descText;
    [SerializeField] private RewardItem rewardItem;

    public void SetData(RewardsData data)
    {
        rewardItem.SetData(data.Coins);
        descText.text = data.Type == Board.BoardValidType.WIN ? data.Winner.ToString()
            : data.Type == Board.BoardValidType.DRAW ? "DRAW!" : "---";
    }
}
