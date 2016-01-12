import { combineReducers } from 'redux'
import { reducer as formReducer} from 'redux-form'
import { routeReducer } from 'redux-simple-router'
import { handleActions } from 'redux-actions'
import { INITIALIZE_DONE } from './actions'
import login from './Users/Login/reducers'
import register from './Users/Register/reducers'

const defaultState = {
  server: {
    initialized: false,
    unavailable: false
  },
  ui: {
    login: {},
    register: {}
  },
  data: {
    user: {}
  }
}

const initialize = handleActions({
  INITIALIZE_DONE: {
    throw: (state, action) => Object.assign({}, state, {
      server: {
        ...state.server,
        unavailable: true
      }
    }),
    next: (state, action) => Object.assign({}, state, {
      server: action.payload.state.server,
      data: {
        ...state.data,
        user: action.payload.state.data.user
      }
    })
  }
}, defaultState)

const app = function(state = defaultState, action) {
  state = initialize(state, action)
  state = login(state, action)
  state = register(state, action)
  return state
}

export default combineReducers({
  app,
  form: formReducer,
  routing: routeReducer
})
