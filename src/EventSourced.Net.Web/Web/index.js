import { compose, createStore, applyMiddleware } from 'redux'
import thunkMiddleware from 'redux-thunk'
import createLogger  from 'redux-logger'
import { apiMiddleware } from 'redux-api-middleware'
import { createHistory, useBasename } from 'history'
import { syncHistory } from 'redux-simple-router'
import reducer from './reducer'
import initializeState from './Shared/actions/initializeState'
import App from './Shared/App'

const history = useBasename(createHistory)({
  basename: '/'
})
const reduxRouterMiddleware = syncHistory(history)

const loggerMiddleware = createLogger()
const createFinalStoreWithMiddleware = compose(
  applyMiddleware(
    thunkMiddleware,
    apiMiddleware,
    //loggerMiddleware,
    reduxRouterMiddleware
  ),
  window.devToolsExtension ? window.devToolsExtension() : f => f
)(createStore)

const store = createFinalStoreWithMiddleware(reducer)
reduxRouterMiddleware.listenForReplays(store)
store.dispatch(initializeState())

ReactDOM.render(
  <App store={store} history={history} />,
  document.getElementById('app')
)
