using UnityEngine;
using UnityEngine.Advertisements;

namespace Script.Menu.DataManagment
{
    public class Ads : MonoBehaviour
    {
        // Serialize private fields
        //  instead of making them public.
        [SerializeField] string iosGameId;
        [SerializeField] private static string _androidGameId = "1030201";

        [SerializeField] private static bool _enableTestMode = false;

        public void Awake()
        {
            Advertisement.debugLevel = Advertisement.DebugLevel.None;
            string gameId = null;

#if UNITY_IOS // If build platform is set to iOS...
        gameId = iosGameId;
#elif UNITY_ANDROID // Else if build platform is set to Android...
            gameId = _androidGameId;
#endif

            if (string.IsNullOrEmpty(gameId))
            {
                // Make sure the Game ID is set.
                Debug.LogError("Failed to initialize Unity Ads. Game ID is null or empty.");
            }
            else if (!Advertisement.isSupported)
            {
                Debug.LogWarning("Unable to initialize Unity Ads. Platform not supported.");
            }
            else
            {
                Advertisement.Initialize(gameId, _enableTestMode);
            }
        }

        public void ShowAds()
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
        }
    }
}