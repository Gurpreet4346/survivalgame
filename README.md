# survivalgame
AudioManager.cs:The AudioManager script centralizes audio control using a singleton pattern to persist across scenes, managing background music (main menu and game) . It provides methods to toggle music on/off based on user settings, ensuring seamless audio transitions and global access throughout the project.
BackGroundMusicManager.cs:The BackgroundMusicManager script plays the appropriate background music for the current scene by checking if the active scene is the Main Menu or another scene and then using the AudioManager to play the corresponding music.
