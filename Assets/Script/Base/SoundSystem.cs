using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField]
    private AudioClip collectCrystal;
    [SerializeField]
    private AudioClip killEnemy;
    [SerializeField]
    private AudioClip forAway;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void SoundCollect()
    {
        source.PlayOneShot(collectCrystal);
    }

    public void SoundKill()
    {
        source.PlayOneShot(killEnemy);
    }

    public void SoundAway()
    {
        source.PlayOneShot(forAway);
    }
}
