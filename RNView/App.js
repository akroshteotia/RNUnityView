/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 * @flow strict-local
 */

 import * as React from 'react';
 import {
   Platform,
   StyleSheet,
   Text,
   View,
   StatusBar,
   NativeSyntheticEvent,
   Alert
 } from 'react-native';

 import Button from './components/Button';
 import UnityView, { MessageHandler, UnityModule } from 'react-native-unity-view';

 type Props = {};

 type State = {
   startTimer: number;
   fps: number;
   renderUnity: boolean;
   unityPaused: boolean;
 };

class App extends React.Component<Props, State>{

  constructor(props) {
    super(props);
    this.state = {
      startTimer: 0,
      fps: 0,
      renderUnity: false,
      unityPaused: false,
    }
  }

  componentDidMount() {
    StatusBar.setHidden(false);
    StatusBar.setBarStyle('dark-content');
    if (Platform.OS == 'android') {
      StatusBar.setBackgroundColor('rgba(255,255,255,0)');
      StatusBar.setTranslucent(true);
    }
  }

  onToggleUnity() {
    this.setState({ renderUnity: !this.state.renderUnity });
  }

  onPauseAndResumeUnity() {
    if (this.state.unityPaused) {
      UnityModule.resume();
    } else {
      UnityModule.pause();
    }
    this.setState({ unityPaused: !this.state.unityPaused });
  }

  onToggleRotate() {
    UnityModule.postMessageToUnityManager({
      name: 'ToggleRotate',
      data: '',
      callBack: (data) => {
        Alert.alert('Tip', JSON.stringify(data))
      }
    });
  }

  onUnityMessage(hander: MessageHandler) {
    if(hander.name == 'Start')
      this.setState({ startTimer: hander.data });
    if(hander.name == 'FPS')
        this.setState({ fps: hander.data });
  }

  render() {
    const { renderUnity, unityPaused, startTimer, fps } = this.state;
    let unityElement: JSX.Element;

    if (renderUnity) {
      unityElement = (
        <UnityView
          style={{ position: 'absolute', left: 0, right: 0, top: 0, bottom: 0 }}
          onUnityMessage={this.onUnityMessage.bind(this)}
        >
        </UnityView>
      );
    }

    return(
      <View style={[styles.container]}>
        {renderUnity ? unityElement : null}
        <Text style={styles.welcome, {color: renderUnity ? 'white' : 'black' }}>
          Welcome to React Native Unity View!
        </Text>
        <Text style={{ color: renderUnity ? 'white' : 'black', fontSize: 15 }}>Current View: <Text style={{ color: 'red' }}>{renderUnity ? "Unity View" : "React Native View"}</Text> </Text>
        {renderUnity ? <Text style={{ color: renderUnity ? 'white' : 'black', fontSize: 15 }}>FPS: <Text style={{ color: 'red' }}>{fps}</Text> </Text> : null}
        {renderUnity ? <Text style={{ color: renderUnity ? 'white' : 'black', fontSize: 15 }}>Load Time: <Text style={{ color: 'red' }}>{startTimer}</Text> </Text> : null}

        <Button label="Toggle Unity" style={styles.button} onPress={this.onToggleUnity.bind(this)} />
      </View>
    );
  }
}

const styles = StyleSheet.create( {
  container: {
    // flex: 1,
    position: 'absolute', top: 0, bottom: 0, left: 0, right: 0,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
  },
  welcome: {
    fontSize: 20,
    textAlign: 'center',
    margin: 10,
  },
  button: {
    marginTop: 10
  }
});

export default App;
