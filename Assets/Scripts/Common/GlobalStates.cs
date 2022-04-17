using System;

namespace EAR
{
    public class GlobalStates
    {
        public enum Mode
        {
            ViewModel, EditModel, EditARModule, ViewARModule
        }

        public static event Action<Mode> OnModeChange;
        public static event Action<bool> OnEnableScreenshotChange;
        public static event Action<bool> OnIsPlayModeChange;

        public delegate void MouseRaycastHandler(ref bool isBlocked);
        public static event MouseRaycastHandler CheckMouseRaycastBlocked;

        private static Mode mode = Mode.EditARModule;
        private static bool enableScreenshot = true;
        private static bool isPlayMode = false;

        public static bool IsMouseRaycastBlocked()
        {
            bool isBlocked = false;
            CheckMouseRaycastBlocked?.Invoke(ref isBlocked);
            return isBlocked;
        }

        public static bool IsPlayMode()
        {
            return isPlayMode;
        }

        public static Mode GetMode()
        {
            return mode;
        }

        public static bool IsEnableScreenshot()
        {
            return enableScreenshot;
        }

        public static void SetIsPlayMode(bool value)
        {
            if (isPlayMode != value)
            {
                isPlayMode = value;
                OnIsPlayModeChange?.Invoke(value);
            }
        }

        public static void SetMode(Mode mode)
        {
            if (mode != GlobalStates.mode)
            {
                GlobalStates.mode = mode;
                OnModeChange?.Invoke(mode);
            }
        }

        public static void SetEnableScreenshot(bool value)
        {
            if (enableScreenshot != value)
            {
                enableScreenshot = value;
                OnEnableScreenshotChange?.Invoke(value);
            }
        }
    }
}

