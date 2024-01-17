using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace dev5x.PdfTools
{
    public class TextExtractor : IDisposable
    {
        public event EventHandler ErrorMessage;

        private readonly PdfReader _pdfReader = null;
        private readonly PdfDocument _pdfDocument = null;

        public TextExtractor(string FileName)
        {
            // This class creates a reader and document object for the file name parm
            _pdfReader = new PdfReader(FileName);
            _pdfDocument = new PdfDocument(_pdfReader);
        }

        private void SetErrorMessage(string msg)
        {
            ErrorMessage?.Invoke(msg, EventArgs.Empty);
        }

        #region Public Methods

        public int NumberOfPages
        {
            get { return _pdfDocument.GetNumberOfPages(); }
        }

        public string GetPageText(int PageNumber)
        {
            // Get text for the page number
            try
            {
                return  PdfTextExtractor.GetTextFromPage(_pdfDocument.GetPage(PageNumber));
            }
            catch(Exception ex)
            {
                SetErrorMessage("GetPageText - " + ex.Message);
                return "--ERROR--";
            }
        }

        public string[] GetPageTextArray(int PageNumber)
        {
            // Get page text array for the page number
            try
            {
                return PdfTextExtractor.GetTextFromPage(_pdfDocument.GetPage(PageNumber)).Split('\n');
            }
            catch (Exception ex)
            {
                SetErrorMessage("GetPageTextArray - " + ex.Message);
                return new string[1] { "--ERROR--" };
            }
        }

        public bool WriteFile(List<int> ListPages, string NewFileName)
        {
            // Write all page numbers in List<> from current document to new file
            try
            {
                using (PdfDocument newDocument = new PdfDocument(new PdfWriter(NewFileName)))
                {
                    _pdfDocument.CopyPagesTo(ListPages, newDocument);
                }

                return true;
            }
            catch (Exception ex)
            {
                SetErrorMessage("WriteFile - " + ex.Message);
                return false;
            }
        }

        // Dispose implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed objects
                if (_pdfDocument != null)
                {
                    _pdfDocument.Close();
                }

                if (_pdfReader != null)
                {
                    _pdfReader.Close();
                }
            }
        }
        #endregion
    }
}