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
        ON_ROOM_LIST_UPDATE,
        ON_SHOW_SETTINGS_VIEW,
        ON_SHOW_VIEW,
        ON_CLOSE_VIEW
    }

    public enum ScreenType
    {
        None,
        HomeView,
        SettingView,
        RewardView,
        JoinView,
        CreateView
    }

    public enum ButtonName
    {
        SinglePlayer,
        Multiplayer,
        TwoPlayer
    }

    public enum NetworkEvents
    {
        MOVE_EVENT,
        START_ROUND_EVENT
    }
}