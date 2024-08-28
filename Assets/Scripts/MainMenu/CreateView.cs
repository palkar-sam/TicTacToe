using TMPro;
using UnityEngine;
using Views;

public class CreateView : BaseView
{
    [SerializeField] private TextMeshProUGUI inputText;

    public void CreateRoom()
    {
        NetworkManager.Instance.CreateRoom(inputText.text);
    }

    protected override void OnBackClick()
    {
        base.OnBackClick();
        NetworkManager.Instance.DisconnectToServer();
    }

}
