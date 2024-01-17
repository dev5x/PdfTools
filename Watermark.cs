using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace dev5x.PdfTools
{
    class Watermark
    {
        public event EventHandler ErrorMessage;

        private void SetErrorMessage(string msg)
        {
            ErrorMessage?.Invoke(msg, EventArgs.Empty);
        }

        #region Public Methods

        public void AddWatermark(string FileName, string WatermarkText)
        {
            // Place a watermark in lower left corner, rotated 90 degrees up
            try
            {
                FileInfo fi = new FileInfo(Path.Combine(Path.GetTempPath(), Path.GetFileName(FileName)));

                //Initialize PDF document
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(FileName), new PdfWriter(fi.FullName));
                Document document = new Document(pdfDoc);

                //Draw watermark
                Paragraph p = new Paragraph(WatermarkText);
                p.SetFontSize(10);
                p.SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN));
                p.SetFontColor(ColorConstants.BLUE);

                document.ShowTextAligned(p, 30f, 40f, 1, TextAlignment.LEFT, VerticalAlignment.MIDDLE, 1.570796f);

                document.Close();
                pdfDoc.Close();

                // Replace orig file with watermarked file
                fi.CopyTo(FileName, true);
                fi.Delete();
            }
            catch (Exception ex)
            {
                SetErrorMessage("AddWatermark - " + ex.Message);
            }
        }
        #endregion
    }
}