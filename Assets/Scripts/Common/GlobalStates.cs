using System;

namespace EAR
{
    public class GlobalStates
    {
        public enum Mode
        {
            ViewModel, EditModel, EditARModule, ViewARModule
        }

        public enum SaveStatus
        {
            Saved, Savable, Saving, 
        }

        public static event Action<Mode> OnModeChange;
        public static event Action<bool> OnEnableScreenshotChange;
        public static event Action<bool> OnIsPlayModeChange;
        public static event Action<SaveStatus> OnSavableChange;

        public delegate void MouseRaycastHandler(ref bool isBlocked);
        public static event MouseRaycastHandler CheckMouseRaycastBlocked;

        private static Mode mode = Mode.EditARModule;
        private static bool enableScreenshot = true;
        private static SaveStatus savable = SaveStatus.Saved;
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

        public static SaveStatus GetSaveStatus()
        {
            return savable;
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

        public static void SetSavable(SaveStatus value)
        {
            if (savable != value)
            {
                savable = value;
                OnSavableChange?.Invoke(value);
            }
        }
    }
}

