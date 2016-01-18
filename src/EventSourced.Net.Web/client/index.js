import React from 'react'
import { render } from 'react-dom'
import { compose, createStore, applyMiddleware } from 'redux'
import { Provider } from 'react-redux'
import thunkMiddleware from 'redux-thunk'
import createLogger  from 'redux-logger'
import { createHistory, useBasename } from 'history'
import { Router, Route, IndexRoute } from 'react-router'
import { syncHistory } from 'redux-simple-router'
import reducer from './reducers'
import { submitInitialize } from './actions'
import App from './components/App'
import Home from '../Web/Home/Home'
import About from '../Web/Home/About'
import Contact  from '../Web/Home/Contact'
import Login  from '../Web/Users/Login/LoginForm'
import Register  from '../Web/Users/Register/RegisterForm'
import Verify  from '../Web/Users/Register/VerifyForm'

const history = useBasename(createHistory)({
  basename: '/'
})
const loggerMiddleware = createLogger()
const reduxRouterMiddleware = syncHistory(history)
const createFinalStoreWithMiddleware = compose(
  applyMiddleware(
    reduxRouterMiddleware,
    thunkMiddleware,
    loggerMiddleware
  ),
  window.devToolsExtension ? window.devToolsExtension() : f => f
)(createStore)

const store = createFinalStoreWithMiddleware(reducer)
reduxRouterMiddleware.listenForReplays(store)
store.dispatch(submitInitialize())

render(
  <Provider store={store}>
    <Router history={history}>
      <Route path="/" component={App}>
        <IndexRoute component={Home} />
        <Route path="about" component={About} />
        <Route path="contact" component={Contact} />
        <Route path="login" component={Login} />
        <Route path="register" component={Register} />
        <Route path="verify" component={Verify} />
      </Route>
    </Router>
  </Provider>,
  document.getElementById('app')
)
