namespace DasKeyboard.Q
{
    /// <summary>
    /// Represents the method of OAuth 2.0 authentication to use to access the
    /// Q REST API.
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// Do not authenticate.
        /// </summary>
        None,

        /// <summary>
        /// Use the client credentials grant type.
        /// </summary>
        ClientCredentials,

        /// <summary>
        /// Use the resource owner password credentials grant type.
        /// </summary>
        Password,

        /// <summary>
        /// Use the authorization code grant type.
        /// </summary>
        AuthorizationCode,

        /// <summary>
        /// Use a refresh token.
        /// </summary>
        RefreshToken,
    }
}
