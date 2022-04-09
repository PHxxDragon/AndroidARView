using System;

namespace EAR
{
    [Serializable]
    public class LoginResponse
    {
        public string token;
        public UserProfileResponseData data;
    }
}

