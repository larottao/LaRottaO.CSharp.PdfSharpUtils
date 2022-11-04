using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LaRottaO.CSharp.PdfSharpUtils
{
    public class PdfSharpUtilities
    {
        private double topMargin = 0;
        private double leftMargin = 0;
        private double rightMargin = 0;
        private double bottomMargin = 0;
        private double cm;

        private PdfDocument document;
        private PdfPage page;
        private XGraphics gfx;
        private XFont font;
        private XPen pen;
        private String outputPath;

        public PdfSharpUtilities(String argOutputpath, Boolean argAddMarginGuides = false)
        {
            this.outputPath = argOutputpath;

            //You’ll need a PDF document:
            this.document = new PdfDocument();

            //And you need a page:
            this.page = document.AddPage();
            this.page.Size = PageSize.Letter;

            //Define how much a cm is
            this.cm = new Interpolation().linearInterpolation(0, 0, 27.9, page.Height, 1);
            Console.WriteLine("1 cm:" + cm);

            topMargin = 2.5 * cm;
            leftMargin = 3 * cm;
            rightMargin = page.Width - (3 * cm);
            bottomMargin = page.Height - (2.5 * cm);

            Console.WriteLine("Page Width cm:" + page.Width * cm);
            Console.WriteLine("Page Height cm:" + page.Height * cm);

            Console.WriteLine("Top Margin cm:" + topMargin);
            Console.WriteLine("Left Margin cm:" + leftMargin);
            Console.WriteLine("Right Margin cm:" + rightMargin);
            Console.WriteLine("Bottom Margin cm:" + bottomMargin);

            //Drawing is done with an XGraphics object:

            this.gfx = XGraphics.FromPdfPage(page);

            this.font = new XFont("Arial", 12, XFontStyle.Bold);
            this.pen = new XPen(XColors.Black, 0.5);

            if (argAddMarginGuides)
            {
                gfx.DrawString("+", font, XBrushes.Black, rightMargin, topMargin);
                gfx.DrawString("+", font, XBrushes.Black, leftMargin, topMargin);
                gfx.DrawString("+", font, XBrushes.Black, rightMargin, bottomMargin);
                gfx.DrawString("+", font, XBrushes.Black, leftMargin, bottomMargin);
            }
        }

        public void addText(String text, DPoint xyStartingPosition, int size = 12)
        {
            this.gfx.DrawString(text, this.font, XBrushes.Black, leftMargin + (xyStartingPosition.x * cm), topMargin + (xyStartingPosition.y * cm));
        }

        public void drawSquare(DPoint xyStartingPosition, double width, double height, XBrush xbrush)
        {
            this.gfx.DrawRectangle(xbrush, new XRect(leftMargin + (xyStartingPosition.x * cm), topMargin + (xyStartingPosition.y * cm), (width * cm), (height * cm)));
        }

        public void drawLine(DPoint fromXyPosition, DPoint toXyPosition)
        {
            Console.WriteLine("Drawing line from (" + fromXyPosition.x + "," + fromXyPosition.y + "),(" + toXyPosition.x + "," + toXyPosition.y + ")");

            this.gfx.DrawLine(this.pen, leftMargin + (fromXyPosition.x * cm), topMargin + (fromXyPosition.y * cm), leftMargin + (toXyPosition.x * cm), topMargin + (toXyPosition.y * cm));
        }

        public void drawTable(double initialPosX, double initialPosY, double width, double height, int rows, int columns, XBrush xbrush)
        {
            drawSquare(new DPoint(initialPosX, initialPosY), width, height, xbrush);

            double distanceBetweenRows = height / rows;
            double distanceBetweenColumns = width / columns;

            DPoint pointA = new DPoint(initialPosX, initialPosY);
            DPoint pointB = new DPoint(initialPosX + width, initialPosY);

            for (int i = 0; i <= rows; i++)
            {
                drawLine(pointA, pointB);

                pointA.y = pointA.y + distanceBetweenRows;
                pointB.y = pointB.y + distanceBetweenRows;
            }

            pointA = new DPoint(initialPosX, initialPosY);
            pointB = new DPoint(initialPosX, initialPosY + height);

            for (int i = 0; i <= columns; i++)
            {
                drawLine(pointA, pointB);

                pointA.x = pointA.x + distanceBetweenColumns;
                pointB.x = pointB.x + distanceBetweenColumns;
            }
        }

        public void saveAndShow(Boolean argShowAfterSaving = true)
        {
            document.Save(this.outputPath);

            if (argShowAfterSaving)
            {
                Process.Start(this.outputPath);
            }
        }
    }
}