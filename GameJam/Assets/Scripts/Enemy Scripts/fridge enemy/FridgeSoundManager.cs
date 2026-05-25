using UnityEngine;

public class FridgeSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource sourceLoop;
    [SerializeField] AudioSource sourceSFX;

    [SerializeField] AudioClip walkClip;
    [SerializeField] AudioClip shootClip;
    [SerializeField] AudioClip swingClip;
    [SerializeField] AudioClip deathClip;

    FridgeEnemy fridgeControl;

    void Start()
    {
        fridgeControl = GetComponent<FridgeEnemy>();
    }

    public void ManageWalkAudio()
    {
        sourceLoop.clip = walkClip;

        if (fridgeControl.isMoving)
        {
            if (!sourceLoop.isPlaying)
            {
                sourceLoop.Play();
                print("playing walk");
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
        sourceSFX.clip = deathClip;
        sourceSFX.Play();
    }
    public void playSwingAudio()
    {
        sourceSFX.clip = swingClip;
        sourceSFX.Play();
    }
}
