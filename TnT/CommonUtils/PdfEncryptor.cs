using System;
using System.IO;
using System.Collections;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Security;
using PdfSharp.Pdf.IO;

namespace TnT
{

    public class PDFEncryptor
    {
        public byte[] encrypt(byte[] pdfFile, string userPass, string ownerPass)
        {
            byte[] encfileContents = null;

            Stream stream = new MemoryStream(pdfFile);
            // Open an existing document. Providing an unrequired password is ignored.
            PdfDocument document = PdfReader.Open(stream); // new PdfDocument (stream);   // PdfReader.Open(filename, "some text");

            PdfSecuritySettings securitySettings = document.SecuritySettings;

            // Setting one of the passwords automatically sets the security level to 
            // PdfDocumentSecurityLevel.Encrypted128Bit.
            securitySettings.UserPassword = userPass;
            securitySettings.OwnerPassword = ownerPass;

            // Don't use 40 bit encryption unless needed for compatibility reasons
            //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            // Restrict some rights.
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                document.Close();
                encfileContents = ms.ToArray();
                return encfileContents;
            }
            // document.Save(resultStream);
            // document.Close();
            //return resultStream.ToArray();

            // Save the document...


            //using (MemoryStream ms = new MemoryStream())
            //{
            //    document.Save (ms, true);
            //    fileContents =ms.ToArray();
            //}

            //  document.Save(res,true);
            //byte[] buffer = new byte[res.Length];
            //res.Seek(0, SeekOrigin.Begin);
            //res.Flush();
            //res.Read(buffer, 0, (int)res.Length);

            //return fileContents;


        }
    }



}