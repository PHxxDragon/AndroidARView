using System;

namespace EAR
{
    [Serializable]
    public class UserProfileResponse
    {
        public UserProfileResponseData data;
    }

    [Serializable]
    public class UserProfileResponseData
    {
        public UserProfileData user;
    }
}

