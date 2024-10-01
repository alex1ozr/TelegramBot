namespace TelegramBot.Framework.Configuration;

/// <summary>
/// Host options
/// </summary>
public class HostOptions : IOptionDescription
{
    public static string OptionKey => "Host";

    /// <summary>
    /// Service name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Namespace in which the service operates
    /// </summary>
    public string? Namespace { get; set; }

    /// <summary>
    /// Resolve host name
    /// </summary>
    public string ResolveHostName()
    {
        return Name == null || string.IsNullOrWhiteSpace(Name)
            ? throw new ArgumentException($"Configuration value '{nameof(HostOptions)}.{nameof(Name)}' not found")
            : Name;
    }

    /// <summary>
    /// Resolve host namespace
    /// </summary>
    /// <remarks>
    /// If the namespace is not specified, the machine name is used
    /// </remarks>
    public string ResolveHostNamespace()
    {
        return Namespace == null || string.IsNullOrWhiteSpace(Namespace)
            ? Environment.MachineName.ToLowerInvariant()
            : Namespace;
    }
}
