using System;
using System.Text;
using System.IO;
using iText.Kernel.Pdf;

namespace dev5x.PdfTools
{
    public class Encrypt
    {
        public event EventHandler ErrorMessage;

        private void SetErrorMessage(string msg)
        {
            ErrorMessage?.Invoke(msg, EventArgs.Empty);
        }

        #region Public Methods
 
        public bool EncryptFile(string FileName)
        {
            try
            {
                string encryptedFileName = Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName) + "_encrypt.pdf");
                byte[] pwd = Encoding.ASCII.GetBytes("");

                WriterProperties writerProp = new WriterProperties();
                int encryptOptions = EncryptionConstants.ALLOW_PRINTING | EncryptionConstants.ALLOW_SCREENREADERS | EncryptionConstants.ALLOW_FILL_IN;
                writerProp.SetStandardEncryption(pwd, pwd, encryptOptions, EncryptionConstants.STANDARD_ENCRYPTION_128);

                using (PdfWriter encryptedWriter = new PdfWriter(encryptedFileName, writerProp))
                using (PdfDocument encryptedDocument = new PdfDocument(encryptedWriter))
                using (PdfDocument pdfDocOriginal = new PdfDocument(new PdfReader(FileName)))
                {
                    pdfDocOriginal.CopyPagesTo(1, pdfDocOriginal.GetNumberOfPages(), encryptedDocument);
                }

                // Delete original file
                File.Delete(FileName);
                // Rename merged file to the original file
                File.Move(encryptedFileName, FileName);

                return true;
            }
            catch (Exception ex)
            {
                SetErrorMessage("EncryptFile - " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}