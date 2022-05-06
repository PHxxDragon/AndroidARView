using System;

namespace EAR
{
    [Serializable]
    public class ErrorResponse
    {
        public string status;
        public Error error;
        public string message;
    }

    [Serializable]
    public class Error
    {
        public string name;
        public string message;
        public string status;
        public int statusCode;
    }
}

