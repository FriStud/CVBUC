using Catel.Data;
using CatelVisBeh.Roles;
using CatelVisBeh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatelVisBeh.Models
{
    public class ActionModel : ModelBase
    {
        public ActionModel()
        {
            AllRoles = EnumUtil.GetValues<Roles.Roles>();
        }

        public Roles.Roles SelectedRole { get; set; }
        public IEnumerable<Roles.Roles> AllRoles { get; set; }
    }
}
