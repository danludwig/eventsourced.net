import { LOGIN_SENT, LOGIN_DONE } from './actions'
import { convertServerErrors } from '../../forms/reducers'
import { handleActions } from 'redux-actions'

const defaultState = {
  ui: { },
  data: {
    user: {}
  }
}

const login = handleActions({
  LOGIN_SENT: (state = defaultState, action) =>
    Object.assign({}, state, {
      ui: {
        ...state.ui,
        login: {
          submitting: true
        }
      }
  }),
  LOGIN_DONE: {
    throw: (state, action) => Object.assign({}, state, {
      ui: {
        ...state.ui,
        login: {
          submitting: false,
          serverErrors: convertServerErrors(action.payload)
        }
      }
    }),
    next: (state, action) => Object.assign({}, state, {
      data: {
        ...state.data,
        user: {
          username: action.payload.username
        }
      }
    })
  }
}, defaultState)

export default login
