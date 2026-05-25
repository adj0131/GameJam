using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource sourceLoop;
    [SerializeField] AudioSource sourceSFX;

    [SerializeField] AudioClip walkClip;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip celebrationClip;

    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void ManageWalkAudio()
    {
        sourceLoop.clip = walkClip;

        if (playerMovement.isRunning && playerMovement.isTouchingGround)
        {
            if(!sourceLoop.isPlaying)
            {
                sourceLoop.Play();
            }
        }
        else
        {
            if (sourceLoop.isPlaying)
            {
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
    public void playCelebrateAudio()
    {
        sourceSFX.clip = celebrationClip;
        sourceSFX.Play();
    }
}
