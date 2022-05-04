using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Soundlar
{
    PickUp,
    CheckpointPickUp,
    PickUpGem,
    LevelComplete,
    LevelFailed

}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private List<AudioSource> sounds = new List<AudioSource>();
    [SerializeField] private List<AudioSource> musics = new List<AudioSource>();

    private Soundlar soundlar;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    void Start()
    {
        //Get Sounds

        for (int i = 0; i < transform.childCount; i++) 
        {
            sounds.Add(transform.GetChild(i).GetComponent<AudioSource>());
        }

        //Transform musicObject = transform.GetChild(0);
        //for (int i = 0; i < musicObject.childCount; i++)
        //{
        //    musics.Add(musicObject.GetChild(i).GetComponent<AudioSource>());
        //}
        //if(musics.Count > 0)
        //    PlayRandomMusic();
    }

    private void PlayRandomMusic()
    {
        int rndmMusic = Random.Range(0, musics.Count);
        musics[rndmMusic].Play();

    }

   

    public void PlaySound(Soundlar soundfx)
    {
        soundlar = soundfx;
        SoundsIndexes();
    }

    private void SoundsIndexes()
    {
        switch (soundlar)
        {
            case Soundlar.PickUp:
                sounds[0].Play();
                break;
            case Soundlar.CheckpointPickUp:
                sounds[1].Play();
                break;
            case Soundlar.PickUpGem:
                sounds[2].Play();
                break;
            case Soundlar.LevelComplete:
                sounds[3].Play();
                break;
            case Soundlar.LevelFailed:
                sounds[4].Play();
                break;
            default:
                break;
        }
    }

}
