﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdentityServer.Locale {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Logout {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Logout() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("IdentityServer.Locale.Logout", typeof(Logout).Assembly);
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
        ///   Looks up a localized string similar to Would you like to logout?.
        /// </summary>
        public static string Confirm {
            get {
                return ResourceManager.GetString("Confirm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are now logged out..
        /// </summary>
        public static string Loggedout {
            get {
                return ResourceManager.GetString("Loggedout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LogOut.
        /// </summary>
        public static string LogoutTitle {
            get {
                return ResourceManager.GetString("LogoutTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The session has been closed.
        /// </summary>
        public static string LoguedOut {
            get {
                return ResourceManager.GetString("LoguedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Click {0} here {1} to return to the application {1}.
        /// </summary>
        public static string ReturnToUrl {
            get {
                return ResourceManager.GetString("ReturnToUrl", resourceCulture);
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
