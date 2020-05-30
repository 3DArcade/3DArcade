namespace Arcade
{
    public enum ArcadeType
    {
        FpsArcade, CylArcade, FpsMenu, CylMenu, None
    }

    public enum ArcadeStates
    {
        LoadingAssets, LoadingArcade, ArcadeMenu, SettingsMenu, GeneralConfigurationmenu, ArcadesConfigurationMenu, AlertArcadesConfigurationMenuError, EmulatorsConfigurationMenu, AlertEmulatorsConfigurationMenuError, DialogAddEmulatorConfiguration, AlertAddEmulatorConfigurationError, DialogUpdateMasterGamelist, Running, MoveCabs, MoveCabsAdd, MoveCabsEdit, MoveCabsDelete, Information, Game
    }

    public enum GameLauncherMethod
    {
        None, Internal, External, URL, ArcadeConfiguration
    }

    public enum ModelFilterOperator
    {
        Equals, NotEquals, Contains, NotContains, Starts, NotStarts, Larger, Smaller
    }

    public enum ModelVideoEnabled
    {
        Always, VisiblePlay, VisibleUnobstructed, VisibleEnable, Selected, Never
    }

    public enum ModelType
    {
        Arcade, Game, Prop
    }

    public enum ModelComponentType
    {
        Marquee, Screen, Generic
    }

    public enum OS
    {
        MacOS, iOS, tvOS, Windows, Linux, Android
    }

    public enum TriggerEvent
    {
        ModelSelected, ModelDeSelected, ModelSelectedChanged, ModelCollisionEnter, ModelCollisionExit, GameStarted, GameEnded, NewGameSelected, MainMenuStarted, ArcadeStarted, ArcadeEnded, ArcadeStateChanged, MenuStarted, MenuEnded
    }

    public enum TriggerAction
    {
        PlayAnimation, PauseAnimation, StopAnimation, PlayAudio, PauseAudio, StopAudio, SetActiveEnabled, SetActiveDisabled, SetParent, SetTransform, GetArtworkFromSelectedModel
    }
}
