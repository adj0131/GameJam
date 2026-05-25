using UnityEngine;

public class FiretankSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource sourceLoop;
    [SerializeField] AudioSource sourceSFX;

    [SerializeField] AudioClip walkClip;
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip deathClip;

    FireTankEnemy firetankControl;

    void Start()
    {
        firetankControl = GetComponent<FireTankEnemy>();
    }

    public void ManageWalkAudio()
    {
        sourceLoop.clip = walkClip;

        if (firetankControl.isMoving)
        {
            if (!sourceLoop.isPlaying)
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
    public void playShootAudio()
    {
        sourceSFX.clip = shootClip;
        sourceSFX.Play();
    }
    public void playDeathAudio()
    {
        sourceSFX.clip = shootClip;
        sourceSFX.Play();
    }
}
