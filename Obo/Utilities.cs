using System;
using System.IO;

namespace Obo
{
    public static class Utilities
    {
        public static string GetProgramName() => Path.GetFileName(Environment.GetCommandLineArgs()[0]);
    }
}