using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace WpfApp1
{
    /// <summary>
    /// Visualization for the WPF
    /// </summary>
    public static class VisualizationWpf
    {
        #region graphics
        private const string _startGridHorizontal = ".\\Images\\_startGridHorizontal.png";
        private const string _leftCornerHorizontal = ".\\Images\\_leftCornerHorizontal.png";
        private const string _leftCornerVertical = ".\\Images\\_leftCornerVertical.png";
        private const string _rightCornerHorizontal = ".\\Images\\_rightCornerHorizontal.png";
        private const string _rightCornerVertical = ".\\Images\\_rightCornerVertical.png";
        private const string _straightHorizontal = ".\\Images\\_straightHorizontal.png";
        private const string _straightVertical = ".\\Images\\_straightVertical.png";
        private const string _finishHorizontal = ".\\Images\\_finishHorizontal.png";
        private const string _finishVertical = ".\\Images\\_finishVertical.png";
        private const string car_blue = ".\\Images\\car_blue.png";
        private const string car_gray = ".\\Images\\car_gray.png";
        private const string car_green = ".\\Images\\car_green.png";
        private const string car_red = ".\\Images\\car_red.png";
        private const string car_yellow = ".\\Images\\car_yellow.png";
        private const string car_broken = ".\\Images\\car_broken.png";
        #endregion

        private static int _cursPosX;
        private static int _cursPosY;
        private static int _calcX;
        private static int _calcY;
        private static int _direction = 1;

        private static int _widthSectionType = 500;
        
        private static int _width;
        private static int _height;

        /// <summary>
        /// Draws the track
        /// </summary>
        /// <param name="track"></param>
        /// <param name="differenceX"></param>
        /// <param name="differenceY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>The track bitmapsource</returns>
        public static BitmapSource DrawTrack(Track track, int differenceX, int differenceY, int width, int height)
        {
            _width = width;
            _height = height;

            var bitmap = new Bitmap(ImageController.GetImageCache("empty", _width, _height));
            var bitmapGraphics = Graphics.FromImage(bitmap);

            Bitmap sectionBm = null;

            _cursPosX = differenceX;
            _cursPosY = differenceY;

            foreach (var section in track.Sections)
            {
                AddSectionToGraphics(section, sectionBm, bitmapGraphics);
                AddParticipantToGraphics(section, bitmapGraphics);
            }

            return ImageController.CreateBitmapSourceFromGdiBitmap(bitmap);
        }

        /// <summary>
        /// Adds participant to section, if its on that section
        /// </summary>
        /// <param name="section"></param>
        /// <param name="bitmapGraphics"></param>
        private static void AddParticipantToGraphics(Section section, Graphics bitmapGraphics)
        {
            var participantLeft = Data.CurrentRace.GetSectionData(section).Left;
            var participantRight = Data.CurrentRace.GetSectionData(section).Right;

            var drawX = _cursPosX;
            var drawY = _cursPosY;

            if (participantLeft != null)
            {
                var participantBm = new Bitmap(ImageController.GetImageCache(GetCarColor(participantLeft), _width, _height));

                switch (_direction)
                {
                    case 1:
                        drawX = _cursPosX + (participantBm.Width / 2) + (participantBm.Width / 4);
                        drawY = _cursPosY + (participantBm.Width / 4);
                        break;
                    case 2:
                        drawX = _cursPosX + (participantBm.Width / 4);
                        drawY = _cursPosY + participantBm.Width;
                        participantBm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 3:
                        drawY = _cursPosY + (participantBm.Width / 4);
                        participantBm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 0:
                        drawX = _cursPosX + (participantBm.Width / 4);
                        participantBm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }

                bitmapGraphics.DrawImage(participantBm, drawX, drawY);

                drawY = _cursPosY;
                drawX = _cursPosX;
            }

            if (participantRight != null)
            {
                var participantBm = new Bitmap(ImageController.GetImageCache(GetCarColor(participantRight), _width, _height));

                switch (_direction)
                {
                    case 1:
                        drawX = _cursPosX + (participantBm.Width / 2) + (participantBm.Width / 4);
                        drawY = _cursPosY + participantBm.Width - (participantBm.Width / 4);
                        break;
                    case 2:
                        drawX = _cursPosX + participantBm.Width - (participantBm.Width / 4);
                        drawY = _cursPosY + participantBm.Width;
                        participantBm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 3:
                        drawY = _cursPosY + participantBm.Width - (participantBm.Width / 4);
                        participantBm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 0:
                        drawX = _cursPosX + participantBm.Width - (participantBm.Width / 4);
                        participantBm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }

                bitmapGraphics.DrawImage(participantBm, drawX, drawY);

                drawY = _cursPosY;
                drawX = _cursPosX;
            }
        }

        /// <summary>
        /// Participants color to draw on the bitmap
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static string GetCarColor(IParticipant participant)
        {
            if (participant.Equipment.IsBroken) return car_broken;

            return participant.TeamColor switch
            {
                IParticipant.TeamColors.Yellow => car_yellow,
                IParticipant.TeamColors.Green => car_green,
                IParticipant.TeamColors.Grey => car_gray,
                IParticipant.TeamColors.Blue => car_blue,
                _ => car_red
            };
        }

        /// <summary>
        /// Adds section to graphics
        /// </summary>
        /// <param name="section"></param>
        /// <param name="sectionBm"></param>
        /// <param name="bitmapGraphics"></param>
        private static void AddSectionToGraphics(Section section, Bitmap sectionBm, Graphics bitmapGraphics)
        {
            sectionBm = section.SectionType switch
            {
                SectionTypes.StartGrid => DrawStraightSection(section.SectionType),
                SectionTypes.RightCorner => DrawRightCornerSection(),
                SectionTypes.Straight => DrawStraightSection(section.SectionType),
                SectionTypes.LeftCorner => DrawLeftCornerSection(),
                SectionTypes.Finish => DrawStraightSection(section.SectionType),
                _ => sectionBm
            };

            bitmapGraphics.DrawImage(sectionBm, _cursPosX, _cursPosY);
        }
        
        /// <summary>
        /// Draws a straight section on the bitmap
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns></returns>
        public static Bitmap DrawStraightSection(SectionTypes sectionType)
        {
            Bitmap new_bitmap;
            if (sectionType == SectionTypes.Straight)
            {
                new_bitmap = new Bitmap(ImageController.GetImageCache(_straightHorizontal, _width, _height));
            }
            else if (sectionType == SectionTypes.Finish)
            {
                new_bitmap = new Bitmap(ImageController.GetImageCache(_finishHorizontal, _width, _height));
            }
            else
            {
                new_bitmap = new Bitmap(ImageController.GetImageCache(_startGridHorizontal, _width, _height));
            }

            if (_direction == 1)
            {
                _cursPosX += new_bitmap.Width;
            }
            else if (_direction == 2)
            {
                _cursPosY += new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (_direction == 3)
            {
                _cursPosX -= new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if (_direction == 0)
            {
                _cursPosY -= new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }

            return new_bitmap;
        }

        /// <summary>
        /// Draws a right corner section on the bitmap
        /// </summary>
        /// <returns></returns>
        public static Bitmap DrawRightCornerSection()
        {
            var new_bitmap = new Bitmap(ImageController.GetImageCache(_rightCornerHorizontal, _width, _height));

            if (_direction == 1)
            {
                _cursPosX += new_bitmap.Width;
                _direction++;
            }
            else if (_direction == 2)
            {
                _cursPosY += new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                _direction++;
            }
            else if (_direction == 3)
            {
                _cursPosX -= new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                _direction = 0;
            }
            else if (_direction == 0)
            {
                _cursPosY -= new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                _direction++;
            }

            return new_bitmap;
        }

        /// <summary>
        /// Draws a left corner section on the bitmap
        /// </summary>
        /// <returns></returns>
        public static Bitmap DrawLeftCornerSection()
        {
            var new_bitmap = new Bitmap(ImageController.GetImageCache(_leftCornerHorizontal, _width, _height));
            
            if (_direction == 1)
            {
                _cursPosX += new_bitmap.Width;
                _direction--;
            }
            else if (_direction == 2)
            {
                _cursPosY += new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                _direction--;
            }
            else if (_direction == 3)
            {
                _cursPosX -= new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                _direction--;
            }
            else if (_direction == 0)
            {
                _cursPosY -= new_bitmap.Width;
                new_bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                _direction = 3;
            }

            return new_bitmap;
        }

        /// <summary>
        /// Calculate the size of the map 
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static Tuple<List<int>, List<int>> CalculateSizeOfMap(Track track)
        {
            _calcX = 0;
            _calcY = 0;
            var _xList = new List<int>();
            var _yList = new List<int>();

            foreach (var section in track.Sections)
            {
                if (section.SectionType == SectionTypes.Straight || section.SectionType == SectionTypes.Finish || section.SectionType == SectionTypes.StartGrid)
                    CalculateStraightSection();
                else if (section.SectionType == SectionTypes.RightCorner)
                    CalculateRightCornerSection();
                else if (section.SectionType == SectionTypes.LeftCorner)
                    CalculateLeftCornerSection();

                _xList.Add(_calcX);
                _yList.Add(_calcY);
            }



            return Tuple.Create(_xList, _yList);
        }

        /// <summary>
        /// Calculates position for straight sections depending on the direction
        /// </summary>
        public static void CalculateStraightSection()
        {
            if (_direction == 1)
            {
                _calcX += _widthSectionType;
            }
            else if (_direction == 2)
            {
                _calcY += _widthSectionType;
            }
            else if (_direction == 3)
            {
                _calcX -= _widthSectionType;
            }
            else if (_direction == 0)
            {
                _calcY -= _widthSectionType;
            }
        }

        /// <summary>
        /// Calculates the position for right corner sections depending on the direction
        /// </summary>
        public static void CalculateRightCornerSection()
        {
            if (_direction == 1)
            {
                _calcX += _widthSectionType;
                _direction++;
            }
            else if (_direction == 2)
            {
                _calcY += _widthSectionType;
                _direction++;
            }
            else if (_direction == 3)
            {
                _calcX -= _widthSectionType;
                _direction = 0;
            }
            else if (_direction == 0)
            {
                _calcY -= _widthSectionType;
                _direction++;
            }
        }

        /// <summary>
        /// Calculates the position for the left corner sections depending on the direction
        /// </summary>
        public static void CalculateLeftCornerSection()
        {
            if (_direction == 1)
            {
                _cursPosX += _widthSectionType;
                _direction--;
            }
            else if (_direction == 2)
            {
                _cursPosY += _widthSectionType;
                _direction--;
            }
            else if (_direction == 3)
            {
                _cursPosX -= _widthSectionType;
                _direction--;
            }
            else if (_direction == 0)
            {
                _cursPosY -= _widthSectionType;
                _direction = 3;
            }
        }
    }
}
