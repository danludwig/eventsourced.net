import { createStore, combineReducers } from 'redux'
import { Provider } from 'react-redux'
import { Router, Route, IndexRoute } from 'react-router'
import { createMemoryHistory } from 'history'
import { reducer as formReducer} from 'redux-form'
import App from '../../Web/Shared/App'

class ServerRenderedApp extends React.Component {
  static propTypes = {
    serverState: React.PropTypes.shape({
      location: React.PropTypes.string.isRequired,
      app: React.PropTypes.object.isRequired,
      routing: React.PropTypes.shape({
        location: React.PropTypes.shape({
          search: React.PropTypes.string,
          query: React.PropTypes.objectOf(React.PropTypes.string),
        }).isRequired,
      }).isRequired,
    }).isRequired
  };

  render() {
    const { location, app, routing } = this.props.serverState
    const store = createStore(combineReducers({
      app: (state = app) => state,
      routing: (state = routing) => state,
      form: formReducer
    }))
    const history = createMemoryHistory(location)
    return (
      <App store={store} history={history} />
		)
  }
}

module.exports = {
  App: ServerRenderedApp
}
