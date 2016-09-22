namespace Beyova.Gravity
{
    /// <summary>
    /// Interface IBaseGravityWebSocketClientProtocol
    /// </summary>
    internal interface IBaseGravityWebSocketClientProtocol
    {
        #region Server trigger actions

        void ReloadConfiguration();

        void HeartBeat();

        #endregion
    }
}
