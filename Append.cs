using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace dev5x.PdfTools
{
    public class Append
    {
        public event EventHandler ErrorMessage;

        private void SetErrorMessage(string msg)
        {
            ErrorMessage?.Invoke(msg, EventArgs.Empty);
        }

        #region Public Methods

        public bool AppendFile(string ToFileName, string FromFileName)
        {
            // Append FromFileName to the ToFileName
            // The two files are merged into a new file, then the new file is renamed to the ToFileName
            try
            {
                string mergedFileName = Path.Combine(Path.GetDirectoryName(ToFileName), Path.GetFileNameWithoutExtension(ToFileName) + "_merge.pdf");

                using (PdfDocument mergeDocument = new PdfDocument(new PdfWriter(mergedFileName)))
                {
                    PdfMerger pdfMerge = new PdfMerger(mergeDocument);

                    using (PdfDocument pdfDoc1 = new PdfDocument(new PdfReader(ToFileName)))
                    {
                        pdfMerge.Merge(pdfDoc1, 1, pdfDoc1.GetNumberOfPages());
                    }

                    using (PdfDocument pdfDoc2 = new PdfDocument(new PdfReader(FromFileName)))
                    {
                        pdfMerge.Merge(pdfDoc2, 1, pdfDoc2.GetNumberOfPages());
                    }

                    pdfMerge.Close();
                }

                // Delete original file
                File.Delete(ToFileName);
                // Rename merged file to the original file
                File.Move(mergedFileName, ToFileName);

                return true;
            }
            catch (Exception ex)
            {
                SetErrorMessage("AppendFile - " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}