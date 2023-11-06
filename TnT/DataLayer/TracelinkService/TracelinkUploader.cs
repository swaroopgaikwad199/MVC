using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.TracelinkService
{
    public enum TLFileType
    {
        SOMFile,
        Disposition,
        DispositionUpdate,
        Production,
        None

    }
    public enum TLUploadingErrors
    {
       NoPermmision,
       ConnectionProblem,
       SSHException,
       Other

    }

    public class TracelinkUploader
    {
        string host;
        string username;
        string password;
        string destinationpath;
        int port;


        TLUploadingErrors TLError;

        public TLUploadingErrors getError()
        {
            return TLError;
        }

        public TracelinkUploader()
        {
            host = Utilities.getAppSettings("TraceLinkFileSysHost");
            username = Utilities.getAppSettings("TraceLinkFileSysUser");
            password = Utilities.getAppSettings("TraceLinkFileSysPass");           
            port = Convert.ToInt32(Utilities.getAppSettings("TraceLinkFileSysPort"));
        }

        public bool UploadSFTPFile(string sourcefile, TLFileType ftype)
        {
            try
            {
                if (ftype != TLFileType.None)
                {
                    switch (ftype)
                    {
                        case TLFileType.SOMFile:
                            destinationpath = Utilities.getAppSettings("TraceLinkFileSysSOMDestination");
                            break;
                        case TLFileType.Disposition:
                            destinationpath = Utilities.getAppSettings("TraceLinkFileSysDispositionDestination");
                            break;
                        case TLFileType.DispositionUpdate:
                            destinationpath = Utilities.getAppSettings("TraceLinkFileSysDispositionUpdtDestination");
                            break;
                    }
                    using (SftpClient client = new SftpClient(host, port, username, password))
                    {
                        client.Connect();
                        client.ChangeDirectory(destinationpath);
                        using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                        {
                            client.BufferSize = 4 * 1024;
                            client.UploadFile(fs, Path.GetFileName(sourcefile));
                            File.Delete(sourcefile);
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(SshConnectionException ex)
            {
                TLError = TLUploadingErrors.ConnectionProblem;
                return false;
            }
            catch (SftpPermissionDeniedException ex)
            {
                TLError = TLUploadingErrors.NoPermmision;
                return false;
            }
            catch (SshException ex)
            {
                TLError = TLUploadingErrors.SSHException;
                return false;
            }
            catch (Exception ex)
            {
                TLError = TLUploadingErrors.Other;
                return false;
                //throw;
            }

        }


    }
}