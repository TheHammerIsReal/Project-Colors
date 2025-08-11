using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    public Sound[] sounds;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }

        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void StopAllsounds()
    {
        foreach (var sound in sounds)
        {
            sound.source.Stop();
        }
    }
    public void Play(int id)
    {
        Sound s = sounds[id];
        if (s == null)
            return;
        s.source.Play();
    }
    public void Stop(int id)
    {
        Sound s = sounds[id];
        if (s == null || !s.source.isPlaying)
            return;
        s.source.Stop();
    }
    public void SetVolume(int id, float volume)
    {
        Sound s = sounds[id];
        if (s == null)
            return;
        s.source.volume = volume;
    }
    public void SetPitch(int id, float pitch)
    {
        Sound s = sounds[id];
        if (s == null)
            return;
        s.source.pitch = pitch;

    }
    public bool SoundIsPlaying(int soundId)
    {
        Sound s = sounds[soundId];

        if (s == null)
            return false;

        return s.source.isPlaying;
    }

    public void PlayRandom(int[] IdSounds)
    {
        int randomChance = Random.Range(0, IdSounds.Length);
        Play(IdSounds[randomChance]);
    }

    public static class Sounds
    {
        public const int Werewolf = 0;
        public const int Elite = 1;
        public const int Fire1 = 2;
        public const int Fire2 = 3;
        public const int Fire3 = 4;
        public const int Fire4 = 5;
        public const int Shield = 6;
        public const int Twisters = 7;
        public const int Zones = 8;
        public const int LifePotion = 9;
        public const int DieEnemies = 10;
        public const int Lotion = 11;
        public const int TheOpenDoor = 12;
        public const int InstancePowerUp = 13;
        public const int Burn = 14;
        public const int DmgEnemies = 15;
        public const int EnemyShoot = 16;
        public const int BossScream = 17;
        public const int HitShield = 18;
        public const int HitTarget = 19;
        public const int Hurt1 = 20;
        public const int Hurt2 = 21;
        public const int Jump1 = 22;
        public const int Jump2 = 23;
        public const int Jump3 = 24;
        public const int Jump4 = 25;
        public const int Jump5 = 26;
        public const int ShieldBreak = 27;
        public const int TouchLava = 28;
        public const int BossShoot1 = 29;
        public const int BossShoot2 = 30;
        public const int BossShoot3 = 31;
        public const int Ambience = 32;
        public const int PowerUpBullets = 33;
        public const int SpawnEnemies = 34;
        public const int Charge1 = 35;
        public const int Charge2 = 36;
        public const int Charge3 = 37;
    }
}
