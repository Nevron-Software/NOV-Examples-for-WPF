using System;

using Nevron.Nov.Examples.ExamplesUI;

namespace Nevron.Nov.Examples.Wpf
{
    internal class NWpfExampleLinkProcessor : INExampleLinkProcessor
    {
        public string GetExampleLink(string exampleTypeName)
        {
            return Scheme + exampleTypeName;
        }

        public string GetExampleType(string exampleLinkUri)
        {
            if (String.IsNullOrEmpty(exampleLinkUri))
                return null;

            if (exampleLinkUri[exampleLinkUri.Length - 1] == '/')
            {
                exampleLinkUri = exampleLinkUri.Remove(exampleLinkUri.Length - 1);
            }

            if (exampleLinkUri.StartsWith(Scheme, StringComparison.OrdinalIgnoreCase))
                return exampleLinkUri.Substring(Scheme.Length);
            else
                return null;
        }

        private const string Scheme = "nov-wpf://";
    }
}