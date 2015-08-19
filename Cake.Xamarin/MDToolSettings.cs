using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.Tools;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Xamarin
{

    public class MDToolSettings
    {
        public MDToolSettings ()
        {
            Configuration = "Debug|iPhoneSimulator";
            Target = "Build";
        }

        public FilePath ToolPath { get; set; }

        public bool IncreaseVerbosity { get; set; }

        public string Configuration { get; set; }

        public string Target { get; set; }
    }
    
}
