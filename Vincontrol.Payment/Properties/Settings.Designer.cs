﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vincontrol.Payment.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://certify.securenet.com/spos/sposserver.asmx")]
        public string Vincontrol_Payment_com_securenet_certify_SPOSServer {
            get {
                return ((string)(this["Vincontrol_Payment_com_securenet_certify_SPOSServer"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://certify.securenet.com/API/DATA/SERVICE.svc/soap")]
        public string Vincontrol_Payment_com_securenet_extendedServices_SERVICE {
            get {
                return ((string)(this["Vincontrol_Payment_com_securenet_extendedServices_SERVICE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://certify.securenet.com/API/Gateway.svc/soap")]
        public string Vincontrol_Payment_com_securenet_gateway_Gateway {
            get {
                return ((string)(this["Vincontrol_Payment_com_securenet_gateway_Gateway"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://certify.securenet.com/API/DATA/TRANSACTION.svc/wsHttp")]
        public string Vincontrol_Payment_com_securenet_transactionServices_TRANSACTION {
            get {
                return ((string)(this["Vincontrol_Payment_com_securenet_transactionServices_TRANSACTION"]));
            }
        }
    }
}
