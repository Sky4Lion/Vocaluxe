﻿using System;
using System.Collections.Generic;
using System.Text;

using Vocaluxe.Base;
using Vocaluxe.Lib.Song;

namespace Vocaluxe.GameModes
{
    public enum EGameMode
    {
        Normal,
        Medley,
        Duet
    }

    interface IGameMode
    {
        void Init();

        EGameMode GetCurrentGameMode();

        bool AddVisibleSong(int VisibleIndex, EGameMode GameMode);
        bool AddSong(int AbsoluteIndex, EGameMode GameMode);
        bool RemoveVisibleSong(int VisibleIndex);
        bool RemoveSong(int AbsoluteIndex);
        void ClearSongs();

        void Reset();
        void Start(SPlayer[] Player);
        void NextRound(SPlayer[] Player);
        bool IsFinished();
        int GetCurrentRoundNr();
        
        CPoints GetPoints();

        int GetNumSongs();
        CSong GetSong();
        CSong GetSong(int Num);
    }
}
