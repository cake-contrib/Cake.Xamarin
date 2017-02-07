using Cake.Core.IO;

namespace Cake.Xamarin
{
    /// <summary>
    /// Xamarin component tool settings.
    /// </summary>
    public class XamarinComponentSettings : Cake.Core.Tooling.ToolSettings
    {
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
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Xamarin.XamarinComponentUploadSettings"/> class.
        /// </summary>
        public XamarinComponentUploadSettings () : base ()
        {
            MaxAttempts = 3;
        }

        /// <summary>
        /// How many attempts should be made to upload the component before failing
        /// </summary>
        /// <value>The max attempts.</value>
        public int MaxAttempts { get; set; }
    }

    /// <summary>
    /// Xamarin component restore command settings.
    /// </summary>
    public class XamarinComponentSubmitSettings : XamarinComponentCredentialSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Xamarin.XamarinComponentSubmitSettings"/> class.
       /// </summary>
        public XamarinComponentSubmitSettings () : base ()
        {
            MaxAttempts = 3;
        }

        /// <summary>
        /// How many attempts should be made to upload the component before failing
        /// </summary>
        /// <value>The max attempts.</value>
        public int MaxAttempts { get; set; }
    }

}
