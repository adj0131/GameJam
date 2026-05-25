using UnityEngine;

public class chainsawSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource sourceSFX;

    [SerializeField] AudioClip swingClip;
    [SerializeField] AudioClip deathClip;

    FireTankEnemy firetankControl;

    void Start()
    {
        firetankControl = GetComponent<FireTankEnemy>();
    }

    public void playSwingAudio()
    {
        sourceSFX.clip = swingClip;
        sourceSFX.Play();
    }
    public void playDeathAudio()
    {
        sourceSFX.clip = deathClip;
        sourceSFX.Play();
    }
}
