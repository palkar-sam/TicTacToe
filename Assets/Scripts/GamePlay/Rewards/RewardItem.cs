using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    [SerializeField] private Text RewardText;

    public void SetData(int count)
    {
        RewardText.text = count.ToString();
    }
}
