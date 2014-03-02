using System;
using System.Collections.Generic;
using System.IO;

namespace Nx.Cloud.Blobs
{
    public abstract class BlobData<T> : IBlobData<T>
        where T : class
    {
        private string _Id;
        public string Id
        {
            get { return _Id; }
        }

        public long Size
        {
            get { return Data.Length; }
        }

        private ContentType _ContentType;
        public ContentType ContentType
        {
            get { return _ContentType; }
        }

        private IDictionary<string, string> _MetaData;
        public IDictionary<string, string> MetaData
        {
            get
            {
                if (_MetaData == null)
                {
                    _MetaData = new Dictionary<string, string>();
                    _MetaData["Id"] = Id;
                    _MetaData["Size"] = Size.ToString();
                    _MetaData["ContentType"] = GetContentType(ContentType);
                }

                return _MetaData;
            }
        }

        private Stream _Data;
        public Stream Data
        {
            get { return _Data; }
        }

        private static string GetContentType(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Image:
                    return "image/jpeg";
                case ContentType.Text:
                    return "text/plain";
                case ContentType.Xml:
                    return "text/xml";
                case ContentType.Video:
                    return "video/avi";
                case ContentType.Audio:
                    return "audio/mpeg3";
                case ContentType.Binary:
                    return "application/octet-stream";
                default:
                    throw new ArgumentException("Unsupported content type");
            }
        }

        /// <summary>
        /// Byte-array constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <remarks>Converts the byte array to a memory stream and takes ownership of it</remarks>
        public BlobData(string id, byte[] data, ContentType contentType)
            : this(id, new MemoryStream(data, 0, data.Length), contentType)
        {
        }

        /// <summary>
        /// Stream constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <remarks>Takes ownership of the data stream</remarks>
        public BlobData(string id, Stream data, ContentType contentType)
        {
            _Id = id;
            _Data = data;
            _Data.Seek(0L, SeekOrigin.Begin);

            _ContentType = contentType;
        }

        ~BlobData()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Data.Close();
                Data.Dispose();
            }
        }
    }
}
