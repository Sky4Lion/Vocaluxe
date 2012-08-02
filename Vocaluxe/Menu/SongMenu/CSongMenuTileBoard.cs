using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Vocaluxe.Base;
using Vocaluxe.Lib.Draw;
using Vocaluxe.Lib.Song;

namespace Vocaluxe.Menu.SongMenu
{
    class CSongMenuTileBoard : CSongMenuFramework
    {
        private SRectF _ScrollRect;
        private List<CStatic> _Tiles;
        private CStatic _CoverBig;
        private CStatic _TextBG;
        private CStatic _DuetIcon;
        private CStatic _VideoIcon;

        //SkyLion add start:
        private CStatic _SongRatingFalse;
        private CStatic _SongRatingTrue_1;
        private CStatic _SongRatingTrue_2;
        private CStatic _SongRatingTrue_3;
        private CStatic _SongRatingTrue_4;
        private CStatic _SongRatingTrue_5;
        private CStatic _SongRatingTrue_6;
        private CStatic _SongRatingTrue_7;
        private CStatic _SongRatingTrue_8;
        private CStatic _SongRatingTrue_9;
        private CStatic _SongRatingTrue_0;

        private short _SongRatingImport = 1000;
        private short _SongRatingIndex;
        //SkyLion add end
		
		private STexture _CoverBigTexture;
        private STexture _CoverTexture;

        private CText _Artist;
        private CText _Title;
        private CText _SongLength;
        
        private int _NumW;
        private int _NumH;

        private float _SpaceW;
        private float _SpaceH;

        private int _TileW;
        private int _TileH;

        private int _Offset = 0;
        private int _actualSelection = -1;

        public override int GetActualSelection()
        {
            return _actualSelection;
        }

        public override void Init()
        {
            base.Init();

            _NumW = _Theme.songMenuTileBoard.numW;
            _NumH = _Theme.songMenuTileBoard.numH;
            _SpaceW = _Theme.songMenuTileBoard.spaceW;
            _SpaceH = _Theme.songMenuTileBoard.spaceH;

            _PendingTime = 100L;

            _TileW = (int)((_Theme.songMenuTileBoard.TileRect.W - _SpaceW * (_NumW - 1)) / _NumW);
            _TileH = (int)((_Theme.songMenuTileBoard.TileRect.H - _SpaceH * (_NumH - 1)) / _NumH);

            _CoverTexture = CTheme.GetSkinTexture(_Theme.CoverBackgroundName);
            _CoverBigTexture = CTheme.GetSkinTexture(_Theme.CoverBigBackgroundName);

            _Tiles = new List<CStatic>();
            for (int i = 0; i < _NumH; i++)
            {
                for (int j = 0; j < _NumW; j++)
                {
                    SRectF rect = new SRectF(_Theme.songMenuTileBoard.TileRect.X + j * (_TileW + _SpaceW),
                        _Theme.songMenuTileBoard.TileRect.Y + i * (_TileH + _SpaceH), _TileW, _TileH, _Rect.Z);
                    CStatic tile = new CStatic(_CoverTexture, Color, rect);
                    _Tiles.Add(tile);
                }
            }

            _ScrollRect = new SRectF(0, 0, CSettings.iRenderW, CSettings.iRenderH, _Theme.songMenuTileBoard.TileRect.Z);

            _PreviewSelected = -1;
            _Offset = 0;

            _CoverBig = _Theme.songMenuTileBoard.StaticCoverBig;
            _TextBG = _Theme.songMenuTileBoard.StaticTextBG;
            _DuetIcon = _Theme.songMenuTileBoard.StaticDuetIcon;
            _VideoIcon = _Theme.songMenuTileBoard.StaticVideoIcon;

            //SkyLion add start:
            _SongRatingFalse = _Theme.songMenuTileBoard.StaticSongRatingFalse;
            _SongRatingTrue_1 = _Theme.songMenuTileBoard.StaticSongRatingTrue_1;
            _SongRatingTrue_2 = _Theme.songMenuTileBoard.StaticSongRatingTrue_2;
            _SongRatingTrue_3 = _Theme.songMenuTileBoard.StaticSongRatingTrue_3;
            _SongRatingTrue_4 = _Theme.songMenuTileBoard.StaticSongRatingTrue_4;
            _SongRatingTrue_5 = _Theme.songMenuTileBoard.StaticSongRatingTrue_5;
            _SongRatingTrue_6 = _Theme.songMenuTileBoard.StaticSongRatingTrue_6;
            _SongRatingTrue_7 = _Theme.songMenuTileBoard.StaticSongRatingTrue_7;
            _SongRatingTrue_8 = _Theme.songMenuTileBoard.StaticSongRatingTrue_8;
            _SongRatingTrue_9 = _Theme.songMenuTileBoard.StaticSongRatingTrue_9;
            _SongRatingTrue_0 = _Theme.songMenuTileBoard.StaticSongRatingTrue_0;
            //SkyLion add end
			
			_Artist = _Theme.songMenuTileBoard.TextArtist;
            _Title = _Theme.songMenuTileBoard.TextTitle;
            _SongLength = _Theme.songMenuTileBoard.TextSongLength;
        }
       
        public override void OnShow()
        {
            if (CSongs.Tabs == EOffOn.TR_CONFIG_OFF && CSongs.NumCategories > 0 && CSongs.Category == -1)
            {
                EnterCategory(0);
            }
            _actualSelection = -1;
            _Locked = -1;
            _PreviewSelected = -1;
            UpdateList(0);
            //AfterCategoryChange();
            SetSelectedSong(_ActSong);
            AfterCategoryChange();
            CSongs.UpdateRandomSongList();

            int actcat = _PreviewSelected;
            if ((CSongs.NumCategories > 0) && (actcat < 0))
            {
                _CoverBig.Texture = CSongs.Categories[0].CoverTextureSmall;
                _Artist.Text = CSongs.Categories[0].Name;
                _Title.Text = String.Empty;
                _SongLength.Text = String.Empty;
                _PreviewSelected = 0;
                _Locked = 0;
                _DuetIcon.Visible = false;
                _VideoIcon.Visible = false;
            }

            if (CSongs.NumVisibleSongs == 0 && CSongs.SearchFilter != String.Empty)
            {
                _CoverBig.Texture = _CoverBigTexture;
                _Artist.Text = String.Empty;
                _Title.Text = String.Empty;
                _SongLength.Text = String.Empty;
                _PreviewSelected = -1;
                _Locked = -1;
                _DuetIcon.Visible = false;
                _VideoIcon.Visible = false;
            }
        }

        public override void HandleInput(ref KeyEvent KeyEvent)
        {
            base.HandleInput(ref KeyEvent);

            if (KeyEvent.KeyPressed)
            {
                //
            }
            else
            {
                bool sel = false;
                foreach (CStatic tile in _Tiles)
                {
                    if (tile.Selected)
                    {
                        sel = true;
                        break;
                    }
                }

                if ((_Locked == -1 || !sel) &&
                    KeyEvent.Key != Keys.Escape && KeyEvent.Key != Keys.Back && KeyEvent.Key != Keys.PageUp && KeyEvent.Key != Keys.PageDown)
                {
                    if (_PreviewSelected > -1)
                    {
                        _Locked = _PreviewSelected;
                    }
                    else
                    {
                        _Locked = 0;
                        _actualSelection = 0;
                        _PreviewSelected = 0;
                        SetSelectedNow();
                        UpdateList(0);
                    }
                }
                else
                {
                    switch (KeyEvent.Key)
                    {
                        case Keys.Enter:
                            if (CSongs.Category < 0)
                            {
                                EnterCategory(_PreviewSelected);
                                KeyEvent.Handled = true;
                            }
                            else if (_actualSelection > -1 && _PreviewSelected >= 0)
                            {
                                _Locked = _actualSelection;
                            }
                            break;

                        case Keys.Escape:
                        case Keys.Back:
                            if (CSongs.Category > -1 && CSongs.Tabs == EOffOn.TR_CONFIG_ON)
                            {
                                ShowCategories();
                                KeyEvent.Handled = true;
                            }
                            //else if (CSongs.Tabs == EOffOn.TR_CONFIG_OFF)
                            //{
                            //    CGraphics.FadeTo(EScreens.ScreenMain);
                            //}
                            break;

                        case Keys.PageUp:
                            if (CSongs.Tabs == EOffOn.TR_CONFIG_ON)
                            {
                                PrevCategory();
                            }
                            break;

                        case Keys.PageDown:
                            if (CSongs.Tabs == EOffOn.TR_CONFIG_ON)
                            {
                                NextCategory();
                            }
                            break;

                        case Keys.Left:
                            if (_Locked > 0)
                            {
                                _Locked--;
                                if (CSongs.Category < 0 && _Locked < CSongs.NumCategories - _NumW ||
                                    CSongs.Category >= 0 && _Locked < CSongs.NumVisibleSongs - _NumW)
                                    UpdateList((_Locked / _NumW) * _NumW - (_NumW * (_NumH - 2)));
                            }
                            break;

                        case Keys.Right:
                            if (CSongs.Category < 0)
                            {
                                if (_Locked < CSongs.NumCategories - 1)
                                {
                                    _Locked++;
                                    if (_Locked < CSongs.NumCategories - _NumW)
                                        UpdateList((_Locked / _NumW) * _NumW - (_NumW * (_NumH - 2)));
                                }

                            }
                            else
                            {
                                if (_Locked < CSongs.NumVisibleSongs - 1)
                                {
                                    _Locked++;
                                    if (_Locked < CSongs.NumVisibleSongs - _NumW)
                                        UpdateList((_Locked / _NumW) * _NumW - (_NumW * (_NumH - 2)));
                                }
                            }
                            break;

                        case Keys.Up:
                            if (KeyEvent.ModSHIFT && CSongs.Tabs == EOffOn.TR_CONFIG_ON)
                            {
                                PrevCategory();
                                break;
                            }
                            
                            if (_Locked > _NumW - 1)
                            {
                                _Locked -= _NumW;
                                UpdateList((_Locked / _NumW) * _NumW - (_NumW * (_NumH - 2)));
                            }
                            break;

                        case Keys.Down:
                            if (KeyEvent.ModSHIFT && CSongs.Tabs == EOffOn.TR_CONFIG_ON)
                            {
                                NextCategory();
                                break;
                            }

                            if (CSongs.Category < 0)
                            {
                                if (_Locked < CSongs.NumCategories - _NumW)
                                {
                                    _Locked += _NumW;
                                    if (_Locked < CSongs.NumCategories - _NumW)
                                        UpdateList((_Locked / _NumW) * _NumW - (_NumW * (_NumH - 2)));
                                }

                            }
                            else
                            {
                                if (_Locked < CSongs.NumVisibleSongs - _NumW)
                                {
                                    _Locked += _NumW;
                                    if (_Locked < CSongs.NumVisibleSongs - _NumW)
                                        UpdateList((_Locked / _NumW) * _NumW - (_NumW * (_NumH - 2)));
                                }
                            }
                            break;

                    }
                }

                if (!KeyEvent.Handled)
                {
                    _PreviewSelected = _Locked;
                    _actualSelection = _Locked;

                    for (int i = 0; i < _Tiles.Count; i++)
                    {
                        _Tiles[i].Selected = _Locked == i + _Offset;
                    }
                }
            }  
        }

        public override void HandleMouse(ref MouseEvent MouseEvent)
        {
            base.HandleMouse(ref MouseEvent);

            int i = 0;
            bool sel = false;
            foreach (CStatic tile in _Tiles)
            {
                if ((tile.Texture.index != _CoverTexture.index) && CHelper.IsInBounds(tile.Rect, MouseEvent) && !sel)
                {
                    if (MouseEvent.LB || CSongs.Category == -1)
                    {
                        if (_PreviewSelected == i + _Offset)
                            _Locked = _PreviewSelected;
                        else
                        {
                            _PreviewSelected = i + _Offset;
                            _Locked = -1;
                        }
                    }
                    tile.Selected = true;
                    _actualSelection = i + _Offset;
                    sel = true;
                }
                else
                {
                    tile.Selected = false;
                }
                i++;
            }

            if (!sel)
                _actualSelection = -1;

            if ((MouseEvent.RB) && (CSongs.NumCategories > 0) && CSongs.Category >= 0 && CSongs.Tabs == EOffOn.TR_CONFIG_ON)
            {
                ShowCategories();
                return;
            }
            else if (MouseEvent.RB && CSongs.Tabs == EOffOn.TR_CONFIG_OFF)
            {
                CGraphics.FadeTo(EScreens.ScreenMain);
            }
            else if (_PreviewSelected != -1 && MouseEvent.LB && CSongs.Category != -1)
            {
                if (CHelper.IsInBounds(_CoverBig.Rect, MouseEvent) || CHelper.IsInBounds(_TextBG.Rect, MouseEvent))
                {
                    _Locked = _PreviewSelected;
                }
            }
            else if ((MouseEvent.LB) && (CSongs.Category == -1))
            {
                foreach (CStatic tile in _Tiles)
                {
                    if ((tile.Texture.index != _CoverTexture.index) && CHelper.IsInBounds(tile.Rect, MouseEvent))
                    {
                        EnterCategory(_PreviewSelected);
                        return;
                    }
                }
            }

            if (MouseEvent.Wheel > 0)
            {
                if (CHelper.IsInBounds(_ScrollRect, MouseEvent))
                {
                    if (CSongs.Category >= 0 && CSongs.NumVisibleSongs > _Offset + _NumW * MouseEvent.Wheel + _NumW * (_NumH - 1) ||
                        CSongs.Category < 0 && CSongs.NumCategories > _Offset + _NumW * MouseEvent.Wheel + _NumW * (_NumH - 1))
                    {
                        _Offset += _NumW * MouseEvent.Wheel;
                        UpdateList(_Offset);
                    }
                }
            }

            if (MouseEvent.Wheel < 0)
            {
                if (CHelper.IsInBounds(_ScrollRect, MouseEvent))
                {
                    _Offset += _NumW * MouseEvent.Wheel;
                    if (_Offset < 0)
                    {
                        _Offset = 0;
                    }
                    UpdateList(_Offset);
                }
            }
        }

        public override void Draw()
        {
            foreach (CStatic tile in _Tiles)
            {
                if (tile.Selected)
                    tile.Draw(1.2f, tile.Rect.Z - 0.1f, true, false);
                else
                {
                    if (tile.Texture.index != _CoverTexture.index)
                        tile.Draw(1f, tile.Rect.Z, true, false);
                    else
                        tile.Draw(1f, tile.Rect.Z, false, false);
                }
            }

            if (CSongs.Category >= 0)
            {
                int actsong = _PreviewSelected;
                if ((CSongs.NumVisibleSongs > actsong) && (actsong >= 0))
                {
                    CSong song = CSongs.VisibleSongs[actsong];

                    _CoverBig.Texture = song.CoverTextureSmall;
                    _Artist.Text = song.Artist;
                    _Title.Text = song.Title;
                    _DuetIcon.Visible = song.IsDuet;
                    if (song.VideoFileName.Length > 0)
                    {
                        _VideoIcon.Visible = true;
                    }
                    else
                    {
                        _VideoIcon.Visible = false;
                    }

                    float Time = CSound.GetLength(_SongStream);
                    if (song.Finish != 0)
                        Time = song.Finish;

                    Time -= song.Start;
                    int min = (int)Math.Floor(Time / 60f);
                    int sec = (int)(Time - min * 60f);
                    _SongLength.Text = min.ToString("00") + ":" + sec.ToString("00");
                }
            }
            else
            {
                int actcat = _PreviewSelected;
                if ((CSongs.NumCategories > actcat) && (actcat >= 0))
                {
                    _CoverBig.Texture = CSongs.Categories[actcat].CoverTextureSmall;
                    _Artist.Text = CSongs.Categories[actcat].Name;

                    int num = CSongs.NumSongsInCategory(actcat);
                    if (num != 1)
                        _Title.Text = CLanguage.Translate("TR_SCREENSONG_NUMSONGS").Replace("%v", num.ToString());
                    else
                        _Title.Text = CLanguage.Translate("TR_SCREENSONG_NUMSONG").Replace("%v", num.ToString());

                    _SongLength.Text = String.Empty;
                    _DuetIcon.Visible = false;
                    _VideoIcon.Visible = false;
                }
            }


            _CoverBig.Draw(1f, true);
            _TextBG.Draw();

            if (_vidtex.index != -1 && _Video != -1)
            {
                RectangleF bounds = new RectangleF(_CoverBig.Rect.X, _CoverBig.Rect.Y, _CoverBig.Rect.W, _CoverBig.Rect.H);
                RectangleF rect = new RectangleF(0f, 0f, _vidtex.width, _vidtex.height);
                CHelper.SetRect(bounds, ref rect, rect.Width / rect.Height, EAspect.Crop);

                CDraw.DrawTexture(_vidtex, new SRectF(rect.X, rect.Y, rect.Width, rect.Height, _CoverBig.Rect.Z),
                    _vidtex.color, new SRectF(bounds.X, bounds.Y, bounds.Width, bounds.Height, 0f), false); 
            }
			
			
			//SkyLion add start:
            // Sky comment: Berechnet die passende Bewertung (in Prozent)
            // Sky comment: zulässige Bewertungen zwischen 0 und 1000; 1000 entspricht 100%
            if (_SongRatingImport <= 0)
            {
                _SongRatingImport = -1;
            }
            //Sky comment: falls Zahl zu groß (1000 ist gerade auch noch zu groß)
            if (_SongRatingImport >= 1000)
            {
                _SongRatingImport = 999;
            }
            // Sky comment: /100 als vorbereitung für switch case (unten); Floor rundet immer ab; Convert sorgt für eine gültige int-Zahl
            _SongRatingIndex = Convert.ToInt16(Math.Floor((decimal)_SongRatingImport / 100));


            // Sky comment: Choose of the right rating icon
            _SongRatingFalse.Visible = _SongRatingIndex == -1;
            _SongRatingTrue_1.Visible = _SongRatingIndex == 0;
            _SongRatingTrue_2.Visible = _SongRatingIndex == 1;
            _SongRatingTrue_3.Visible = _SongRatingIndex == 2;
            _SongRatingTrue_4.Visible = _SongRatingIndex == 3;
            _SongRatingTrue_5.Visible = _SongRatingIndex == 4;
            _SongRatingTrue_6.Visible = _SongRatingIndex == 5;
            _SongRatingTrue_7.Visible = _SongRatingIndex == 6;
            _SongRatingTrue_8.Visible = _SongRatingIndex == 7;
            _SongRatingTrue_9.Visible = _SongRatingIndex == 8;
            _SongRatingTrue_0.Visible = _SongRatingIndex == 9;
            //SkyLion add end
			

            
            _Artist.Draw();
            _Title.Draw();
            _SongLength.Draw();
            _DuetIcon.Draw();
            _VideoIcon.Draw();
			
			//SkyLion add start:
            _SongRatingFalse.Draw();
            _SongRatingTrue_1.Draw();
            _SongRatingTrue_2.Draw();
            _SongRatingTrue_3.Draw();
            _SongRatingTrue_4.Draw();
            _SongRatingTrue_5.Draw();
            _SongRatingTrue_6.Draw();
            _SongRatingTrue_7.Draw();
            _SongRatingTrue_8.Draw();
            _SongRatingTrue_9.Draw();
            _SongRatingTrue_0.Draw();
            //SkyLion add end
                      
        }

        public override int GetSelectedSong()
        {
            return _Locked;
        }

        public override void SetSelectedSong(int VisibleSongNr)
        {
            base.SetSelectedSong(VisibleSongNr);

            if (VisibleSongNr < 0)
                return;

            if (CSongs.NumVisibleSongs > VisibleSongNr)
            {
                bool sel = false;
                foreach (CStatic tile in _Tiles)
                {
                    if (tile.Selected)
                    {
                        sel = true;
                        break;
                    }
                }

                if (_Locked == -1 || !sel)
                {
                    if (_PreviewSelected > -1)
                    {
                        _Locked = _PreviewSelected;
                    }
                    else
                    {
                        _Locked = 0;
                        _actualSelection = 0;
                        _PreviewSelected = 0;
                        SetSelectedNow();
                        UpdateList(0);
                    }
                }
                
                foreach (CStatic tile in _Tiles)
                {
                    tile.Selected = false;
                }

                _PreviewSelected = VisibleSongNr;
                SetSelectedNow();
                _Locked = VisibleSongNr;

                UpdateList((VisibleSongNr / _NumW) * _NumW - (_NumW * (_NumH - 2)));

                _PreviewSelected = _Locked;
                _actualSelection = _Locked;

                for (int i = 0; i < _Tiles.Count; i++)
                {
                    _Tiles[i].Selected = _Locked == i + _Offset;
                }
            }

        }

        protected override void EnterCategory(int cat)
        {
            base.EnterCategory(cat);

            _PreviewSelected = 0;
            _Locked = -1;
            _actualSelection = 0;
            AfterCategoryChange();
        }

        protected override void ShowCategories()
        {
            base.ShowCategories();

            _PreviewSelected = 0;
            _Locked = -1;
            _actualSelection = 0;
            AfterCategoryChange();
        }

        private void NextCategory()
        {
            if (CSongs.Category > -1)
            {
                Reset();
                CSongs.NextCategory();
                EnterCategory(CSongs.Category);
                _Locked = 0;
            }
        }

        private void PrevCategory()
        {
            if (CSongs.Category > -1)
            {
                Reset();
                CSongs.PrevCategory();
                EnterCategory(CSongs.Category);
                _Locked = 0;
            }
        }

        private void AfterCategoryChange()
        {
            SetSelectedNow();
            SelectSong(_PreviewSelected);

            foreach (CStatic tile in _Tiles)
            {
                tile.Selected = false;
            }

            if (_actualSelection >= 0 && _actualSelection < _Tiles.Count)
                _Tiles[_actualSelection].Selected = true;
           
            if ((_LastKnownNumSongs == CSongs.NumVisibleSongs) && (_LastKnownCategory == CSongs.Category))
                return;

            _LastKnownCategory = CSongs.Category;
            _LastKnownNumSongs = CSongs.NumVisibleSongs;
            UpdateList(0);
            CSongs.UpdateRandomSongList();
        }

        private void UpdateList(int offset)
        {
            if (CSongs.Category > -1)
            {
                if (offset >= (CSongs.NumVisibleSongs / _NumW) * _NumW - (_NumW * (_NumH - 1)))
                    offset = (CSongs.NumVisibleSongs / _NumW) * _NumW - (_NumW * (_NumH - 1));
            }
            else
            {
                if (offset >= (CSongs.NumCategories / _NumW) * _NumW - (_NumW * (_NumH - 1)))
                    offset = (CSongs.NumCategories / _NumW) * _NumW - (_NumW * (_NumH - 1));
            }

            if (offset < 0)
                offset = 0;

            for (int i = 0; i < _Tiles.Count; i++)
            {
                if (CSongs.Category >= 0)
                {
                    if (CSongs.NumVisibleSongs > i + offset)
                    {
                        _Tiles[i].Texture = CSongs.VisibleSongs[i + offset].CoverTextureSmall;
                        _Tiles[i].Color = new SColorF(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        _Tiles[i].Texture = _CoverTexture;
                        _Tiles[i].Color = _Color;
                    }
                }

                if (CSongs.Category == -1)
                {
                    if (CSongs.NumCategories > i + offset)
                    {
                        _Tiles[i].Texture = CSongs.Categories[i + offset].CoverTextureSmall;
                        _Tiles[i].Color = new SColorF(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        _Tiles[i].Texture = _CoverTexture;
                        _Tiles[i].Color = _Color;
                    }
                }
            }
            _Offset = offset;
        }

        public override void LoadTextures()
        {
            base.LoadTextures();

            _Theme.songMenuTileBoard.StaticCoverBig.ReloadTextures();
            _Theme.songMenuTileBoard.StaticTextBG.ReloadTextures();
            _Theme.songMenuTileBoard.StaticDuetIcon.ReloadTextures();
            _Theme.songMenuTileBoard.StaticVideoIcon.ReloadTextures();
			
			//SkyLion add start:
            _Theme.songMenuTileBoard.StaticSongRatingFalse.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_1.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_2.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_3.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_4.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_5.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_6.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_7.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_8.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_9.ReloadTextures();
            _Theme.songMenuTileBoard.StaticSongRatingTrue_0.ReloadTextures();
            //SkyLion add end

            _Theme.songMenuTileBoard.TextArtist.ReloadTextures();
            _Theme.songMenuTileBoard.TextTitle.ReloadTextures();
            _Theme.songMenuTileBoard.TextSongLength.ReloadTextures();
        }
    }
}
