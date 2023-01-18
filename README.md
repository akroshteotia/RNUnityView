# RNUnityView

Integrate unity3d within a React Native app. Add a react native component to show unity. Works on both iOS and Android.

## Requirements

1. [Node.js](https://nodejs.org/en/)(lastest version)
2. [Unity 3D](https://unity.com/)

### Current Project Hierarchy

```
.
├── Unity Project(Demo)
├── Modified Library(React-native_unity_view)
├── React Naive Sample Project(RNView)
│   └── Android (Android project for React Native)
│   └── ios (iOS project for React Native)
│   └── node_modules
│   └── package.json
├── README.md
```

### Generating Projects

### React native

1. Create a folder for Project
2. Navigate to the project in terminal
3. Run command

    ```
    npx react-native init <project-name>
    npm i react-native-unity-view
    ```

### Unity Project

1. Start Unity Hub
2. Follow the wizard to create project.


## Copy files

### Unity

1. Copy Editor folder from Demo/Assets to your UnityProject/Assets.
2. Copy Newtonsoft.Json to your project.
3. Copy UnityMessageManager to your Project.

### Android Build

1. Copy files from React-native_unity_view/android to Assets/Plugins/Android (if directory is not present please create 1)
2. Delete folder node_modules/react-native-unity-view/android/src/main/java
3. Go to Unity and open build setting ctrl + shift + B.
4. Click on Player Setting and go to Publishing Setting.
5. Tick option Custom Main Gradle Template. (It will create a Gradle file in Assets/Plugins/Android)
6. Open mainTemplate and add line to dependencies
    ```
    implementation "com.facebook.react:react-native:+"
    ```
7. Tick option Export Project and press Build
8. Choose the path <yourreactnativeprojectname>/android/UnityExport
9. Go to <yourreactnativeprojectname>/android/UnityExport.
10. Copy files from launcher/src/main/res/values
    ```
    ids.xml
    string.xml
    ```
    to unityLibrary/src/main/res/values.
11. Once done with all step open <yourreactnativeprojectname>/android in Android Studio.
12. Got to app/build.gradle and line  to dependencies and remove line if any implements react-native-unity-view from the same
    ```
    implementation project(':unityLibrary')
    ```
13. Got to setting.gradle and remove react-native-unity-view project and unityLibrary (it will look like this)
    ```
    rootProject.name = 'RNView'
    include ':unityLibrary'
    project(':unityLibrary').projectDir = new File(rootProject.projectDir, './unity/unityLibrary')
    apply from: file("../node_modules/@react-native-community/cli-platform-android/native_modules.gradle"); applyNativeModulesSettingsGradle(settings)
    include ':app'
    ```
14. Try to build if there is no error you are good to go.

### iOS Build

1. Copy all files from React-native_unity_view except android folder to <yourreactnativeprojectname>/node_modules/react-native-unity-view folder
2. Go to unity in top bar and select ReactNative/ExportiOS or you can do this from build setting as well.
3. It will export iOS project to <yourreactnativeprojectname>/ios/UnityExport
4. open terminal and navigate to <yourreactnativeprojectname>/ios and run command
    ```
    pod install
    ```
5. Open <yourreactnativeprojectname.workspace> in xCode
6. Modify ####main.m
    ```
    #import "UnityUtils.h"

    int main(int argc, char * argv[]) {
      @autoreleasepool {
        InitArgs(argc, argv);
        return UIApplicationMain(argc, argv, nil, NSStringFromClass([AppDelegate class]));
      }
    ```
7. Open pod file and add these lines
    ```
    post_install do |installer|
     installer.pods_project.targets.each do |target|
         if target.name == 'RNUnityView'
           target.build_configurations.each do |config|
             config.build_settings['HEADER_SEARCH_PATHS'] ||= "$(inherited)"\
                                                              " $(PODS_ROOT)/../../node_modules/React/**"\
                                                              " $(PODS_ROOT)/../../node_modules/react-native/React/**"\
                                                              " $(PODS_ROOT)/../UnityExport/Classes"\
                                                              " $(PODS_ROOT)/../UnityExport"\
                                                              " $(PODS_ROOT)/../UnityExport/Classes/Unity"\
                                                              " $(PODS_ROOT)/../UnityExport/Classes/PluginBase"\
                                                              " $(PODS_ROOT)/../UnityExport/Libraries"\
                                                              " $(PODS_ROOT)/../UnityExport/Libraries/libil2cpp/include"\
                                                              " $(PODS_ROOT)/../UnityExport/Libraries/bdwgc/include"

     react_native_post_install(installer)
           end
       end
     end
    end
    ```
8. Go to Libraries folder under <yourreactnativeprojectname> and rigth click on it. Click on "AddFiles".
9. Navigate to UnityExport folder and add Unity-iPhone.xcodeproj
10. Under Unity-iPhone.xcodeproj got products there is UnityFramwork.framework.
11. Add UnityFramwork.framework to <yourreactnativeprojectname>/General/EmbeddedContent
12. Click run and you are good to go.
