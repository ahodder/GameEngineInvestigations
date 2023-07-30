using Cyberstar.Core;
using Cyberstar.Engine.AssetManagement;
using Cyberstar.Engine.Logging;
using Raylib_cs;

namespace Cyberstar.Engine.Sounds;

public class AudioManager
{
    /// <summary>
    /// Whether or not the audio manager is currently playing background music.
    /// </summary>
    public bool IsPlayingBackgroundMusic => !string.IsNullOrEmpty(ActiveTrack); 
    
    /// <summary>
    /// The name of the currently playing track.
    /// </summary>
    public string ActiveTrack { get; private set; }
    
    private Music ActiveMusic { get; set; }

    /// <summary>
    /// The asset manager that is used to read our audio from.
    /// </summary>
    private readonly AssetManager _assetManager;
    
    public AudioManager(AssetManager assetManager)
    {
        _assetManager = assetManager;
        Raylib.InitAudioDevice();
    }

    public void PerformTick(FrameTiming frameTiming)
    {
        if (IsPlayingBackgroundMusic)
        {
            Raylib.UpdateMusicStream(ActiveMusic);
        }
    }

    public void PlayMusic(string musicFileName, bool shouldRepeat = false)
    {
        if (!_assetManager.TryGetAudioTrack(musicFileName, out Music music))
        {
            Log.Error($"Failed to play audio track: {musicFileName}");
            return;
        }

        music.looping = shouldRepeat;
        ActiveTrack = musicFileName;
        ActiveMusic = music;
        Raylib.PlayMusicStream(music);
    }

    public void StopMusic()
    {
        if (IsPlayingBackgroundMusic)
        {
            Raylib.StopMusicStream(ActiveMusic);
            ActiveTrack = null;
            ActiveMusic = default;
        }
    }
}