import { SENT_LOGIN, FAILED_LOGIN, RECEIVED_LOGIN } from './actions'
import { createReducer } from '../../reducers'
import { convertServerErrors } from '../../forms/reducers'

const loginUI = createReducer({}, {
  [SENT_LOGIN](state, action) {
    const newState = {
      submitting: true
    }
    return newState
  },
  [FAILED_LOGIN](state, action) {
    const newState = {
      submitting: false,
      serverErrors: convertServerErrors(action)
    }
    return newState
  },
  [RECEIVED_LOGIN](state, action) {
    const newState = {
      submitting: false
    }
    return newState
  }
})

const loginData = createReducer({}, {
  [RECEIVED_LOGIN](state, action) {
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
  [SENT_LOGIN](state, action) {
    return reduceLogin(state, action)
  },
  [FAILED_LOGIN](state, action) {
    return reduceLogin(state, action)
  },
  [RECEIVED_LOGIN](state, action) {
    return reduceLogin(state, action)
  }
})

export default login
