using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Doe.Ls.EntityBase.Helper
    {
    public class BrowserHelper
        {
        public HttpRequest Request { get; }
        public HttpBrowserCapabilities BrowserCapabilities { get; }
        public string UserAgent;

        private BrowserHelper(HttpRequest request)

            {
            Request = request;
            BrowserCapabilities = request.Browser;
            UserAgent = request.UserAgent;
            }
        public static BrowserHelper CreateInstance(HttpRequest request)
            {
            return new BrowserHelper(request);
            }

        public static BrowserHelper CreateInstance()
            {
            return new BrowserHelper(HttpContext.Current.Request);
            }

        public BrowserEnum Browser()
            {
            if(Request.Browser.Browser.Equals("InternetExplorer", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.InternetExplorer;
            if(Request.Browser.Browser.Equals("IE", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.InternetExplorer;

            if(Request.Browser.Browser.Equals("Chrome", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.Chrome;
            if(Request.Browser.Browser.Equals("ChromePlus", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.ChromePlus;


            if(Request.Browser.Browser.Equals("Firefox", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.Firefox;
            if(Request.Browser.Browser.Equals("Mozilla", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.Mozilla;

            if(Request.Browser.Browser.Equals("Safari", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.Safari;

            if(Request.Browser.Browser.Equals("Opera", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.Opera;
            if(Request.Browser.Browser.Equals("Opera Mini", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.OperaMini;
            if(Request.Browser.Browser.Equals("Opera Mobile", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.OperaMobile;


            if(Request.Browser.Browser.Equals("AOL", StringComparison.CurrentCultureIgnoreCase)) return BrowserEnum.AOL;




            return BrowserEnum.Other;
            }

        public Version Version()
            {
            var b = Request.Browser;
            return new Version
                {
                VersionString = b.Version,
                MajorVersion = b.MajorVersion,
                MinorVersion = b.MinorVersion,
                JScriptVersion = b.JScriptVersion,
                EcmaScriptVersion = b.EcmaScriptVersion,

                };

            }

        }


    public enum BrowserEnum
        {
        Other,
        ABrowse
,
        AcooBrowser
,
        AmericaOnlineBrowser
,
        AmigaVoyager
,
        AOL
,
        Arora
,
        AvantBrowser
,
        Beonex
,
        BonEcho
,
        Browzar
,
        Camino
,
        Charon
,
        Cheshire
,
        Chimera
,
        Chrome
,
        ChromePlus
,
        Classilla
,
        CometBird
,
        ComodoDragon
,
        Conkeror
,
        CrazyBrowser
,
        Cyberdog
,
        DeepnetExplorer
,
        DeskBrowse
,
        Dillo
,
        Dooble
,
        Edge
,
        ElementBrowser
,
        Elinks
,
        EnigmaBrowser
,
        EnigmaFox
,
        Epiphany
,
        Escape
,
        Firebird
,
        Firefox
,
        FirewebNavigator
,
        Flock
,
        Fluid
,
        Galaxy
,
        Galeon
,
        GranParadiso
,
        GreenBrowser
,
        Hana
,
        HotJava
,
        IBMWebExplorer
,
        IBrowse
,
        iCab
,
        Iceape
,
        IceCat
,
        Iceweasel
,
        iNetBrowser
,
        InternetExplorer
,
        iRider
,
        Iron
,
        KMeleon
,
        KNinja
,
        Kapiko
,
        Kazehakase
,
        KindleBrowser
,
        KKman
,
        KMLite
,
        Konqueror
,
        LeechCraft
,
        Links
,
        Lobo
,
        lolifox
,
        Lorentz
,
        Lunascape
,
        Lynx
,
        Madfox
,
        Maxthon
,
        Midori
,
        Minefield
,
        Mozilla
,
        myibrow
,
        MyIE2
,
        Namoroka
,
        Navscape
,
        NCSAMosaic
,
        NetNewsWire
,
        NetPositive
,
        Netscape
,
        NetSurf
,
        OmniWeb
,
        Opera
,
        Orca
,
        Oregano
,
        osbbrowser
,
        Palemoon
,
        Phoenix
,
        Pogo
,
        Prism
,
        QtWebInternetBrowser
,
        Rekonq
,
        retawq
,
        RockMelt
,
        Safari
,
        SeaMonkey
,
        Shiira
,
        Shiretoko
,
        Sleipnir
,
        SlimBrowser
,
        Stainless
,
        Sundance
,
        Sunrise
,
        surf
,
        Sylera
,
        TencentTraveler
,
        TenFourFox
,
        theWorldBrowser
,
        uzbl
,
        Vimprobable
,
        Vonkeror
,
        w3m
,
        WeltweitimnetzBrowser
,
        WorldWideWeb
,
        Wyzo
,
        MOBILEBROWSERS
,
        AndroidWebkitBrowser
,
        BlackBerry
,
        Blazer
,
        Bolt
,
        BrowserforS60
,
        Doris
,
        Dorothy
,
        Fennec
,
        GoBrowser
,
        IEMobile
,
        Iris
,
        MaemoBrowser
,
        MIB
,
        Minimo
,
        NetFront
,
        OperaMini
,
        OperaMobile
,
        SEMCBrowser
,
        Skyfire
,
        TeaShark
,
        TelecaObigo
,
        uZardWeb


        }

    public class Version
        {
        public string VersionString { get; set; }
        public int MajorVersion { get; set; }
        public double MinorVersion { get; set; }
        public System.Version JScriptVersion { get; set; }
        public System.Version EcmaScriptVersion { get; set; }

        public override string ToString()
        {
            return VersionString;
        }
        }

    }
