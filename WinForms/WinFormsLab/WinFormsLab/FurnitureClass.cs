using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Imaging;

namespace WinFormsLab
{
    [Serializable]
    class FurnitureClass
    {
        public Image furniture { get; set; }
        public Point centerLocation { get; set; }
        public string myText { get; set; }
        public float Transparency { get; set; }
        public float Rotation { get; set; }

        public FurnitureClass(Image p, Point center, string text)
        {
            furniture = p;
            centerLocation = center;
            myText = text;
            Transparency = 1;
            Rotation = 0;
        }

        public Image DrawMyImage(Image pboxImage)
        {
            Bitmap canvas = new Bitmap(pboxImage);
            Bitmap bmp = new Bitmap(furniture);
            Graphics g = Graphics.FromImage(canvas);
            g.DrawImage(bmp, centerLocation.X - (bmp.Width / 2), centerLocation.Y - (bmp.Height / 2));
            //pictureBox1.Refresh();
            g.Dispose();
            return canvas;
        }

        public virtual Image ChangeImageColor(Image pboxImage)
        {
            Bitmap canvas = new Bitmap(pboxImage);
            Graphics g = Graphics.FromImage(canvas);
            ImageAttributes imageAttributes = new ImageAttributes();
            //int width = image.Width;
            //int height = image.Height;

            float[][] colorMatrixElements = {
                new float[] {1,  0,  0,  0, 0},        // red scaling factor of 1
                new float[] {0,  1,  0,  0, 0},        // green scaling factor of 1
                new float[] {0,  0,  1,  0, 0},        // blue scaling factor of 1
                new float[] {0,  0,  0,  Transparency, 0},        // alpha scaling factor of 1
                new float[] {0, 0, 0, 0, 1}};   

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            //set the color matrix.
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            g.TranslateTransform(centerLocation.X, centerLocation.Y);
            g.RotateTransform(Rotation);
            g.TranslateTransform(-centerLocation.X, -centerLocation.Y);

            Point start = new Point(centerLocation.X - (furniture.Width/2), centerLocation.Y - (furniture.Height/2));
            Rectangle rect = new Rectangle(start, furniture.Size);
            g.DrawImage(furniture, rect, 0, 0, furniture.Width, furniture.Height,
                GraphicsUnit.Pixel, imageAttributes);
            
            g.Dispose();
            return canvas;
        }

    }

    [Serializable]
    class WallClass : FurnitureClass
    {
        public List<PointF> clickPoints = new List<PointF>();
        public Point? MousePosition { get; set; }

        public WallClass(Image img, Point center, string text) : base(img, center, text)
        {
            clickPoints.Add(center);
            MousePosition = null;
        }

        public override Image ChangeImageColor(Image pboxImage)
        {
            Bitmap canvas = new Bitmap(pboxImage);
            Graphics g = Graphics.FromImage(canvas);
            GraphicsPath path = new GraphicsPath();
            Pen blackPen = new Pen(Color.Black);

            if (Transparency == 0.50f)
                blackPen = new Pen(Color.FromArgb(128, 0, 0, 0), 10);
            else blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 10);
            PointF last_point = new Point();
            int it = 0;

            path.StartFigure();
            foreach (PointF p in clickPoints)
            {
                blackPen.LineJoin = LineJoin.Bevel;
                if (it != 0)//g.DrawLine(Pen, p_last, p);
                    path.AddLine(last_point, p);
                it++;
                last_point = p;
            }
            if(MousePosition != null)
                path.AddLine(last_point, MousePosition.Value);
            blackPen.LineJoin = LineJoin.Bevel;

            Matrix rotateMatrix = new Matrix();
            // Set the rotation angle and starting point for the text.
            rotateMatrix.RotateAt(Rotation, centerLocation);
            // Transform the text with the matrix.
            path.Transform(rotateMatrix);
            g.DrawPath(blackPen, path);

            g.Dispose();
            blackPen.Dispose();
            return canvas;
        }

        public bool WallClicked(Point mousePos)
        {
            GraphicsPath path = new GraphicsPath();
            PointF last_point = new Point();
            Pen blackPen = new Pen(Color.Black, 10);
            int it = 0;

            path.StartFigure();
            foreach (PointF p in clickPoints)
            {
                blackPen.LineJoin = LineJoin.Bevel;
                if (it != 0)//g.DrawLine(Pen, p_last, p);
                    path.AddLine(last_point, p);
                it++;
                last_point = p;
            }
            return path.IsOutlineVisible(mousePos, blackPen);
        }

        public PointF RotatePoint(PointF pointToRotate, double angle)
        {
            double angleInRadians = angle * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new PointF
            {
                X = (float)Math.Round(cosTheta * (pointToRotate.X - centerLocation.X) - sinTheta * (pointToRotate.Y - centerLocation.Y) + centerLocation.X),
                Y = (float)Math.Round(sinTheta * (pointToRotate.X - centerLocation.X) + cosTheta * (pointToRotate.Y - centerLocation.Y) + centerLocation.Y)
            };
        }

    }

}
