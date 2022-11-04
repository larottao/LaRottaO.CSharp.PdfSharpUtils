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
    /*******************************************************************/

    //By Luis Felipe La Rotta

    // Usage example
    /*

    private void buttonRunExample_Click(object sender, EventArgs e)
    {
        PdfSharpUtilities pdf = new PdfSharpUtilities("test.pdf", true);

        pdf.drawSquare(new DPoint(0, 0), 3, 2, XBrushes.Purple);

        pdf.addText("Username", new DPoint(0, 4.5), 16);

        pdf.addText("Invoice", new DPoint(12.15, 1.5), 14);

        pdf.addText("Account: 6666969696", new DPoint(0, 6));

        pdf.addText("Period: 2022-11", new DPoint(0, 7));

        pdf.addText("E-mail: mail@gmail.com", new DPoint(0, 8));

        pdf.addText("Inventory:", new DPoint(0, 10));

        //Example table: to fill with example data leave contents = null
        pdf.drawTable(0, 11, 15.7, 3, XBrushes.LightGray, null);

        pdf.saveAndShow();
    }

    */
    /******************************************************************/

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

            //Define how much a cm is in document's units
            this.cm = new Interpolation().linearInterpolation(0, 0, 27.9, page.Height, 1);
            Console.WriteLine("1 cm:" + cm);

            //Drawing is done with an XGraphics object:

            this.gfx = XGraphics.FromPdfPage(page);

            this.font = new XFont("Arial", 12, XFontStyle.Bold);
            this.pen = new XPen(XColors.Black, 0.5);

            //Sugested margins

            topMargin = 2.5 * cm;
            leftMargin = 3 * cm;
            rightMargin = page.Width - (3 * cm);
            bottomMargin = page.Height - (2.5 * cm);

            if (argAddMarginGuides)
            {
                gfx.DrawString("+", font, XBrushes.Black, rightMargin, topMargin);
                gfx.DrawString("+", font, XBrushes.Black, leftMargin, topMargin);
                gfx.DrawString("+", font, XBrushes.Black, rightMargin, bottomMargin);
                gfx.DrawString("+", font, XBrushes.Black, leftMargin, bottomMargin);
            }

            Console.WriteLine("Page Width in cm:" + page.Width * cm);
            Console.WriteLine("Page Height in cm:" + page.Height * cm);

            Console.WriteLine("Top Margin in cm:" + topMargin);
            Console.WriteLine("Left Margin in cm:" + leftMargin);
            Console.WriteLine("Right Margin in cm:" + rightMargin);
            Console.WriteLine("Bottom Margin in cm:" + bottomMargin);
        }

        public void drawTable(double initialPosX, double initialPosY, double width, double height, XBrush xbrush, List<String[]> contents = null)
        {
            drawSquare(new DPoint(initialPosX, initialPosY), width, height, xbrush);

            if (contents == null)
            {
                contents = new List<String[]>();

                contents.Add(new string[] { "Type", "Size", "Weight", "Stock", "Tax", "Price" });
                contents.Add(new string[] { "Obo", "1", "45", "56", "16.00", "6.50" });
                contents.Add(new string[] { "Crotolamo", "2", "72", "63", "16.00", "19.00" });
            }

            int columns = contents[0].Length;
            int rows = contents.Count;

            double distanceBetweenRows = height / rows;
            double distanceBetweenColumns = width / columns;

            /*******************************************************************/
            // Draw the row lines
            /*******************************************************************/

            DPoint pointA = new DPoint(initialPosX, initialPosY);
            DPoint pointB = new DPoint(initialPosX + width, initialPosY);

            for (int i = 0; i <= rows; i++)
            {
                drawLine(pointA, pointB);

                pointA.y = pointA.y + distanceBetweenRows;
                pointB.y = pointB.y + distanceBetweenRows;
            }

            /*******************************************************************/
            // Draw the column lines
            /*******************************************************************/

            pointA = new DPoint(initialPosX, initialPosY);
            pointB = new DPoint(initialPosX, initialPosY + height);

            for (int i = 0; i <= columns; i++)
            {
                drawLine(pointA, pointB);

                pointA.x = pointA.x + distanceBetweenColumns;
                pointB.x = pointB.x + distanceBetweenColumns;
            }

            /*******************************************************************/
            // Insert text corresponding to each cell
            /*******************************************************************/

            pointA = new DPoint(initialPosX, initialPosY);

            foreach (String[] rowDataArray in contents)
            {
                foreach (String cellText in rowDataArray)
                {
                    //The text starting point offset can be 0 for longest words and half distance for 1 character

                    this.gfx.DrawString(cellText, this.font, XBrushes.Black, new XRect(leftMargin + (pointA.x * cm), topMargin + (pointA.y * cm), distanceBetweenColumns * cm, distanceBetweenRows * cm), XStringFormats.Center);

                    pointA.x = pointA.x + distanceBetweenColumns;
                }

                pointA.x = initialPosX;
                pointA.y = pointA.y + distanceBetweenRows;
            }
        }

        public void addText(String text, DPoint xyStartingPosition, int size = 12)
        {
            this.gfx.DrawString(text, this.font, XBrushes.Black, leftMargin + (xyStartingPosition.x * cm), topMargin + (xyStartingPosition.y * cm));
        }

        public void drawSquare(DPoint xyStartingPosition, double width, double height, XBrush xbrush)
        {
            Console.WriteLine("Drawing square starting at: " + xyStartingPosition.x + "," + xyStartingPosition.y + " width: " + width + " height: " + height);
            this.gfx.DrawRectangle(xbrush, new XRect(leftMargin + (xyStartingPosition.x * cm), topMargin + (xyStartingPosition.y * cm), (width * cm), (height * cm)));
        }

        public void drawLine(DPoint fromXyPosition, DPoint toXyPosition)
        {
            this.gfx.DrawLine(this.pen, leftMargin + (fromXyPosition.x * cm), topMargin + (fromXyPosition.y * cm), leftMargin + (toXyPosition.x * cm), topMargin + (toXyPosition.y * cm));
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