using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System;
using UnityEngine;

public class zIPManager
{

    static bool debug = false;

    // for some reason android does not see the network interface (only finds one that "unknown", so we search by IPproperty in this case

    public static string myIP()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return ipByProperty();
#else
        return ipByType();
#endif
    }


    public static string ipByType()
    {
        string discoveredIP = "";

        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        //do what you want with the IP here... add it to a list, just get the first and break out. Whatever.
                        if (discoveredIP == "") discoveredIP = ip.Address.ToString();
                        if (debug) Debug.Log("zIPManager ipByType(): Discovered IP: " + discoveredIP);
                    }
                }
            }
        }
        if (discoveredIP == "")
        {
            if (debug) Debug.LogError("zIPManager ipByType():  interface Type: no IP Address found, setting to localhost");
             discoveredIP = "127.0.0.1";
        }
        if (debug) Debug.Log("zIPManager ipByType(): ASSIGNING IP: " + discoveredIP);
        return (discoveredIP);
    }


    static string ipByProperty()
    {
        string discoveredIP = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    NetworkInterfaceType itype = item.NetworkInterfaceType;

                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        discoveredIP = ip.Address.ToString();
                        if (debug) Debug.Log("zIPManager ipByProperty():  interface Type: " + itype + " Discovered IP: " + discoveredIP);
                    }
                }
            }
        }
        if (discoveredIP == "")
        {
            if (debug) Debug.LogError("zIPManager ipByProperty():  interface Type: no IP Address found, setting to localhost");
            discoveredIP = "127.0.0.1";
        }
        if (debug) Debug.Log("zIPManager ipByProperty(): ASSIGNING IP: " + discoveredIP);
        return (discoveredIP);
    }
}