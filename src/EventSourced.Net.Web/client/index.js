import React from 'react'
import { render } from 'react-dom'
import { compose, createStore, applyMiddleware } from 'redux'
import { Provider } from 'react-redux';
import thunkMiddleware from 'redux-thunk'
import createLogger  from 'redux-logger'
import { Router, Route, IndexRoute } from 'react-router'
import { createHistory, useBasename } from 'history'
import { syncHistory } from 'redux-simple-router'
import reducer from './reducers'
import { submitInitialize } from './actions'
import App from './components/App';
import Home from './Home/Home'
import About from './Home/About'
import Contact  from './Home/Contact'
import Login  from './Users/Login/LoginForm'
import Register  from './Users/Register/RegisterForm'

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

const store = createFinalStoreWithMiddleware(reducer);
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
      </Route>
    </Router>
  </Provider>,
  document.getElementById('app')
)
