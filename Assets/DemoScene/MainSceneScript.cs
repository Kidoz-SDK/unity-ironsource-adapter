using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using KidozSDK;



// Example for IronSource Unity.
public class MainSceneScript : MonoBehaviour
{

    private String text;
    public int maxLines = 10;
    private List<string> Eventlog = new List<string>();

#if UNITY_ANDROID
        string appKey = "11cfc977d";
        string sdk_version = "8.9.5";
#elif UNITY_IPHONE
        string appKey = "11ec9bd9d";
        string sdk_version = "8.9.1";
#else
    string appKey = "unexpected_platform";
    string sdk_version = "unexpected_platform";
#endif


    string is_sdk_version = IronSource.pluginVersion();

    public void Start()
    {
        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("unity-script: IronSource.Agent.init");      
        IronSource.Agent.setManualLoadRewardedVideo(true);        
        IronSource.Agent.init(appKey);

        // This is not neccesary if you don't call Kidoz Plugin Directly
        // If you do - make sure to replace the test publisherId and token with your own.
        Kidoz.init("5", "i0tnrdwdtq0dm36cqcpg6uyuwupkj76s");
        AddEvent("Initializing Kidoz SDK:: v" + sdk_version);

    }

    void OnEnable()
    {

        //Add ironSource Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdReadyEvent += RewardedVideoAdReadyEvent;
        IronSourceEvents.onRewardedVideoAdLoadFailedEvent += RewardedVideoAdLoadFailedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;        
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;

        // Add ironSource Interstitial Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        // Add Kidoz SDK init Events
        Kidoz.initSuccess += onKidozInitSuccess;
        Kidoz.initError += onKidozInitError;

        // Add Kidoz Rewraded Video Events
        Kidoz.onRewardedDone += onRewarded;
        Kidoz.onRewardedVideoStarted += onRewardedVideoStarted;
        Kidoz.rewardedOpen += rewardedOpen;
        Kidoz.rewardedClose += rewardedClose;
        Kidoz.rewardedReady += rewardedReady;
        Kidoz.rewardedOnLoadFail += rewardedOnLoadFail;
        Kidoz.rewardedOnNoOffers += rewardedOnNoOffers;

        // Add Kidoz Banner Events
        Kidoz.bannerReady += bannerReady;
        Kidoz.bannerClose += bannerClose;
        Kidoz.bannerError += bannerError;
        Kidoz.bannerNoOffers += bannerNoOffers;
    }

    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    public void OnGUI()
    {

        GUI.backgroundColor = Color.white;
        GUI.skin.button.fontSize = (int)(0.035f * Screen.width);

        GUIStyle labelStyle = GUI.skin.GetStyle("Label");
        labelStyle.fontSize = (int)(0.045f * Screen.width);
        labelStyle.normal.textColor = Color.black;

        float lineHeight = 0.15f;
        #region IronSource Mediation

        Rect labelRect = new Rect(0.05f * Screen.width, lineHeight * Screen.height, Screen.width, 0.08f * Screen.height);
        GUI.Label(labelRect, "ironSource v" + is_sdk_version, labelStyle);

        lineHeight = lineHeight + 0.05f;
        Rect loadInterstitialButton = new Rect(0.10f * Screen.width, lineHeight * Screen.height, 0.35f * Screen.width, 0.08f * Screen.height);
        if (GUI.Button(loadInterstitialButton, "Load Interstitial"))
        {
            Debug.Log("unity-script: LoadInterstitialButtonClicked");
            IronSource.Agent.loadInterstitial();
            AddEvent("Loading Interstitial");
        }

        Rect showInterstitialButton = new Rect(0.55f * Screen.width, lineHeight * Screen.height, 0.35f * Screen.width, 0.08f * Screen.height);
        if (GUI.Button(showInterstitialButton, "Show Interstitial"))
        {
            Debug.Log("unity-script: ShowInterstitialButtonClicked");
            if (IronSource.Agent.isInterstitialReady())
            {
                IronSource.Agent.showInterstitial();
                AddEvent("Show Interstitial");
            }
            else
            {
                Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
                AddEvent("Show Interstitial - Not Ready");
            }
        }

       

        lineHeight = lineHeight + 0.10f;
        Rect loadRewardedButton = new Rect(0.10f * Screen.width, lineHeight * Screen.height, 0.35f * Screen.width, 0.08f * Screen.height);
        if (GUI.Button(loadRewardedButton, "Load Rewarded"))
        {
            Debug.Log("unity-script: LoadRewardedButtonClicked");
            IronSource.Agent.loadManualRewardedVideo();
            AddEvent("Loading Rewarded");
        }

        Rect showRewardedButton = new Rect(0.55f * Screen.width, lineHeight * Screen.height, 0.35f * Screen.width, 0.08f * Screen.height);
        if (GUI.Button(showRewardedButton, "Show Rewarded"))
        {
            Debug.Log("unity-script: ShowRewardedButtonClicked");
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
                AddEvent("Show Rewarded");
            }
            else
            {
                Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
                AddEvent("Show Rewarded - Not Ready");
            }
        }
        

#endregion

#region Kidoz Direct
        lineHeight = lineHeight + 0.10f;
        Rect labelRect_ = new Rect(0.05f * Screen.width, lineHeight * Screen.height, Screen.width, 0.08f * Screen.height);
        GUI.Label(labelRect_, "Kidoz Direct v" + sdk_version, labelStyle);
        lineHeight = lineHeight + 0.05f;        
        Rect loadBannerButton = new Rect(0.10f * Screen.width, lineHeight * Screen.height, 0.35f * Screen.width, 0.08f * Screen.height);
        if (GUI.Button(loadBannerButton, "Load Banner"))
        {
            Kidoz.loadBanner(true, Kidoz.BANNER_POSITION.BOTTOM_CENTER);
            AddEvent("Loading Bottom Banner");
        }

        Rect destroyBannerButton = new Rect(0.55f * Screen.width, lineHeight * Screen.height, 0.35f * Screen.width, 0.08f * Screen.height);
        if (GUI.Button(destroyBannerButton, "Close Banner"))
        {
            Kidoz.hideBanner();
        }

#endregion

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = (int)(0.025f * Screen.width);
        guiStyle.normal.textColor = Color.black;
        float btnHeight = 0.55f * Screen.height;
        GUI.Label(new Rect(30, btnHeight, Screen.width - 30, Screen.height - btnHeight), text, guiStyle);

    }




#region IronSource Interstitial callback handlers

    void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
        AddEvent("Interstitial Ad Ready");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        AddEvent("Interstitial load failed:\ncode: " + error.getCode() + "\ndescription: " + error.getDescription());
    }

    void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
        AddEvent("Interstitial Ad Show Succeeded");
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        AddEvent("Interstitial show failed:\ncode: " + error.getCode() + "\ndescription: " + error.getDescription());
    }

    void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
        AddEvent("Interstitial Ad Clicked");
    }

    void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
        AddEvent("Interstitial Ad Opened");
    }

    void InterstitialAdClosedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
        AddEvent("Interstitial Ad Closed"); ;
    }

    ///************* Interstitial DemandOnly Delegates *************/

    //void InterstitialAdReadyDemandOnlyEvent(string instanceId)
    //{
    //    Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
    //}

    //void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    //{
    //    Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
    //}

    //void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    //{
    //    Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
    //}

    //void InterstitialAdClickedDemandOnlyEvent(string instanceId)
    //{
    //    Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
    //}

    //void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
    //{
    //    Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
    //}

    //void InterstitialAdClosedDemandOnlyEvent(string instanceId)
    //{
    //    Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
    //}

    #endregion

#region IronSource Rewarded callback handlers

    void RewardedVideoAdReadyEvent()
    {
        Debug.Log("unity-script: I got RewardedAdReadyEvent");
        AddEvent("Rewarded Ad Ready");
    }

    void RewardedVideoAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        AddEvent("Rewarded load failed:\ncode: " + error.getCode() + "\ndescription: " + error.getDescription());
    }

    void RewardedVideoAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got RewardedAdShowSucceededEvent");
        AddEvent("Rewarded Ad Show Succeeded");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        AddEvent("Rewarded show failed:\ncode: " + error.getCode() + "\ndescription: " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedAdClickedEvent");
        AddEvent("Rewarded Ad Clicked");
    }

    void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedAdOpenedEvent");
        AddEvent("Rewarded Ad Opened");
    }

    void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedAdClosedEvent");
        AddEvent("Rewarded Ad Closed"); ;
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent");
        AddEvent("Rewarded Ad Rewarded"); ;
    }

    #endregion

    #region Kidoz Direct

    #region Kidoz Direct init
    public void onKidozInitSuccess(string value)
    {
        print("SampleCode | KidozInitSuccess");
        Kidoz.printToastMessage("KidozInitSuccess");
        AddEvent("Kidoz Init Success");

    }

    public void onKidozInitError(string value)
    {
        string errMsg = "KidozInitError: " + value;
        print("SampleCode | " + errMsg);
        Kidoz.printToastMessage(errMsg);
        AddEvent("Kidoz Init error:: " + errMsg);
    }

#endregion

#region Kidoz Direct Rewarded callback handlers
    private void onRewarded(string value)
    {
        print("SampleCode |onRewardedDone");
        Kidoz.printToastMessage("SampleCode | onRewarded");
        AddEvent("On User Rewarded:: " + value);
    }

    private void onRewardedVideoStarted(string value)
    {
        print("SampleCode |onRewardedVideoStarted");
        AddEvent("Rewarded Video Started");
    }

    private void rewardedOpen(string value)
    {
        AddEvent("Rewarded Started");
    }

    private void rewardedClose(string value)
    {
        AddEvent("Rewarded Close");
    }

    private void rewardedReady(string value)
    {
        Kidoz.printToastMessage("SampleCode | rewardedReady");
        AddEvent("Rewarded Ready");
    }

    private void rewardedOnLoadFail(string value)
    {
        Kidoz.printToastMessage("SampleCode | rewardedOnLoadFail");
        AddEvent("Rewarded Load Fail:: " + value);
    }

    private void rewardedOnNoOffers(string value)
    {
        print("SampleCode | rewardedOnNoOffers");
        Kidoz.printToastMessage("SampleCode | rewardedOnNoOffers");
        AddEvent("Rewarded No Offers:: " + value);
    }
#endregion



#region Kidoz Direct Banner callback handlers
    private void bannerReady(string value)
    {
        print("SampleCode | bannerReady");
        Kidoz.printToastMessage("SampleCode | bannerReady");
        AddEvent("Banner Ready");
    }

    private void bannerClose(string value)
    {
        print("SampleCode | bannerHide");
        Kidoz.printToastMessage("SampleCode | bannerHide");
        AddEvent("Banner Hide");
    }

    private void bannerError(string value)
    {
        print("SampleCode | bannerError: " + value);
        Kidoz.printToastMessage("SampleCode | bannerError: " + value);
        AddEvent("Banner Error:: " + value);
    }

    private void bannerNoOffers(string value)
    {
        print("SampleCode | bannerNoOffers");
        Kidoz.printToastMessage("SampleCode | bannerNoOffers");
        AddEvent("Banner No Offers:: " + value);
    }

#endregion

#endregion

    public void AddEvent(string eventString)
    {
        Eventlog.Add(System.DateTime.Now + " - " + eventString);

        if (Eventlog.Count >= maxLines)
            Eventlog.RemoveAt(0);

        text = "";

        foreach (string logEvent in Eventlog)
        {
            text += logEvent;
            text += "\n";
        }
    }

}

