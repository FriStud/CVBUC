using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using Catel.Fody;

namespace CatelVisBeh.Models
{
    public class User : ModelBase
    {
        #region Constructors

        public User()
        {
        }

        public User(string name, string pass, Roles.Roles accessLevel)
        {
            Name = name;
            Password = pass;
            AccessLevel = accessLevel;
        }

        #endregion Constructors

        #region Properties

        [DefaultValue("default")]
        public string Name { get; set; }

        [DefaultValue("default")]
        public string Password { get; set; }

        public Roles.Roles AccessLevel { get; set; }

        public string AccessLevelString
        {
            get { return AccessLevel.ToString(); }
        }

        #endregion Properties
    }
}
