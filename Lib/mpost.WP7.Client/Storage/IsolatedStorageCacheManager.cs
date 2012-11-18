using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.IO;

namespace mpost.WP7.Client.Storage
{
    public static class IsolatedStorageCacheManager<T>
    {
        public static void Store(string filename, T obj)
        {
            IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication();
            using (IsolatedStorageFileStream fileStream = appStore.OpenFile(filename, FileMode.Create))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fileStream, obj);
            }
        }
        public static T Retrieve(string filename)
        {
            T obj = default(T);
            IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (appStore.FileExists(filename))
            {
                using (IsolatedStorageFileStream fileStream = appStore.OpenFile(filename, FileMode.Open))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    obj = (T)serializer.ReadObject(fileStream);
                }
            }
            return obj;
        }
    }
}
