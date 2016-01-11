import { LOG_IN, RECEIVE_LOG_IN_ERROR, RECEIVE_LOG_IN_RESPONSE } from './actions'
import { createReducer } from '../../reducers'

const loginUI = createReducer({}, {
  [LOG_IN](state, action) {
    const newState = {
      submitting: true
    }
    return newState
  },
  [RECEIVE_LOG_IN_ERROR](state, action) {
    const newState = {
      submitting: false,
      serverErrors: action.serverErrors
    }
    return newState
  },
  [RECEIVE_LOG_IN_RESPONSE](state, action) {
    const newState = {
      submitting: false
    }
    return newState
  }
})

const loginData = createReducer({}, {
  [RECEIVE_LOG_IN_RESPONSE](state, action) {
    const newState = Object.assign({}, state, {
      username: action.username
    })
    return newState
  }
})

const reduceLogin = function(state, action) {
  const newState = Object.assign({}, state, {
    ui: {
      ...state.ui,
      login: loginUI(state.ui.login, action)
    },
    data: {
      ...state.data,
      user: loginData(state.data.user, action)
    },
  })
  return newState
}

const login = createReducer({}, {
  [LOG_IN](state, action) {
    return reduceLogin(state, action)
  },
  [RECEIVE_LOG_IN_ERROR](state, action) {
    return reduceLogin(state, action)
  },
  [RECEIVE_LOG_IN_RESPONSE](state, action) {
    return reduceLogin(state, action)
  }
})

export default login
