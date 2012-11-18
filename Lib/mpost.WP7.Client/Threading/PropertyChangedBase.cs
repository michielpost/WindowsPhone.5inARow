﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Windows.Threading;

//http://www.jeff.wilcox.name/2010/04/propertychangedbase-crossthread/

namespace System.ComponentModel
{
    /// <summary>
    /// A base class for data objects that implement the property changed 
    /// interface, offering data binding and change notifications.
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// A static set of argument instances, one per property name.
        /// </summary>
        private static Dictionary<string, PropertyChangedEventArgs> _argumentInstances = new Dictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify any listeners that the property value has changed.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("PropertyName cannot be empty or null.");
            }

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs args;
                if (!_argumentInstances.TryGetValue(propertyName, out args))
                {
                    args = new PropertyChangedEventArgs(propertyName);
                    _argumentInstances[propertyName] = args;
                }

                // Fire the change event. The smart dispatcher will directly
                // invoke the handler if this change happened on the UI thread,
                // otherwise it is sent to the proper dispatcher.
                SmartDispatcher.BeginInvoke(delegate
                {
                    handler(this, args);
                });
            }
        }
    }
}
