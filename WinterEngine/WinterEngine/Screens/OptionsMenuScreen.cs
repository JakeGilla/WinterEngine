#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using WinterEngine;
#endregion

namespace WinterEngine
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry screenMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry cheatsMenuEntry;
        MenuEntry volumeMenuEntry;

        enum DisplayType
        {
            FullScreen,
            Windowed,
        }

        static DisplayType currentDisplayType = DisplayType.Windowed;

        //KeyBinding currentControlBindings;

        static int currentControlBindings = 0;

        static string[] languages = { "C#", "Engrish", "Nonya" };
        static int currentLanguage = 0;

        static bool cheats = true;

        static int volume = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen(Game game)
            : base(game, "Options")
        {
            // Create our menu entries.
            screenMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            cheatsMenuEntry = new MenuEntry(string.Empty);
            volumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            screenMenuEntry.Selected += ScreenMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            cheatsMenuEntry.Selected += CheatsMenuEntrySelected;
            volumeMenuEntry.Selected += VolumeMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(screenMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(cheatsMenuEntry);
            MenuEntries.Add(volumeMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            screenMenuEntry.Text = "Controls: " + currentControlBindings;
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            cheatsMenuEntry.Text = "Frobnicate: " + (cheats ? "on" : "off");
            volumeMenuEntry.Text = "Volume: " + volume;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the screen menu entry is selected.
        /// This handles switching between fullscreen and windowed mode.
        /// </summary>
        void ScreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentDisplayType++;

            if (currentDisplayType > DisplayType.Windowed)
                currentDisplayType = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void CheatsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            cheats = !cheats;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void VolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            volume++;

            SetMenuEntryText();
        }


        #endregion
    }
}
