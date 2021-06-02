# Target-Base-Techniques

This repository includes all the unity3d project files for the VR target-based locomotion techniques implementation. It includes three technique implementations: 
  1. *Motion* (Auto move by targeting)
  2. *Automatic* (Auto move along pre-defined route)
  3. *Teleport* (Teleport by targeting)


### AnimatedTeleport.cs

 Implementation of the *Motion* locomotion technique. 
 
 ### AutomatedTeleport.cs

Implementation of the *Automatic* locomotion technique.
 
 ### Teleport.cs

 Implementation of the *Teleport* locomotion technique.
 
 ### TravelTask.cs

 Implementation of the travel task. The number of column targets, the diameter of the target, and the diameter of the task circle can be adjusted through the inspector.
 
 ### CognitiveTask.cs
 
 Implementation of the cognitive task. It controls the display/hidden of the letter cue image based on the task progress.
 
 ### CueManager.cs
 
 Implementation of the cue manager. It controls the randomization of the letter cues and generates the list of cues to be used in a single task session.
 
 ### DataManager.cs
 
 Implementation of the data manager. It collects and saves all the study related data including the positions, times, and user responses.
