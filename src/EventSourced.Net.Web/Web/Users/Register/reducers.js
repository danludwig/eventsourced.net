import { REGISTER_SENT, REGISTER_DONE } from './actions'
import { convertServerErrors } from '../../forms/reducers'
import { handleActions } from 'redux-actions'

const defaultState = {
  ui: { }
}

const register = handleActions({
  REGISTER_SENT: (state, action) =>
    Object.assign({}, state, {
      ui: {
        ...state.ui,
        register: {
          submitting: true
        }
      }
  }),
  REGISTER_DONE: {
    throw: (state, action) => Object.assign({}, state, {
      ui: {
        ...state.ui,
        register: {
          submitting: false,
          serverErrors: convertServerErrors(action.payload)
        }
      }
    }),
    next: (state = defaultState, action) => Object.assign({}, state, {
      data: {
        ...state.data
      }
    })
  }
}, defaultState)

export default register
