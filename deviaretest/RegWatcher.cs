using deviaretest;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

class RegWatcher : ManagementEventWatcher
{

    public event EventHandler<EventArgs> RegKeyChangeEvent;

    /// <exception cref="System.Security.SecurityException">
    /// Thrown when current user does not have the permission to access the key 
    /// to monitor.
    /// </exception> 
    /// <exception cref="System.ArgumentException">
    /// Thrown when the key to monitor does not exist.
    /// </exception> 
    public RegWatcher()
    {

        // Construct the query string.
        string queryString = string.Format(@"SELECT * FROM RegKeyChangeEvent 
                   WHERE Hive = '{0}' AND KeyPath = '{1}' ", "HKEY_LOCAL_MACHINE", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

        WqlEventQuery query = new WqlEventQuery(queryString);
        query.EventClassName = "RegKeyChangeEvent";
        query.WithinInterval = new TimeSpan(0, 0, 0, 1);
        this.Query = query;

        this.EventArrived += new EventArrivedEventHandler(RegWatcher_EventArrived);


    }

    void RegWatcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
        if (RegKeyChangeEvent != null)
        {
            FormInterface.listViewAddItem(FormInterface.GetInstance().signsListView, "StartupRegChange");
            Debug.WriteLine(e.NewEvent.Properties["KeyPath"].Value as string);

        }
    }

}

