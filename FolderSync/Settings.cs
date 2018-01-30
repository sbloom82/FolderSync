using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class Settings
    {

        public string LastSourceFolder { get; private set; }
        public bool SortSourceByModified { get; private set; }
        public string LastDestinationFolder { get; private set; }
        public bool SortDestinationByModified { get; private set; }

        public void SetValue(string name, object value)
        {
            switch (name)
            {
                case nameof(LastSourceFolder):
                    LastSourceFolder = (string)value;
                    break;
                case nameof(SortSourceByModified):
                    SortSourceByModified = (bool)value;
                    break;
                case nameof(LastDestinationFolder):
                    LastDestinationFolder = (string)value;
                    break;
                case nameof(SortDestinationByModified):
                    SortDestinationByModified = (bool)value;
                    break;
            }

            lock (this)
            {
                var key = Registry.CurrentUser.OpenSubKey(@"Software\FolderSync", true);
                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey(@"Software\FolderSync");
                }

                using (key)
                {
                    key.SetValue(name, value);
                }
            }
        }

        public void Read()
        {
            lock (this)
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\FolderSync"))
                {
                    if (key == null)
                        return;

                    foreach (var name in key.GetValueNames())
                    {
                        string value = key.GetValue(name)?.ToString();
                        if (value != null)
                        {
                            switch (name)
                            {
                                case nameof(LastSourceFolder):
                                    LastSourceFolder = value;
                                    break;
                                case nameof(SortSourceByModified):
                                    SortSourceByModified = bool.Parse(value);
                                    break;
                                case nameof(LastDestinationFolder):
                                    LastDestinationFolder = value;
                                    break;
                                case nameof(SortDestinationByModified):
                                    SortDestinationByModified = bool.Parse(value);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
