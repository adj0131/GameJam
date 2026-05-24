using UnityEngine;
using UnityEngine.Video;

public class MovieController : MonoBehaviour
{
    SceneControl sceneController;
    VideoPlayer videoPlayer;

    void Start()
    {
        sceneController = FindFirstObjectByType<SceneControl>();
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.isLooping = false;

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.Prepare();
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnPrepared;

        vp.loopPointReached += OnVideoFinished;
        vp.Play();
    }
    void OnVideoFinished(VideoPlayer vp)
    {
        sceneController.LoadNextScene();
    }
}
