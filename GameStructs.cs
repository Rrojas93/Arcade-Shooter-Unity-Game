//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public class GameStructs
{
    //used to make it easy to refer the correct clip without worrying about spelling errors (through intellisense)
    public struct ClipName
    {
        public const string GameMusic = "BackGroundMusic";
        public const string PistolFire = "PistolFire";
        public const string HolsterWeapon = "Holster";
        public const string SmgReload = "SMGReload";
        public const string ShotgunFire = "ShotgunFire";
        public const string ShotgunReload = "ShotgunReload";
        public const string Shotgun = "ShotgunCock";
        public const string TitleMusic = "IntroSong";
        public const string ZombieDeath = "ZombieDeath";
        public const string NecroDeath = "NecroDeath";
        public const string DynoDeath = "DynoDeath";
        public const string PlayerDeath = "PlayerDeath";
        public const string PlayerDeathMusic = "PlayerDeathMusic";
        public const string ButtonHoverEnter = "ButtonHoverEnter";
        public const string ButtonClick = "ButtonClick";

    }

    public struct PrefKeys
    {
        public const string MusicOn = "MusicOn";
        public const string FromGameScene = "FromGameScene";
        public const string PlayerScore = "PlayerScore";
    }

}

