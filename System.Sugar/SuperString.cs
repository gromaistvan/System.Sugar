namespace System.Sugar
{
    using Text;

    /// <summary>
    /// Fancy class for using smart string manipulation.
    /// </summary>
    public class SuperString
    {
        /// <summary>
        /// Used for implementation logic.
        /// </summary>
        protected StringBuilder Internal { get; } = new StringBuilder();
    }
}
