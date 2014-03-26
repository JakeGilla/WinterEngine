using System;

namespace WinterEngine
{
    [Serializable]
    struct StartUpState
    {
        /// <summary>
        /// This struct is a start-up file. Its purpose
        /// is to hold data that allows auto-selection upon game start. Global states
        /// such as game configurations and key bindings are automatically saved
        /// and loaded from the ConfigManager, which utilizes this struc to serialize
        /// the data to a binary file.
        /// 
        /// NOTE! This is the file known as wesus.dat
        /// </summary>
        /// 
        private bool useCustomConfigFile;
        private string customConfigFilePath;

        private bool playSpashScreen;
        private bool splashScreenIsVideo;
        private string splashScreenFilePath;

        public bool UseCustomConfigFile 
        { 
            get { return useCustomConfigFile; }
            set { useCustomConfigFile = value; } 
        }
        public string CustomConfigFilePath
        {
            get { return customConfigFilePath; }
            set { customConfigFilePath = value; }
        }

        public bool PlaySpashScreen
        {
            get { return playSpashScreen; }
            set { playSpashScreen = value; }
        }
        public bool SplashScreenIsVideo
        {
            get { return splashScreenIsVideo; }
            set { splashScreenIsVideo = value; }
        }
        public string SplashScreenFilePath
        {
            get { return splashScreenFilePath; }
            set { splashScreenFilePath = value; }
        }

        public StartUpState(bool useCustConfig, string custConfigPath,
            bool playSplash = true, bool splashIsVideo = true,
            string splashFilePath = @"Videos/splashSample")
        {
            this.useCustomConfigFile = useCustConfig;
            this.customConfigFilePath = custConfigPath;
            this.playSpashScreen = playSplash;
            this.splashScreenIsVideo = splashIsVideo;
            this.splashScreenFilePath = splashFilePath;
        }
    }
}