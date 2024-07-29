using Gameplay.Rewards;
using System.Collections;
using System.Collections.Generic;
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
        descText.text = data.Winner.ToString(); 
    }
}
