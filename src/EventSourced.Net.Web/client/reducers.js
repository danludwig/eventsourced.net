import { combineReducers } from 'redux'
import { reducer as formReducer} from 'redux-form'
import { routeReducer } from 'redux-simple-router'
import { RECEIVE_INITIAL_STATE, FAIL_INITIAL_STATE } from './actions'
import { LOG_IN, RECEIVE_LOG_IN_ERROR, RECEIVE_LOG_IN_RESPONSE } from './Users/Login/actions'
import login from './Users/Login/reducers'

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
    case LOG_IN:
    case RECEIVE_LOG_IN_ERROR:
    case RECEIVE_LOG_IN_RESPONSE:
      newState = login(state, action)
      return newState
    case RECEIVE_INITIAL_STATE:
      newState = Object.assign({}, state, action.state)
      return newState
    case FAIL_INITIAL_STATE:
      newState = Object.assign({}, state, {
        ...state,
        server: {
          ...state.server,
          unavailable: true
        }
      })
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
