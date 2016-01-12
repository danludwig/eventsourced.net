import { combineReducers } from 'redux'
import { reducer as formReducer} from 'redux-form'
import { routeReducer } from 'redux-simple-router'
import { RECEIVED_INITIALIZE, FAILED_INITIALIZE } from './actions'
import { SENT_LOGIN, FAILED_LOGIN, RECEIVED_LOGIN } from './Users/Login/actions'
import login from './Users/Login/reducers'
import { SENT_REGISTER, FAILED_REGISTER, RECEIVED_REGISTER } from './Users/Register/actions'
import register from './Users/Register/reducers'

const initialState = {
  server: {
    initialized: false,
    unavailable: false
  },
  ui: {
    register: {},
    login: {}
  },
  data: {
    user: {
      username: undefined
    }
  }
}

export function createReducer(initialState, handlers) {
  return function reducer(state = initialState, action) {
    return handlers.hasOwnProperty(action.type)
      ? handlers[action.type](state, action)
      : state
  }
}

function app(state = initialState, action) {
  // app reducer
  let newState
  switch (action.type) {
    case RECEIVED_INITIALIZE:
      newState = Object.assign({}, state, action.state)
      return newState
    case FAILED_INITIALIZE:
      newState = Object.assign({}, state, {
        ...state,
        server: {
          ...state.server,
          unavailable: true
        }
      })
      return newState
    case SENT_LOGIN:
    case FAILED_LOGIN:
    case RECEIVED_LOGIN:
      newState = login(state, action)
      return newState
    case SENT_REGISTER:
    case FAILED_REGISTER:
    case RECEIVED_REGISTER:
      newState = register(state, action)
      return newState
    default:
      return state
  }
  return state
}

export default combineReducers({
  app,
  form: formReducer,
  routing: routeReducer
})
