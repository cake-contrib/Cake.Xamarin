using Cake.Core.IO;

namespace Cake.Xamarin
{
    /// <summary>
    /// Xamarin component tool settings.
    /// </summary>
    public class XamarinComponentSettings
    {
        /// <summary>
        /// Gets or sets the xamarin-component.exe path.
        /// </summary>
        /// <value>The path to xamarin-component.exe.</value>
        public FilePath ToolPath { get; set; }
    }

    /// <summary>
    /// Xamarin component tool settings.
    /// </summary>
    public abstract class XamarinComponentCredentialSettings : XamarinComponentSettings
    {
        /// <summary>
        /// Gets or sets the Xamarin Account Email to log in with
        /// </summary>
        /// <value>The Xamarin Account Email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Xamarin Account Password to log in with.
        /// </summary>
        /// <value>The Xamarin Account Password.</value>
        public string Password { get; set; }
    }

    /// <summary>
    /// Xamarin component restore command settings.
    /// </summary>
    public class XamarinComponentRestoreSettings : XamarinComponentCredentialSettings
    {
    }

    /// <summary>
    /// Xamarin component restore command settings.
    /// </summary>
    public class XamarinComponentUploadSettings : XamarinComponentCredentialSettings
    {
    }

    /// <summary>
    /// Xamarin component restore command settings.
    /// </summary>
    public class XamarinComponentSubmitSettings : XamarinComponentCredentialSettings
    {
    }

}
