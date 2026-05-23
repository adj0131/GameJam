using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource sourceLoop;
    [SerializeField] AudioSource sourceSFX;

    [SerializeField] AudioClip walkClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip shootClip;

    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void ManageWalkAudio()
    {
        print(playerMovement.isRunning);
        sourceLoop.clip = walkClip;

        if (playerMovement.isRunning)
        {
            if(!sourceLoop.isPlaying)
            {
                print("starting play");
                sourceLoop.Play();
            }
        }
        else
        {
            if (sourceLoop.isPlaying)
            {
                print("stopping play");
                sourceLoop.Stop();
            }
        }
    }
    public void playJumpAudio()
    {
        sourceSFX.clip = jumpClip;
        sourceSFX.Play();
    }
    public void playShootAudio()
    {
        sourceSFX.clip = shootClip;
        sourceSFX.Play();
    }
}
