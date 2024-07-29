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
        ON_ROUND_COMPLETE
    }

    public enum ButtonName
    {
        SinglePlayer,
        Multiplayer,
        TwoPlayer
    }
}