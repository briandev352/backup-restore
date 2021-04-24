namespace Bring2mind.Backup.Restore.Services
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Mime;

    //event examples: https://www.tutorialsteacher.com/csharp/csharp-event

    public class HttpWebRequestDownload
    {
        private long _totalBytesLength = 0;
        private long _transferredBytes = 0;
        private int _transferredPercents => (int)((100 * _transferredBytes) / _totalBytesLength);
        private string _defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public string downloadedFilePath = String.Empty;

        public string AccessToken { get; set; } = "";

        public HttpWebRequestDownload()
        {
            //Windows 7 fix
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public string DownloadFile(string url, string destinationDirectory = default)
        {
            var destinationFile = "";
            string filename = "";
            if (destinationDirectory == default)
                destinationDirectory = _defaultDirectory;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("Cache-Control", "no-cache");
                request.Headers.Add("Cache-Control", "no-store");
                request.Headers.Add("Cache-Control", "max-age=1");
                request.Headers.Add("Cache-Control", "s-maxage=1");
                request.Headers.Add("Pragma", "no-cache");
                request.Headers.Add("Expires", "-1");
                if (!string.IsNullOrEmpty(AccessToken))
                {
                    request.Headers.Add("Authorization", "Bearer " + this.AccessToken);
                }

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result)
                {
                    _totalBytesLength = response.ContentLength;

                    string path = response.Headers["Content-Disposition"];
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        var uri = new Uri(url);
                        filename = Path.GetFileName(uri.LocalPath);
                    }
                    else
                    {
                        ContentDisposition contentDisposition = new ContentDisposition(path);
                        filename = contentDisposition.FileName;
                    }

                    destinationFile = Path.Combine(destinationDirectory, filename);

                    using (Stream responseStream = response.GetResponseStream())
                    using (FileStream fileStream = File.Create(destinationFile))
                    {
                        byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
                        ProgressEventArgs eventArgs = new ProgressEventArgs(_totalBytesLength);

                        int size = responseStream.Read(buffer, 0, buffer.Length);
                        while (size > 0)
                        {
                            fileStream.Write(buffer, 0, size);
                            _transferredBytes += size;

                            size = responseStream.Read(buffer, 0, buffer.Length);

                            eventArgs.UpdateData(_transferredBytes, _transferredPercents);
                            OnDownloadProgressChanged(eventArgs);
                        }
                    }
                }

                downloadedFilePath = Path.Combine(destinationDirectory, filename);
                OnDownloadFileCompleted(EventArgs.Empty);
            }
            catch (Exception e)
            {
                OnError($"{e.Message} => {e?.InnerException?.Message}");
            }

            return destinationFile;
        }

        //events
        public event EventHandler<ProgressEventArgs> DownloadProgressChanged;
        public event EventHandler DownloadFileCompleted;
        public event EventHandler<string> Error;

        public class ProgressEventArgs : EventArgs
        {
            public long TransferredBytes { get; set; }
            public int TransferredPercents { get; set; }
            public long TotalBytesLength { get; set; }

            public ProgressEventArgs(long transferredBytes, int transferredPercents, long totalBytesLength)
            {
                TransferredBytes = transferredBytes;
                TransferredPercents = transferredPercents;
                TotalBytesLength = totalBytesLength;
            }

            public ProgressEventArgs(long totalBytesLength)
            {
                this.TotalBytesLength = totalBytesLength;
            }

            public void UpdateData(long transferredBytes, int transferredPercents)
            {
                TransferredBytes = transferredBytes;
                TransferredPercents = transferredPercents;
            }
        }

        protected virtual void OnDownloadProgressChanged(ProgressEventArgs e)
        {
            DownloadProgressChanged?.Invoke(this, e);
        }
        protected virtual void OnDownloadFileCompleted(EventArgs e)
        {
            DownloadFileCompleted?.Invoke(this, e);
        }
        protected virtual void OnError(string errorMessage)
        {
            Error?.Invoke(this, errorMessage);
        }
    }
}
