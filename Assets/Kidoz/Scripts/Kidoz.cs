using UnityEngine;
#if UNITY_4_6 || UNITY_5
using UnityEngine.EventSystems;
#endif
using System;
using System.Collections;

#if UNITY_IOS
using KIDOZiOSInterface;
#elif UNITY_ANDROID
using KIDOZAndroidInterface;
#else
using KIDOZDummyInterface;
#endif

namespace KidozSDK
{
	
	public class Kidoz :MonoBehaviour
	{

		

		public enum PANEL_TYPE
		{
			BOTTOM = 0, TOP = 1, LEFT = 2, RIGHT = 3
		};
		
		public enum HANDLE_POSITION
		{
			START, CENTER, END
		};
		
		
		public enum BANNER_POSITION
		{
			[ObsoleteAttribute ( "Use TOP_CENTER" )]
			TOP = 0,
			[ObsoleteAttribute ( "Use BOTTOM_CENTER" )]
			BOTTOM = 1,
			TOP_CENTER = 0,
			BOTTOM_CENTER = 1,
			TOP_LEFT = 2,
			TOP_RIGHT = 3,
			BOTTOM_LEFT = 4,
			BOTTOM_RIGHT = 5
		}
		
		public enum INTERSTITIAL_AD_MODE
		{
			NORMAL = 0,
			REWARDED = 1
		}

		interface IAdLoader{
			void LoadAd ();
		}

		class BannerAdLoader : IAdLoader{

			public bool isAutoShow = false;
			public BANNER_POSITION position = BANNER_POSITION.TOP_CENTER;

			public BannerAdLoader (bool isAutoShow, BANNER_POSITION position) {
				this.isAutoShow = isAutoShow;
				this.position = position;
			}

			public void LoadAd (){
				kidozin.loadBanner (isAutoShow, (int)position);
			}
		}


		class InterstitialAdLoader : IAdLoader{

			public bool isAutoShow = false;

			public InterstitialAdLoader (bool isAutoShow) {
				this.isAutoShow = isAutoShow;
			}

			public void LoadAd (){
				kidozin.loadInterstitialAd (isAutoShow);
			}
		}

		class RewardedAdLoader : IAdLoader{

			public bool isAutoShow = false;

			public RewardedAdLoader (bool isAutoShow){
				this.isAutoShow = isAutoShow;
			}

			public void LoadAd (){
				kidozin.loadRewardedAd (isAutoShow);
			}
		}


		public static ArrayList adLoaderArray = new ArrayList ();

		public const int NO_GAME_OBJECT = -1;
		public const int PLATFORM_NOT_SUPPORTED = -2;

		public static event Action<string> initSuccess;
		
		public static event Action<string> initError;
		
		public static event Action<string> bannerContentLoaded;
		
		public static event Action<string> bannerContentLoadFailed;
		
		public static event Action<string> interstitialOpen;
		
		public static event Action<string> interstitialClose;
		
		public static event Action<string> interstitialReady;
		
		public static event Action<string> interstitialOnLoadFail;
		
		public static event Action<string> interstitialOnNoOffers;
		
		public static event Action<string> onRewardedDone;
		
		public static event Action<string> onRewardedVideoStarted;
		
		public static event Action<string> rewardedOpen;
		
		public static event Action<string> rewardedClose;
		
		public static event Action<string> rewardedReady;
		
		public static event Action<string> rewardedOnLoadFail;
		
		public static event Action<string> rewardedOnNoOffers;
		
		
		//banner
		public static event Action<string> bannerReady;
		
		public static event Action<string> bannerClose;
		
		public static event Action<string> bannerLoadError;
		
		public static event Action<string> bannerNoOffers;

		public static event Action<string> bannerShowError;


		public string PublisherID;
		public string SecurityToken;
		
		static private bool initFlag = false;
		static private bool mPause = false;

		static private string staticPublisherID ;
		static private string staticSecurityToken;


#if UNITY_IOS
		private static KIDOZiOSInterface.KIDOZiOSInterface kidozin = new KIDOZiOSInterface.KIDOZiOSInterface();
#elif UNITY_ANDROID
		private static KIDOZAndroidInterface.KIDOZAndroidInterface kidozin = new KIDOZAndroidInterface.KIDOZAndroidInterface();
#else
		private static KIDOZDummyInterface.KIDOZDummyInterface kidozin = new KIDOZDummyInterface.KIDOZDummyInterface ( );
#endif
		
		
		public const string KIDOZ_OBJECT_NAME = "KidozObject";
		
		
		#region Singelton
		
		static private Kidoz instance = null;

		private void SetStaticVars () {
			staticPublisherID = PublisherID;
			staticSecurityToken = SecurityToken;
		}

		private static void InitWithStaticVars () {
			if (!string.IsNullOrEmpty (staticPublisherID) && !string.IsNullOrEmpty (staticSecurityToken)) {
				init (staticPublisherID, staticSecurityToken);
			}
		}

		public static Kidoz Instance
		{
			get
			{
				if (instance == null)
				{
					SetInstance ( Create ( ) );
				}
				return instance;
			}
		}
		
		
		static void SetInstance (Kidoz _instance)
		{
			instance = _instance;
			DontDestroyOnLoad ( instance.gameObject );
		}
		
		void Awake ()
		{
			if (instance == null)
			{
				SetInstance ( this );

				if (!string.IsNullOrEmpty ( PublisherID ) && !string.IsNullOrEmpty ( SecurityToken ))
				{
					init ( PublisherID, SecurityToken );
				}
			}
			else
			{
				print ( "Kidoz | not allowed multiple instances" );
				Destroy ( gameObject );
			}
		}
		
		public static Kidoz Create ()
		{
			if (instance == null)
			{
				GameObject singleton = new GameObject ( KIDOZ_OBJECT_NAME );
				return singleton.AddComponent<Kidoz> ( );
			}
			return null;
		}
		
		#endregion
		
		
		public static void SetiOSAppPauseOnBackground(Boolean pause){
			mPause = pause;
		}

		public static void init (string developerID, string securityToken)
		{
			if (initFlag == true)
			{
				return;
			}
			initFlag = true;
			print ( "Kidoz | in init function -->" );
			if (instance == null)
			{
				print ( "Kidoz | in init function ==> instance == null" );
				SetInstance ( Create ( ) );
				//instance.PublisherID = developerID;
				//instance.SecurityToken = securityToken;
			}
			else
			{
				print ( "Kidoz | in init function ==> instance != null" );
			}
			print ( "Kidoz | init:" + developerID + "," + securityToken /*+ "," + "-->" + instance.PublisherID + "," + instance.SecurityToken*/ );
			kidozin.init ( developerID, securityToken );
		}
		
		
		public static bool isInitialised ()
		{
			if (kidozin == null)
			{
				return false;
			}
			return kidozin.isInitialised ( );
		}
		
		public static void testFunction ()
		{
			kidozin.testFunction ( );
		}
		
		//Basic function creation function.
		//Since Kidoz SDK should be activated only once use this function to create 
		//a game object. If Kidoz game object was added to the scene there is no need to call this function
		
	
		
		//***********************************//
		//***** INTERSTITIAL & REWARDED *****//
		//***********************************//
		// Description: Load interstitial add ---- this function is deprecated 
		// Parameters: 
		// 		
		// return:
		//		0 	- the function worked correctly
		//		NO_GAME_OBJECT	- there is no Kidoz gameobject 
		public static int loadInterstitialAd (bool isAutoShow){

			kidozin.loadInterstitialAd (isAutoShow);
			return 0;

		    /*if (!isInitialised()) {
				IAdLoader adLoader = new InterstitialAdLoader (isAutoShow);
				adLoaderArray.Add (adLoader);
				InitWithStaticVars ();

			} else
			kidozin.loadInterstitialAd ( isAutoShow );
			return 0;*/
		}
		
		public static int loadRewardedAd (bool isAutoShow){

			kidozin.loadRewardedAd (isAutoShow);
			return 0;

			/*if (!isInitialised ()) {
				IAdLoader adLoader = new RewardedAdLoader (isAutoShow);
				adLoaderArray.Add (adLoader);
				InitWithStaticVars ();

			} else
				kidozin.loadRewardedAd ( isAutoShow );
			return 0;*/
		}
		
		// Description: generate the interstitial object
		// Parameters: 
		// 		 
		// return:
		//		0 	- the function worked correctly
		//		NO_GAME_OBJECT	- there is no Kidoz gameobject 
		public static int generateInterstitial (){

			kidozin.generateInterstitial ();
			return 0;

		 /*if (!isInitialised ()) {
				IAdLoader adLoader = new InterstitialAdLoader (false);
				adLoaderArray.Add (adLoader);
				InitWithStaticVars ();

			} else
				kidozin.generateInterstitial ( );
			return 0;*/
		}
		
		public static int generateRewarded (){

			kidozin.generateRewarded ();
			return 0;

			/*if (!isInitialised ()) {
				IAdLoader adLoader = new RewardedAdLoader (false);
				adLoaderArray.Add (adLoader);
				InitWithStaticVars ();

			} else

				kidozin.generateRewarded ( );
			return 0;*/
		}
		
		// Description: show the interstitial add that was loaded
		// Parameters: 
		// 		
		// return:
		//		0 	- the function worked correctly
		//		NO_GAME_OBJECT	- there is no Kidoz gameobject 
		public static int showInterstitial ()
		{
			kidozin.showInterstitial ( );
			return 0;
		}
		
		public static int showRewarded ()
		{
			kidozin.showRewarded ( );
			return 0;
		}
		
		// Description: return if an interstitial add was loaded
		// Parameters: 
		// 		
		// return:
		//		0 	- interstitial add was not loaded
		//		NO_GAME1_OBJECT	- there is no Kidoz gameobject 
		public static bool getIsInterstitialLoaded ()
		{
			return kidozin.getIsInterstitialLoaded ( );
			
		}
		
		public static bool getIsRewardedLoaded ()
		{
			return kidozin.getIsRewardedLoaded ( );
			
		}
		
		
		//******************//
		//***** BANNER *****//
		//******************//
		
		
		
		
		
		
		// Description: add Banner to view 
		// Parameters: 
		// 		int - banner position
		// return:
		//		0 	- the function worked correctly
		//		NO_GAME_OBJECT	- there is no Kidoz gameobject 
		public static int loadBanner (bool isAutoShow, BANNER_POSITION position){

			kidozin.loadBanner (isAutoShow, (int)position);
			return 0;


			/*if (!isInitialised()) {
				IAdLoader adLoader = new BannerAdLoader (isAutoShow, position);
				adLoaderArray.Add (adLoader);
				InitWithStaticVars ();
			} else
				kidozin.loadBanner ( isAutoShow, (int) position );
			return 0;*/
		}
		
		// Description: set Banner Position 
		// Parameters: 
		// 		int - banner position
		// return:
		//		0 	- the function worked correctly
		//		NO_GAME_OBJECT	- there is no Kidoz gameobject 
		public static int setBannerPosition (BANNER_POSITION position)
		{
			kidozin.setBannerPosition ( (int) position );
			return 0;
		}
		
		
		// Description: show the banner 
		// Parameters: 
		// 		N/A
		// return:
		//		0 	- the function worked correctly
		//		NO_GAME_OBJECT	- there is no Kidoz gameobject 
		public static int showBanner ()
		{
			kidozin.showBanner ( );
			return 0;
		}
		
		// Description: hide the banner 
		// Parameters: 
		// 		N/A
		// return:
		//		0 	- the function worked correctly
		//		NO_GAME_OBJECT	- there is no Kidoz gameobject 
		public static int hideBanner ()
		{
			kidozin.hideBanner ( );
			return 0;
		}
		
		
		
		
		public static void printToastMessage (String message)
		{
			kidozin.logMessage ( message );
		}
		
		///listeners calls backs
		////////////////////////////////////
		public void initSuccessCallback (string message)
		{
			if (adLoaderArray.Count > 0) {

				for (int i = adLoaderArray.Count - 1; i >= 0; i--) {
					((IAdLoader)adLoaderArray [i]).LoadAd ();
					adLoaderArray.RemoveAt (i);
				}
			}

			if (initSuccess != null)
			{
				initSuccess ( message );
			}
		}
		
		public void initErrorCallback (string message)
		{
			if (initError != null)
			{
				initError ( message );
			}
		}
		

		public void bannerReadyCallBack (string message)
		{
			if (bannerReady != null)
			{
				bannerReady ( message );
			}
		}
		
		public void bannerCloseCallBack (string message)
		{
			if (bannerClose != null)
			{
				bannerClose ( message );
			}
		}
		
		public void bannerLoadErrorCallBack(string message)
		{
			if (bannerLoadError != null)
			{
				bannerLoadError ( message );
			}
		}
		
		public void bannerNoOffersCallBack (string message)
		{
			if (bannerNoOffers != null)
			{
				bannerNoOffers (message);
			}
		}

		public void bannerShowErrorCallBack(string message)
		{
			if (bannerShowError != null)
			{
				bannerShowError(message);
			}
		}





		//***********************************//
		//***** INTERSTITIAL & REWARDED *****//
		//***********************************//

		public void interstitialOpenCallBack (string message)
		{
			
			#if UNITY_IOS
			if(mPause){
				Time.timeScale = 0;
				AudioListener.pause = true;
			}
			#endif
			
			if (interstitialOpen != null)
			{
				interstitialOpen ( message );
			}
		}
		
		public void interstitialCloseCallBack (string message)
		{
			#if UNITY_IOS
			if(mPause){
				Time.timeScale = 1;
				AudioListener.pause = false;
			}
			#endif
			
			if (interstitialClose != null)
			{
				interstitialClose ( message );
			}
		}
		
		public void interstitialReadyCallBack (string message)
		{
			if (interstitialReady != null)
			{
				interstitialReady ( message );
			}
		}
		
		public void interstitialOnLoadFailCallBack (string message)
		{
			if (interstitialOnLoadFail != null)
			{
				interstitialOnLoadFail ( message );
			}
		}
		
		public void interstitialOnNoOffersCallBack (string message)
		{
			if (interstitialOnNoOffers != null)
			{
				interstitialOnNoOffers ( message );
			}
		}
		
		public void onRewardedCallBack (string message)
		{
			if (onRewardedDone != null)
			{
				onRewardedDone ( message );
			}
		}
		
		public void onRewardedVideoStartedCallBack (string message)
		{
			if (onRewardedVideoStarted != null)
			{
				onRewardedVideoStarted ( message );
			}
		}
		
		public void rewardedOpenCallBack (string message)
		{
			
			#if UNITY_IOS
			if(mPause){
				Time.timeScale = 0;
				AudioListener.pause = true;
			}
			#endif
			
			if (rewardedOpen != null)
			{
				rewardedOpen ( message );
			}
		}
		
		public void rewardedCloseCallBack (string message)
		{
			
			#if UNITY_IOS
			if(mPause){
				Time.timeScale = 1;
				AudioListener.pause = false;
			}
			#endif
			
			if (rewardedClose != null)
			{
				rewardedClose ( message );
			}
		}
		
		public void rewardedReadyCallBack (string message)
		{
			if (rewardedReady != null)
			{
				rewardedReady ( message );
			}
		}
		
		public void rewardedOnLoadFailCallBack (string message)
		{
			if (rewardedOnLoadFail != null)
			{
				rewardedOnLoadFail ( message );
			}
		}
		
		public void rewardedOnNoOffersCallBack (string message)
		{
			if (rewardedOnNoOffers != null)
			{
				rewardedOnNoOffers ( message );
			}
		}

	}

}
