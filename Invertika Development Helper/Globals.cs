using System;
using System.Collections.Generic;
using System.Text;
using CSCL;

namespace Invertika_Development_Helper
{
    public class Globals
    {
        public static string OptionsDirectory=FileSystem.ApplicationDataDirectory+".invertika.org\\Invertika Development Helper\\";
        public static string OptionsXmlFilename=OptionsDirectory+"Invertika Development Helper.xml";
        public static XmlData Options;
    }
}