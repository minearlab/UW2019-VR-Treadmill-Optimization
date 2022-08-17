# UW2019-VR-Treadmill-Optimization
Optimization of Virtualizer omnidirectional treadmill.  Describes an approach to increasing degrees of freedom when using the Virtualizer by decoupling the user's head rotation from that of their body.  Developed by the University of Wyoming Spatial Cognition Lab and Presented at IEEE VR 2019 in Osaka, Japan. 

### *Full Source is not available since it uses artistic assets to which we do not have distribution rights.  The main scripts developed by the lab (and some others) however, are included.*
```diff
+ If you are experiencing unnatural slow down, try turning off Debugging
```

```diff
!- WARNING -!                                                                                             
```
* The Code AND the Fully functional Unity app provided, will NOT work unless you have the CybSDK DLLs installed on your machine.
* Because of this, we have included them under "Treadmill/CybSDK/DLLs/" for you.
* There are two version, one for 64 bit and the other for 32 bit machines.
* We reccommend downloading and adding both to your Unity project and letting Unity itself decide which to choose.
---

### Executable Version Can Be Found Here ---> https://www.dropbox.com/s/3tcu0ijgmxti3fk/IEEEVR2019_Work.zip?dl=0

## Usage
  * The Critical Code can be found under the "Treadmill" folder.  Be warned that it requires Cyberith's suite of development scripts (and a Virtualizer Omnidirectional Treadmill) to actually run.  If you have the requsites, great! If not, we hope the code can at least get the gears moving on how to apply a similar solution to your projects.
  
  * The Majority of the files in this repo are likely not necessary.  However, they do define some useful functions when working with UI and are included as extras that you may use (with citation) or take from in order to streamline your development process.



