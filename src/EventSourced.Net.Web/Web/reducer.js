import { combineReducers } from 'redux'
import { reducer as formReducer} from 'redux-form'
import { routeReducer } from 'redux-simple-router'
import login from './Login/reducers'
import logoff from './Logoff/reducers'
import register from './Register/reducers'

export default combineReducers({
  app: combineReducers({
    login,
    logoff,
    register,
  }),
  form: formReducer,
  routing: routeReducer
})
