using System;
using System.IO;
using iText.Kernel.Pdf;

namespace dev5x.PdfTools
{
    public class Compress
    {
        public event EventHandler ErrorMessage;

        private void SetErrorMessage(string msg)
        {
            ErrorMessage?.Invoke(msg, EventArgs.Empty);
        }

        public bool CompressFile(string FileName)
        {
            // Compress file into new temp file, then rename the compressed file to the original name
            try
            {
                string fileCompressed = Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName) + "_compress.pdf");

                using (PdfReader pdfReader = new PdfReader(FileName))
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                using (PdfWriter pdfWriter = new PdfWriter(fileCompressed, new WriterProperties().SetFullCompressionMode(true)))
                {
                    pdfWriter.SetCompressionLevel(CompressionConstants.BEST_COMPRESSION);

                    using (PdfDocument newDocument = new PdfDocument(pdfWriter))
                    {
                        pdfDocument.CopyPagesTo(1, pdfDocument.GetNumberOfPages(), newDocument);
                    }
                }
                
                // Delete original file
                File.Delete(FileName);
                // Rename compressed file to original name
                File.Move(fileCompressed, FileName);

                return true;
            }
            catch (Exception ex)
            {
                SetErrorMessage("CompressFile - " + ex.Message);
                return false;
            }
        }
    }
}