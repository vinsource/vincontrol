using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace VINCONTROL.Image
{
    public class WalkaroundImage
    {
        private Font _textFont;
        private Color _textColor;
        private Color _backgroundColor;
        private string _sourceFile;
        //private string _resultFile;
        private List<WalkaroundPoint> _list;

        public WalkaroundImage(string sourceFile, List<WalkaroundPoint> list)
        {
            _textFont = new Font("Times New Roman", 13, FontStyle.Bold);
            _textColor = Color.White;
            _backgroundColor = Color.Red;
            _sourceFile = sourceFile;
            _list = list;
        }

        public byte[] CreateImage()
        {
            using (Bitmap bitmap = new Bitmap(_sourceFile))
            {
                using (Bitmap tempBitmap = new Bitmap(bitmap))
                {
                    foreach (var item in _list)
                    {
                        DrawCircleWithNumber(item.Value, tempBitmap, item.RecTangle);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        tempBitmap.Save(memoryStream, ImageFormat.Bmp);
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public Bitmap CreateBitmapImage()
        {
            using (Bitmap bitmap = new Bitmap(_sourceFile))
            {
                Bitmap tempBitmap = new Bitmap(bitmap);

                foreach (var item in _list)
                {
                    DrawCircleWithNumber(item.Value, tempBitmap, item.RecTangle);
                }

                return tempBitmap;

            }
        }

        void DrawCircleWithNumber(int number, Bitmap parentBitmap, RectangleF rec)
        {
            Graphics graphic = Graphics.FromImage(parentBitmap);

            //draw circle       
            graphic.FillEllipse(new SolidBrush(_backgroundColor), rec);
            graphic.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            //draw content
            PointF oPoint = GetTextPosition(rec, number);
            graphic.DrawString(number.ToString(), _textFont, new SolidBrush(_textColor), oPoint);
        }

        PointF GetTextPosition(RectangleF rec, int number)
        {
            float fontSize = _textFont.Size;
            if (number <= 9 && number >= 0)
            {
                return new PointF(rec.X + rec.Width / 2 - fontSize / 2, rec.Y + rec.Height / 2 - fontSize * 3 / 4);
            }
            else
            {
                return new PointF(rec.X + rec.Width / 2 - fontSize * 9 / 10, rec.Y + rec.Height / 2 - fontSize * 3 / 4);
            }
        }
    }

    public class WalkaroundPoint
    {
        public RectangleF RecTangle { get; set; }
        public int Value { get; set; }
    }
}
