using System.Security.Cryptography;
using UnityEngine.Networking;
using System;
using System.IO;
using UnityEngine;

namespace EAR.DownloadHandler
{
    public class DecryptDownloadHandler : DownloadHandlerScript
    {
        private const int HEADER_LENGTH = 32;
        private const int IV_LENGTH = 16;
        private const int HEADER_AND_IV_LENGTH = HEADER_LENGTH + IV_LENGTH * 2;
        private const string ENC_HEADER = "f7b025810bc4592b542544ac4b4fb0a501fc5e8fa49593297dca71de831f1787";

        private MemoryStream memoryStream;
        private bool isMemoryStreamInitialized;
        private bool isEncrypted;

        private ulong contentLength;
        private ulong downloadedLength;

        private byte[] headerAndIVs;
        private byte[] firstIV;
        private byte[] lastIV;
        private int currentHeadLength;
        private byte[] _data = null;

        public DecryptDownloadHandler(byte[] buffer) : base(buffer)
        {
            contentLength = 0;
            downloadedLength = 0;
            currentHeadLength = 0;
            headerAndIVs = new byte[HEADER_AND_IV_LENGTH];
        }

        protected override byte[] GetData()
        {
            return _data;
        }

        protected override float GetProgress()
        {
            if (contentLength != 0)
            {
                return (float)downloadedLength / contentLength;
            }
            return 1;
        }

        protected override void ReceiveContentLengthHeader(ulong contentLength)
        {
            this.contentLength = contentLength;
        }

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            downloadedLength += (ulong) dataLength;
            if (!isMemoryStreamInitialized)
            {
                int offset = 0;
                if (currentHeadLength < HEADER_AND_IV_LENGTH)
                {
                    int addedLength = Math.Min(HEADER_AND_IV_LENGTH - currentHeadLength, dataLength);
                    Buffer.BlockCopy(data, 0, headerAndIVs, currentHeadLength, addedLength);
                    currentHeadLength += addedLength;
                    offset = addedLength;
                }

                if (currentHeadLength > HEADER_AND_IV_LENGTH)
                {
                    Debug.LogError("Error receiving IV");
                    return false;
                }

                if (currentHeadLength == HEADER_AND_IV_LENGTH)
                {
                    byte[] header = new byte[HEADER_LENGTH];
                    Buffer.BlockCopy(headerAndIVs, 0, header, 0, HEADER_LENGTH);
                    isMemoryStreamInitialized = true;
                    memoryStream = contentLength != 0 ? new MemoryStream((int) contentLength) : new MemoryStream();
                    if (!Utils.ByteArrayCompare(header, Utils.StringToByteArrayFastest(ENC_HEADER)))
                    {
                        isEncrypted = false;
                        memoryStream.Write(headerAndIVs, 0, headerAndIVs.Length);
                    }
                    else
                    {
                        isEncrypted = true;
                        firstIV = new byte[IV_LENGTH];
                        Buffer.BlockCopy(headerAndIVs, HEADER_LENGTH, firstIV, 0, IV_LENGTH);
                        lastIV = new byte[IV_LENGTH];
                        Buffer.BlockCopy(headerAndIVs, HEADER_LENGTH + IV_LENGTH, lastIV, 0, IV_LENGTH);
                    }
                    memoryStream.Write(data, offset, dataLength - offset);
                }
            } else
            {
                memoryStream.Write(data, 0, dataLength);
            } 
            return true;
        }

        protected override void CompleteContent()
        {
            if (isMemoryStreamInitialized)
            {
                memoryStream.Flush();
                memoryStream.Close();
                _data = memoryStream.ToArray();
                if (isEncrypted)
                {
                    MyCryptography myCryptography = new MyCryptography();
                    myCryptography.Decrypt(_data, firstIV, lastIV);
                }
            }
        }

        private class MyCryptography
        {
            
            private const string ENC_KEY = "74f8aa7ce7f9b769ba8c22ad541763b3c7704d377a3dc544a3359a1512903518";
            private const int ENC_SIZE = 1024 * 1024;
            public void Decrypt(byte[] buffer, byte[] firstIV, byte[] lastIV)
            {
                int length = buffer.Length;
                byte[] lastPart = length < ENC_SIZE ?
                    new byte[roundToDivisibleBy16(length)] : new byte[ENC_SIZE];
                Buffer.BlockCopy(buffer, (int) length - lastPart.Length, lastPart, 0, lastPart.Length);
                byte[] lastPartDecrypted = DecryptBuffer(lastPart, lastIV);
                Buffer.BlockCopy(lastPartDecrypted, 0, buffer, (int) length - lastPartDecrypted.Length, lastPartDecrypted.Length);

                byte[] firstPart = length < ENC_SIZE ?
                    new byte[roundToDivisibleBy16(length)] : new byte[ENC_SIZE];
                Buffer.BlockCopy(buffer, 0, firstPart, 0, firstPart.Length);
                byte[] firstPartDecrypted = DecryptBuffer(firstPart, firstIV);
                Buffer.BlockCopy(firstPartDecrypted, 0, buffer, 0, firstPartDecrypted.Length);
            }

            private long roundToDivisibleBy16(long number)
            {
                return (number >> 4) << 4;
            }

            private byte[] DecryptBuffer(byte[] buffer, byte[] iv)
            {
                using Aes aes = Aes.Create();
                aes.Key = Utils.StringToByteArrayFastest(ENC_KEY);
                aes.IV = iv;
                aes.Padding = PaddingMode.None;
                byte[] result;
                ICryptoTransform decryptor = aes.CreateDecryptor();
                using MemoryStream memoryStream = new MemoryStream();
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(buffer, 0, buffer.Length);
                    result = memoryStream.ToArray();
                }             
                return result;
            }
        }
    }
}
