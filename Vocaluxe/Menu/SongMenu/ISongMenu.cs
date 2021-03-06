﻿using System;
using System.Collections.Generic;
using System.Text;

using Vocaluxe.Base;
using Vocaluxe.Lib.Draw;

namespace Vocaluxe.Menu.SongMenu
{
    interface ISongMenu : IMenuElement
    {
        void Update();
        void OnShow();
        void OnHide();

        void HandleInput(ref KeyEvent KeyEvent);
        void HandleMouse(ref MouseEvent MouseEvent);
        void Draw();

        int GetSelectedSong();
        void SetSelectedSong(int VisibleSongNr);

        int GetSelectedCategory();
        int GetActualSelection();

        bool IsSelected();
        void SetSelected(bool Selected);

        bool IsVisible();
        void SetVisible(bool Visible);

        SRectF GetRect();
    }
}
