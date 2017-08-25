using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetOpenAuth.OAuth2;

namespace vincontrol.VinSocial.YoutubeHelper
{
    /// <summary>
    /// Describes a flow which captures the authorization code out of the window title of the browser.
    /// </summary>
    /// <remarks>Works on Windows, but not on Unix. Will failback to copy/paste mode if unsupported.</remarks>
    internal class WindowTitleNativeAuthorizationFlow : INativeAuthorizationFlow
    {
        private const string OutOfBandCallback = "urn:ietf:wg:oauth:2.0:oob";

        public string RetrieveAuthorization(UserAgentClient client, IAuthorizationState authorizationState)
        {
            // Create the Url.
            authorizationState.Callback = new Uri(OutOfBandCallback);
            Uri url = client.RequestUserAuthorization(authorizationState);

            // Show the dialog.
            if (!Application.RenderWithVisualStyles)
            {
                Application.EnableVisualStyles();
            }

            Application.DoEvents();
            string authCode = string.Empty;//OAuth2AuthorizationDialog.ShowDialog(url);
            Application.DoEvents();

            if (string.IsNullOrEmpty(authCode))
            {
                return null; // User cancelled the request.
            }

            return authCode;
        }
    }
}
