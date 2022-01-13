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
  
For supporting for Rewarded Videos(currently only for Android) call:
```c#
  IronSource.Agent.setManualLoadRewardedVideo(true);
```
