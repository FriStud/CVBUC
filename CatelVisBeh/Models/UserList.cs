using Catel.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;

namespace CatelVisBeh.Models
{

    public class UserList : ModelBase
    {
        #region Properties

        [DefaultValue("E:\\CODE_TO_LIFE\\Run\\CatelVisBeh\\users")]
        public string UserFolder { get; set; } // should be defined in App.xaml.cs, DIRECTORY 

        /// <summary>
        /// Property, all users, loaded from file.
        /// </summary>
        public  ObservableCollection<User> AllUsers
        {
            get
            {
                if (GetValue<ObservableCollection<User>>(AllUserProperty) == null)
                {
                    HandleNullReferecFromFile();
                }

                return GetValue<ObservableCollection<User>>(AllUserProperty);
            }
           set { SetValue(AllUserProperty, value); }

        }
        public static readonly PropertyData AllUserProperty = RegisterProperty("AllUsers", typeof(ObservableCollection<User>));

        #endregion Properties

        #region Member Functions
        /// <summary>
        /// If the list doesnt exist and needs to be created from file.
        /// </summary>
        private void HandleNullReferecFromFile()
        {
            try
            {
                AllUsers = LoadFromFile();
            }
            catch (Exception ex)
            {
                AllUsers = new ObservableCollection<User>();
                AllUsers.Add(new User("root", UserServiceModel.EncryptPass("root"), Roles.Roles.Admin));
                SaveToFile();
            }
        }

        /// <summary>
        /// Loads Users from a file in specific directory.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<User> LoadFromFile()
        {
            var userlist = new ObservableCollection<User>();

            if (!Directory.Exists(UserFolder))
                Directory.CreateDirectory(UserFolder);

            File.SetAttributes(
                UserFolder,
                File.GetAttributes(UserFolder) | FileAttributes.Hidden | FileAttributes.System);

            var fileName = UserFolder + "8894209842390";
            using (var sw = new StreamReader(fileName))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<User>));
                userlist = serializer.Deserialize(sw) as ObservableCollection<User>;
            }
            
            File.SetAttributes(fileName, File.GetAttributes(fileName) | FileAttributes.Hidden | FileAttributes.System);

            return userlist;
        }

        /// <summary>
        /// Saves all users to the file in specific directory.
        /// </summary>
        public  void SaveToFile()
        {
            var fileName = UserFolder + "8894209842390";

            if(File.Exists(fileName))
                File.SetAttributes(fileName, FileAttributes.Normal);

            using (var sw = new StreamWriter(fileName))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<User>));
                serializer.Serialize(sw, AllUsers);
            }

            File.SetAttributes(fileName, File.GetAttributes(fileName) | FileAttributes.Hidden | FileAttributes.System);
        }
        #endregion

    }
}
