using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class ConfigManager
    {
        #region data fields
        private static string configDefaultFile = @"winterConfig.xml";
        private static string videoSplashDefaultFile = @"Videos\swk";
        private string userConfigFilePath;
        private string splashFilePath;
        private readonly string wesus = "wesus.dat";
        private bool playSplashScreen;
        private StartUpState sus;
        private XElement currConfigs;
        private bool grabCameraControls = false;

        public static string VideoSplashDefaultFile
        {
            get { return videoSplashDefaultFile; }
        }

        public bool PlaySplashScreen
        { get { return sus.PlaySpashScreen; } }

        #endregion

        public void Initialize()
        {
            /*this is bad style, I couldn't get the structure to properly use
             * automatic properties and constructor 
            */
            sus = new StartUpState(false, null);

            if(File.Exists(wesus))
            {
                sus = ReadDefaultSusFile();

                if (sus.UseCustomConfigFile)  // use user-def config file
                {
                    userConfigFilePath = sus.CustomConfigFilePath;
                    currConfigs = ReadFromConfigFile(userConfigFilePath);
                }
                else // use default config file
                {
                    currConfigs = ReadFromConfigFile();
                }

                if (sus.PlaySpashScreen) // splash screen will be added to screenmanager
                {
                    splashFilePath = sus.SplashScreenFilePath;
                }
            } else // file doesn't exist, set to default
            {
                WriteDefaultSusFile();
                currConfigs = WriteDefaultConfigs();
            }
        }

        private XElement ReadFromConfigFile() 
        {
            try
	        {
                XElement kbXml = XElement.Load(configDefaultFile);
                return kbXml;
	        }
	        catch (System.IO.FileNotFoundException e)
	        {
                Debug.WriteLine(e.Message);
                return null;
	        }
        }

        private static XElement ReadFromConfigFile(String configFile)
        {
            try
            {
                XElement kbXml = XElement.Load(configFile);
                return kbXml;
            }
            catch (System.IO.FileNotFoundException e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        private static XElement WriteDefaultConfigs()
        {
            if (!File.Exists(configDefaultFile))
            {
                XElement winterConfigFile =
                    new XElement("GeneralConfiguration",
                        new XComment("DummyConfigurationFile"),
                        new XElement("DefaultResolution", new XAttribute("Fullscreen", "False"),
                            new XElement("Width", new XAttribute("Unit", "px"), "1080"),
                            new XElement("Height", new XAttribute("Unit", "px"), "720")
                        ),
                        new XElement("DefaultVResolution", new XAttribute("Camera", "Smooth"),
                            new XElement("Width", new XAttribute("Unit", "px"), "640"),
                            new XElement("Height", new XAttribute("Unit", "px"), "480")
                        ),
                        new XElement("KeyBindings", new XAttribute("Profile", "Default"),
                            new XElement("GameState", new XAttribute("State","MainMenuState"),
                                new XElement("Group", new XAttribute("Name", "Menu"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Up"), "MenuUp"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Left"), "MenuLeft"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Down"), "MenuDown"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Right"), "MenuRight")
                                ),
                                new XElement("Group", new XAttribute("Name", "Movement"),
                                    new XElement("KeyBinding", new XAttribute("Key", "W"), "GameUp"),
                                    new XElement("KeyBinding", new XAttribute("Key", "A"), "GameLeft"),
                                    new XElement("KeyBinding", new XAttribute("Key", "S"), "GameDown"),
                                    new XElement("KeyBinding", new XAttribute("Key", "D"), "GameRight")
                                ),
                                new XElement("Group", new XAttribute("Name", "Actions"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Space"), "Action1"),
                                    new XElement("KeyBinding", new XAttribute("Key", "C"), "Action2"),
                                    new XElement("KeyBinding", new XAttribute("Key", "F"), "Action3"),
                                    new XElement("KeyBinding", new XAttribute("Key", "V"), "Action4")
                                ),
                                new XElement("Group", new XAttribute("Name", "Camera"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Q"), "RotateCC"),
                                    new XElement("KeyBinding", new XAttribute("Key", "E"), "RotateCW"),
                                    new XElement("KeyBinding", new XAttribute("Mouse", "WheelUp"), "ZoomIn"),
                                    new XElement("KeyBinding", new XAttribute("Mouse", "WheelDown"), "ZoomOut")
                                )
                            ),
                            new XElement("GameState", new XAttribute("State", "GamePlayState"),
                                new XElement("Group", new XAttribute("Name", "Menu"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Up"), "MenuUp"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Left"), "MenuLeft"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Down"), "MenuDown"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Right"), "MenuRight")
                                ),
                                new XElement("Group", new XAttribute("Name", "Movement"),
                                    new XElement("KeyBinding", new XAttribute("Key", "W"), "GameUp"),
                                    new XElement("KeyBinding", new XAttribute("Key", "A"), "GameLeft"),
                                    new XElement("KeyBinding", new XAttribute("Key", "S"), "GameDown"),
                                    new XElement("KeyBinding", new XAttribute("Key", "D"), "GameRight")
                                ),
                                new XElement("Group", new XAttribute("Name", "Actions"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Space"), "Action1"),
                                    new XElement("KeyBinding", new XAttribute("Key", "C"), "Action2"),
                                    new XElement("KeyBinding", new XAttribute("Key", "F"), "Action3"),
                                    new XElement("KeyBinding", new XAttribute("Key", "V"), "Action4")
                                ),
                                new XElement("Group", new XAttribute("Name", "Camera"),
                                    new XElement("KeyBinding", new XAttribute("Key", "Q"), "RotateCC"),
                                    new XElement("KeyBinding", new XAttribute("Key", "E"), "RotateCW"),
                                    new XElement("KeyBinding", new XAttribute("Mouse", "WheelUp"), "ZoomIn"),
                                    new XElement("KeyBinding", new XAttribute("Mouse", "WheelDown"), "ZoomOut")
                                )
                            )
                        )
                    );
                try
                {
                    winterConfigFile.Save(configDefaultFile);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    throw;
                }

                return winterConfigFile;
            } else 
                return null;
        }

        private bool WriteDefaultSusFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            bool sucessful = false;

            StartUpState tempSus = new StartUpState(false, null, true, true, videoSplashDefaultFile);

            try
            {
                using (Stream fStream = new FileStream(wesus,
                FileMode.Create, FileAccess.Write, FileShare.None
                ))
                {
                    bf.Serialize(fStream, tempSus);
                    sucessful = true;
                    fStream.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            return sucessful;
        }

        private StartUpState ReadDefaultSusFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            StartUpState tempSus = new StartUpState();

            try
            {
                using (Stream fStream = new FileStream(wesus,
                    FileMode.Open, FileAccess.Read, FileShare.None
                    ))
                {
                    object obj;
                    obj = bf.Deserialize(fStream);
                    fStream.Close();
                    if (obj is StartUpState)
                    {
                        tempSus = (StartUpState)obj;
                    }
                }
            } catch (Exception e)
            {
                Debug.WriteLine("~~ ERROR ~~");
                Debug.WriteLine(e.Message);
            }
            return tempSus;
        }

        public Point GetStartUpResolution (out bool fullscreen)
        {
            int w =  0, h = 0;
            fullscreen = false;

            IEnumerable<XElement>  defaultRes = 
                from elem in currConfigs.Descendants("DefaultResolution")
                select elem;
            foreach(XElement res in defaultRes)
            {
                fullscreen = (bool)res.Attribute("Fullscreen");
            }

            IEnumerable<XElement> dimensions =
                from elem in currConfigs.Elements("DefaultResolution").Elements()
                select elem;
            foreach(XElement elem in dimensions)
            {
                if (elem.Name == "Width")
                {
                    w = Convert.ToInt32(elem.Value);
                }
                else if (elem.Name == "Height")
                {
                    h = Convert.ToInt32(elem.Value);
                }
            }
            return new Point(w,h);
        }

        public Point GetStartUpVResolution()
        {
            int w = 0, h = 0;

            IEnumerable<XElement> dimensions =
                from elem in currConfigs.Elements("DefaultVResolution").Elements()
                select elem;
            foreach (XElement elem in dimensions)
            {
                if (elem.Name == "Width")
                {
                    w = Convert.ToInt32(elem.Value);
                }
                else if (elem.Name == "Height")
                {
                    h = Convert.ToInt32(elem.Value);
                }
            }
            return new Point(w, h);
        }

        public Controls GetControls()
        {
            //Controls controls;
            Dictionary<Keys,String> mmDict = new Dictionary<Keys,string>();
            Dictionary<Keys,String> gpDict = new Dictionary<Keys,string>();
            Dictionary<Controls.MouseButtons, string> mbMMDict = new Dictionary<Controls.MouseButtons, string>();
            Dictionary<Controls.MouseButtons, string> mbGPDict = new Dictionary<Controls.MouseButtons, string>();

            // grab all key bindings
            IEnumerable<XElement> mMenuState =
                from menuStateElems in currConfigs.Descendants("KeyBindings").Descendants("GameState")
                where (string)menuStateElems.Attribute("State") == "MainMenuState"
                select menuStateElems;

             IEnumerable<XElement> gPlayState =
                from gameStateElems in currConfigs.Descendants("KeyBindings").Descendants("GameState")
                where (string)gameStateElems.Attribute("State") == "GamePlayState"
                select gameStateElems;

            // 1st
            foreach (XElement menuStateElems in mMenuState)
            {
                IEnumerable<XElement> menuStateBindings = 
                    from menuBindElems in menuStateElems.Descendants()
                    select menuBindElems;

                foreach (XElement menuBindElems in menuStateBindings)
                {
                    string strName = menuBindElems.Name.ToString();

                    if (strName.Equals("Group"))
                    {
                        // do something here for all group elements
                        string groupName = menuBindElems.Attribute("Name").Value.ToString();

                        // create grouping here
                        if (groupName.Equals("Camera"))
                        {
                            // do something group specific
                        } else
                        {
                            
                        }
                    } else
                    {
                        string attName = menuBindElems.FirstAttribute.Name.ToString();

                        if (attName.Equals("Mouse"))
                        {
                            mbMMDict.Add(GetMbButton(menuBindElems.FirstAttribute), menuBindElems.Value.ToString());
                        } else
                        {
                            mmDict.Add(GetKeys(menuBindElems.Attribute("Key")), menuBindElems.Value.ToString());
                        }
                    }
                }
            }

            // 2nd
            foreach (XElement gameStateElems in gPlayState)
            {
                IEnumerable<XElement> gameStateBindings =
                    from gameBindElems in gameStateElems.Descendants()
                    select gameBindElems;

                foreach (XElement gameBindElems in gameStateBindings)
                {
                    string strName = gameBindElems.Name.ToString();

                    if (strName.Equals("Group"))
                    {
                        // do something here for all group elements
                        string groupName = gameBindElems.Attribute("Name").Value.ToString();

                        // create grouping here
                        if (groupName.Equals("Camera"))
                        {
                            // do something group specific
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        string attName = gameBindElems.FirstAttribute.Name.ToString();

                        if (attName.Equals("Mouse"))
                        {
                            mbGPDict.Add(GetMbButton(gameBindElems.FirstAttribute), gameBindElems.Value.ToString());
                        }
                        else
                        {
                            gpDict.Add(GetKeys(gameBindElems.Attribute("Key")), gameBindElems.Value.ToString());
                        }
                        // gpDict.Add(GetKeys(gameBindElems.Attribute("Key")), gameBindElems.Value.ToString());
                    }
                }
            }

            return new Controls(new MainMenuCtrlScheme(mmDict, mbMMDict), new GamePlayCtrlScheme(gpDict, mbGPDict));
        }

        /// <summary>
        /// This helper function converts from XML Attribute to XNA Keys
        /// </summary>
        /// <param name="xAtt"></param>
        /// <returns></returns>
        private Keys GetKeys(XAttribute xAtt)
        {
            return (Keys)Enum.Parse(typeof(Keys), xAtt.Value.ToString());
        }

        private Controls.MouseButtons GetMbButton(XAttribute xAtt)
        {
            return (Controls.MouseButtons)Enum.Parse(typeof(Controls.MouseButtons), xAtt.Value.ToString());
        }
    }
}
