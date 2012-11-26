using System;

namespace Hishop.TransferManager
{
    public class YfxTarget : Target
    {
        public const string TargetName = "易分销";

        public YfxTarget(string versionString)
            : base("易分销", versionString)
        {
        }

        public YfxTarget(Version version)
            : base("易分销", version)
        {
        }

        public YfxTarget(int major, int minor, int build)
            : base("易分销", major, minor, build)
        {
        }
    }
}

