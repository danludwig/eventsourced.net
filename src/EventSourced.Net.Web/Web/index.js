import React from 'react'
import { render } from 'react-dom'
import { compose, createStore, applyMiddleware } from 'redux'
import { Provider } from 'react-redux'
import thunkMiddleware from 'redux-thunk'
import createLogger  from 'redux-logger'
import webApiMiddleware from './Shared/webapi-middleware'
import { createHistory, useBasename } from 'history'
import { Router, Route, IndexRoute } from 'react-router'
import { syncHistory } from 'redux-simple-router'
import reducer from './Shared/reducers'
import { initialize } from './Shared/actions'
import App from './Shared/App'

const history = useBasename(createHistory)({
  basename: '/'
})
const reduxRouterMiddleware = syncHistory(history)

const loggerMiddleware = createLogger()
const createFinalStoreWithMiddleware = compose(
  applyMiddleware(
    webApiMiddleware,
    thunkMiddleware,
    loggerMiddleware,
    reduxRouterMiddleware
  ),
  window.devToolsExtension ? window.devToolsExtension() : f => f
)(createStore)

const store = createFinalStoreWithMiddleware(reducer)
reduxRouterMiddleware.listenForReplays(store)
store.dispatch(initialize())

render(
  <App store={store} history={history} />,
  document.getElementById('app')
)
