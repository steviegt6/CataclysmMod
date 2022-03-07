#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using Terraria.Localization;

namespace CataclysmMod.Core.Localization
{
    /// <summary>
    ///     Hard-coded localization entries for various dumb purposes.
    /// </summary>
    public static class FilelessEntries
    {
        public static string GetStartup()
        {
            switch (LanguageManager.Instance.ActiveCulture.Name)
            {
            }
            
            return "Thank you for installing Cataclysm!" +
                   "\nThis mod adds additional content, tweaks, balancing changes, and other things for an ever-growing list of mods, including Calamity, Thorium, Clicker Class, and Split." +
                   "\nThis may not be your first time using the mod, but a lot of changes were made with the 2.0.0 update." +
                   "\nPlease read this message in full to understand what has changed." +
                   "\n" +
                   "\nIF YOU HAVE ANY SUGGESTIONS, WANT CERTAIN CHANGES TO BE MADE CONFIGURABLE, OR HAVE ANY BUGS TO REPORT, REPORT THEM [link/github:HERE]!" +
                   "\n" +
                   "\nYou can now view all mods with additional content in the \"Cataclysm Addons\" menu. You can also view a list of individual changes, as well as their associated mod config." +
                   "\nAdditionally, Cataclysm will start receiving frequent updates once more." +
                   "\nWhile this update has a new major version, no new content has been implemented; only a plethora of bug fixes and other goodies have been added." +
                   "\n" +
                   "\nIf you like Cataclysm, consider:" +
                   "\n* joining the [link/discord:Discord server]" +
                   "\n* leaving a star on our [link/github:GitHub repository]" +
                   "\n" +
                   "\n";
        }

        public static string GetActualChangelog()
        {
            switch (LanguageManager.Instance.ActiveCulture.Name)
            {
            }

            return "Thanks for downloading 2.0.0!" +
                   "\nThe above message will not display after the first time Cataclysm is loaded." +
                   "\nAny subsequent updates will boast a proper changelog message here. I appreciate your guys' support greatly!";
        }

        public static string GetClickEffect(string name)
        {
            switch (name)
            {
                case "Impostor":
                    switch (LanguageManager.Instance.ActiveCulture.Name)
                    {
                    }

                    return "Impostor";
            }

            return $"\"{name}\" not found!";
        }

        public static string GetClickDescription(string name)
        {
            switch (name)
            {
                case "Impostor":
                    switch (LanguageManager.Instance.ActiveCulture.Name)
                    {
                    }

                    return "Can mimic any other effect";
            }
            
            return $"\"{name}\" not found!";
        }

        public static string GetCalamityDescription()
        {
            switch (LanguageManager.Instance.ActiveCulture.Name)
            {
            }

            return "";
        }
        
        public static string GetClickerClassDescription()
        {
            switch (LanguageManager.Instance.ActiveCulture.Name)
            {
            }

            return "* Added new Clicker weapons for each gem variant" +
                   "\n* Added a debug Clicker that can cycle through each Click Effect";
        }
        
        public static string GetThoriumDescription()
        {
            switch (LanguageManager.Instance.ActiveCulture.Name)
            {
            }

            return "* Globee is twice as likely to spawn" +
                   "\n* Modded Stained Glass variants now match vanilla sell prices";
        }
        
        public static string GetSplitDescription()
        {
            switch (LanguageManager.Instance.ActiveCulture.Name)
            {
            }

            return "* Added the Pharaoh's Fear, an upgrade to the Terror Shield" +
                   "\n* Raining projectiles from Menace can be blocked using an Umbrella";
        }
    }
}