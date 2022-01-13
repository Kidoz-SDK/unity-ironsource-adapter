# Unity ironSource Adapter + Sample App

The Kidoz Unity ironSource Adapter SDK is built and tested with ironSource mediation v7.1.13.<BR>
You should only use this version or above as it is the first stable custom mediation release. <BR>

This adapter offers support for the following ad types:

+ ironSource Interstitial Mediation 
+ ironSource Rewarded Video Mediation for **Android**
  
Until ironSource mediation SDK supports Banner mediation for custom networks and Rewarded Videos mediation on the iOS platform, publishers who choose to do so can use Kidoz banners and Rewarded directly from Kidoz SDK.<BR>
  
Before publishing your first app please finish the onboarding process for Kidoz's publishers [HERE](http://accounts.kidoz.net/publishers/register?utm_source=&utm_content=&utm_campaign=&utm_medium=)  
and follow the instructions for ironSource Custom Adapter setup [HERE](https://developers.is.com/ironsource-mobile/general/custom-adapter-setup/).<BR><BR>
Kidoz Network ID on ironSource is `2b618dcd` and you will need to setup the network level parameters with the `Publisher Id` and `Token` you got from Kidoz:  
  
  <img width="598" alt="ironSourceNetwork" src="https://user-images.githubusercontent.com/86282008/149078934-107106f0-a526-45bc-9c93-8ca53d5bf3cc.png">

Getting Started
=================================

- Download and import KidozISAdapter.unitypackage into your Unity project
- Follow ironSource instructions for Unity plugin integration [HERE](https://developers.is.com/ironsource-mobile/unity/unity-plugin)
  
IronSource Integration
=================================
  
For built in Mediation Network Integration you should follow the instructions given on the ironSource Android SDK Integration page [HERE](https://developers.is.com/ironsource-mobile/unity/mediation-networks-unity) but as far as Kidoz integration goes you only need to do the following on your Scene:
  
For supporting the Rewarded Videos(currently only for Android) call:
```c#
IronSource.Agent.setManualLoadRewardedVideo(true);
```
and call the init method with your app's ironSource key:
```c#  
IronSource.Agent.init(appKey);
```
For further demonstration on how to load,show and handle lifecycle events for Interstitials and Rewarded Videos Ads check out our Main Scene sample code [HERE](https://github.com/Kidoz-SDK/unity-ironsource-adapter/blob/main/Assets/DemoScene/MainSceneScript.cs).
  
Kidoz Direct
=================================
  
The ironSource Custom Network Mediation integration currently supports only Interstitials and Rewarded Videos(Android only).<BR>
There is however a way to load and display Banners and Rewarded Videos calling the Kidoz Unity Plugin directly.
  
Kidoz SDK initialization:
  
```c#
// Add Kidoz SDK init Events

Kidoz.initSuccess += onKidozInitSuccess;
Kidoz.initError += onKidozInitError;  
  
// Call Kidoz SDK init
  
Kidoz.init(`Publisher ID`, `Token`);
```
  
Make sure the `Publisher ID` and `Token` you send on the `KidozSDK.initialize(...)` method are your own unique parameters and not the Kidoz Test parameters used in this sample. This parameters need to be the same ones you used as network level parameters when adding the Kidoz network on the ironSource dashboard.<BR>

See the sample code for example as how to init, load and show Kidoz Banners and receive lifecycle callbacks.
  
  

