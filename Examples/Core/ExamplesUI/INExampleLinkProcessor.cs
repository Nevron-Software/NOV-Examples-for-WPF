namespace Nevron.Nov.Examples.ExamplesUI
{
    /// <summary>
    /// Creates and parses example links.
    /// </summary>
    public interface INExampleLinkProcessor
    {
        /// <summary>
        /// Gets a link to the given example.
        /// </summary>
        /// <param name="exampleTypeName"></param>
        /// <returns></returns>
        string GetExampleLink(string exampleTypeName);
        /// <summary>
        /// Gets the type of the example referenced by the given URI.
        /// </summary>
        /// <param name="exampleLink"></param>
        /// <returns></returns>
        string GetExampleType(string exampleLinkUri);
    }
}