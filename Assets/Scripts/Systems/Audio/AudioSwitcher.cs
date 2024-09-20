public class AudioSwitcher : OneActionPerformer
{
    private AudioPlayer _AudioPlayer;

    public AudioSwitcher(AudioPlayer audioPlayer)
    {
        _AudioPlayer = audioPlayer;
    }

    public override void PerformAction()
    {
        _AudioPlayer.AudioEnabledStateHandler.SwitchEnableState();
    }
}