import React, { Component } from 'react'
import { createStore, combineReducers } from 'redux'
import { Provider } from 'react-redux'
import { Router, Route, IndexRoute } from 'react-router'
import { createMemoryHistory } from 'history'
import { reducer as formReducer} from 'redux-form'
import App from '../../Web/Shared/App'

class ServerRenderedApp extends Component {
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
