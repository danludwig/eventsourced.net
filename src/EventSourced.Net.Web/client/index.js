import React from 'react'
import { render } from 'react-dom'
import { createStore, applyMiddleware } from 'redux'
import { Provider } from 'react-redux';
import thunkMiddleware from 'redux-thunk'
import createLogger  from 'redux-logger'
import { Router, Route, IndexRoute } from 'react-router'
import { createHistory, useBasename } from 'history'
import { syncReduxAndRouter } from 'redux-simple-router'
import reducer from './reducers'
import { requestInitialState } from './actions'
import App from './components/App';
import Home from './Home/Home'
import About from './Home/About'
import Contact  from './Home/Contact'
import Login  from './Users/Login/LoginForm'
import Register  from './Users/Register/RegisterForm'

const loggerMiddleware = createLogger()
const createStoreWithMiddleware = applyMiddleware(
  thunkMiddleware,
  loggerMiddleware
)(createStore)

const store = createStoreWithMiddleware(reducer);
store.dispatch(requestInitialState())
const history = useBasename(createHistory)({
  basename: '/'
})

syncReduxAndRouter(history, store)

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
