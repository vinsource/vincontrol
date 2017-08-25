using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vincontrol.VinSocial.YoutubeHelper
{
    /// <summary>
    /// A choice option which can be picked by the user
    /// Used by the CommandLine class
    /// </summary>
    public sealed class UserOption
    {
        /// <summary>
        /// Creates a new option based upon the specified data
        /// </summary>
        /// <param name="name">Name to display</param>
        /// <param name="target">Target function to call if this option was picked</param>
        public UserOption(string name, Action target)
        {
            Name = name;
            Target = target;
        }

        /// <summary>
        /// Name/Keyword to display
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The function which will be called if this option was picked
        /// </summary>
        public Action Target { get; private set; }
    }
}