﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MALShowIDCrawler.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MALShowIDCrawler.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://myanimelist.net/api/animelist/add/{0}.xml.
        /// </summary>
        public static string AddAnimeURL {
            get {
                return ResourceManager.GetString("AddAnimeURL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://myanimelist.net/anime/{0}/.
        /// </summary>
        public static string AnimeURL {
            get {
                return ResourceManager.GetString("AnimeURL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Back.
        /// </summary>
        public static string Back {
            get {
                return ResourceManager.GetString("Back", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Case Sensitive Search.
        /// </summary>
        public static string CaseSensitive {
            get {
                return ResourceManager.GetString("CaseSensitive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Crawl.
        /// </summary>
        public static string Crawl {
            get {
                return ResourceManager.GetString("Crawl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The crawler has detected your local database file already has entries up through your specified maximum ID. Would you like to overwrite these?.
        /// </summary>
        public static string CrawlerMaxIDTooSmall {
            get {
                return ResourceManager.GetString("CrawlerMaxIDTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Begin crawling through the MyAnimeList database.  Depending on the maximum ID, the process could take hours due to MyAnimeList server reponse speed and internet connection speed..
        /// </summary>
        public static string CrawlInfo {
            get {
                return ResourceManager.GetString("CrawlInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Crawl Options.
        /// </summary>
        public static string CrawlOptions {
            get {
                return ResourceManager.GetString("CrawlOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to dbinfo.
        /// </summary>
        public static string DataBaseFileName {
            get {
                return ResourceManager.GetString("DataBaseFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Consecutive Failures..
        /// </summary>
        public static string Fails {
            get {
                return ResourceManager.GetString("Fails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to After this many consecutive connection failures, regardless of the ID, the crawl will stop. This should mean you have reached the end of database list..
        /// </summary>
        public static string FailsTooltip {
            get {
                return ResourceManager.GetString("FailsTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last Crawl: {0}.
        /// </summary>
        public static string FileAccessed {
            get {
                return ResourceManager.GetString("FileAccessed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Go.
        /// </summary>
        public static string Go {
            get {
                return ResourceManager.GetString("Go", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Go to ID: .
        /// </summary>
        public static string GoTo {
            get {
                return ResourceManager.GetString("GoTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Jumps to the specified ID or the nearest one if it does not exist..
        /// </summary>
        public static string GoToIdToolTip {
            get {
                return ResourceManager.GetString("GoToIdToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Have you seen:.
        /// </summary>
        public static string HaveYouSeen {
            get {
                return ResourceManager.GetString("HaveYouSeen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ID: {0}  Status: {1}.
        /// </summary>
        public static string IDStatus {
            get {
                return ResourceManager.GetString("IDStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ID: {0}  Status: {1}  Title: {2}.
        /// </summary>
        public static string IDStatusTitle {
            get {
                return ResourceManager.GetString("IDStatusTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ID: {0}  Title: {1}.
        /// </summary>
        public static string IDTitle {
            get {
                return ResourceManager.GetString("IDTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mal ID: {0}.
        /// </summary>
        public static string MalID {
            get {
                return ResourceManager.GetString("MalID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum ID:.
        /// </summary>
        public static string MaxID {
            get {
                return ResourceManager.GetString("MaxID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The maximum MAL anime ID to query to.  The ID&apos;s are generally sequential and end at roughly 33,000..
        /// </summary>
        public static string MaxIDTooltip {
            get {
                return ResourceManager.GetString("MaxIDTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Next.
        /// </summary>
        public static string Next {
            get {
                return ResourceManager.GetString("Next", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Performs a search and jumps to the first listing that contains the search text, starting from the current ID..
        /// </summary>
        public static string NextTitleTooltip {
            get {
                return ResourceManager.GetString("NextTitleTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No.
        /// </summary>
        public static string No {
            get {
                return ResourceManager.GetString("No", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        public static string Password {
            get {
                return ResourceManager.GetString("Password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remember Credentials.
        /// </summary>
        public static string RememberInfo {
            get {
                return ResourceManager.GetString("RememberInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search title:.
        /// </summary>
        public static string Search {
            get {
                return ResourceManager.GetString("Search", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Performs a search and jumps to the first listing that contains the search text, starting from the beginning of the list..
        /// </summary>
        public static string SearchTitleTooltip {
            get {
                return ResourceManager.GetString("SearchTitleTooltip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to usersettings.
        /// </summary>
        public static string SettingsFileName {
            get {
                return ResourceManager.GetString("SettingsFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stop crawl after.
        /// </summary>
        public static string StopAfter {
            get {
                return ResourceManager.GetString("StopAfter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Username.
        /// </summary>
        public static string UserName {
            get {
                return ResourceManager.GetString("UserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User Options.
        /// </summary>
        public static string UserOptions {
            get {
                return ResourceManager.GetString("UserOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes.
        /// </summary>
        public static string Yes {
            get {
                return ResourceManager.GetString("Yes", resourceCulture);
            }
        }
    }
}
