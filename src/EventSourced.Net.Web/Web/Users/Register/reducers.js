import { SENT_REGISTER, FAILED_REGISTER, RECEIVED_REGISTER } from './actions'
import { createReducer } from '../../reducers'
import { convertServerErrors } from '../../forms/reducers'

const registerUI = createReducer({}, {
  [SENT_REGISTER](state, action) {
    const newState = {
      submitting: true
    }
    return newState
  },
  [FAILED_REGISTER](state, action) {
    const newState = {
      submitting: false,
      serverErrors: convertServerErrors(action)
    }
    return newState
  },
  [RECEIVED_REGISTER](state, action) {
    const newState = {
      submitting: false
    }
    return newState
  }
})

const reduceRegister = function(state, action) {
  const newState = Object.assign({}, state, {
    ui: {
      ...state.ui,
      register: registerUI(state.ui.register, action)
    },
  })
  return newState
}

const register = createReducer({}, {
  [SENT_REGISTER](state, action) {
    return reduceRegister(state, action)
  },
  [FAILED_REGISTER](state, action) {
    return reduceRegister(state, action)
  },
  [RECEIVED_REGISTER](state, action) {
    return reduceRegister(state, action)
  }
})

export default register
