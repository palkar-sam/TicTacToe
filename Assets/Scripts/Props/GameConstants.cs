using System.Collections;
using UnityEngine;

namespace Props
{
    public class GameConstants
    {
    }

    public enum GameEvents
    {
        NONE = 0,
        ON_ROUND_STARTED,
        ON_ROUND_COMPLETE,
        ON_SHOW_MP_CREATEROOM,
        ON_SHOW_MP_JOINROOM,
        ON_DISCONNECTED,
        ON_ROOM_LIST_UPDATE
    }

    public enum ButtonName
    {
        SinglePlayer,
        Multiplayer,
        TwoPlayer
    }
}