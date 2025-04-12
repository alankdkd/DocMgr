using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DocMgr
{
    internal class WebUtils
    {
        public static async void SendFileToUrl(string filePath, string uploadUrl)
        {
            //string filePath = @"C:\path\to\your\file.txt"; // Local file path
            //string uploadUrl = "http://example.com/upload"; // URL of the upload endpoint

            //filePath = filePath.Replace('\\', '/');
            uploadUrl = uploadUrl.Replace('\\', '/');
            uploadUrl = "ftp://" + uploadUrl;
            // Call the method to upload the file
            await UploadFileAsync(filePath, uploadUrl);
        }


        static async Task UploadFileAsync(string filePath, string ftpUrl)
        {
            //string ftpUrl = "ftp://www.priorartband.com/temp/Backups/Alan/Green Tea.rtf";
            //string filePath = @"C:\Users\alank\Documents\Green Tea.rtf";
            string ftpUsername = "alan"; // Replace with your FTP username
            string ftpPassword = "#"; // Replace with your FTP password

            UploadFileToFtp(ftpUrl, filePath, ftpUsername, ftpPassword);
        }

        static void UploadFileToFtp(string ftpUrl, string filePath, string username, string password)
        {
            try
            {
                // Get the file's content as a byte array
                byte[] fileContents = File.ReadAllBytes(filePath);

                // Create an FTP request to upload the file
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                //request.UsePassive = false;     // CHATGPT SUGGESTION.
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(username, password);
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = false;

                // Upload the file
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                // Get response from the server
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload Complete, status: {response.StatusDescription}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
