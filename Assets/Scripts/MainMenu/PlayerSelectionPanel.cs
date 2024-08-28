using UnityEngine;
using Views;

namespace MainMenu
{
    public class PlayerSelectionPanel : BaseView
    {
        [SerializeField] private GameSelectionMode selectionMode;
    }

    public enum GameSelectionMode
    {
        SINGLEPLAYER,
        MULTIPLAYER,
        TWOPLAYER
    }
}

