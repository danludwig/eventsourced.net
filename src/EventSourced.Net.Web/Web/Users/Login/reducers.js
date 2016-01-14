import { LOGIN_SENT, LOGIN_DONE } from './actions'

export const uiLogin = {
  [LOGIN_SENT]: (state, action) =>
    Object.assign({}, state, {
      submitting: true,
      serverErrors: undefined
    }),
  [LOGIN_DONE]: {
    throw: (state, action) => Object.assign({}, state, {
      submitting: false,
      serverErrors: action.payload.serverErrors,
      messages: action.payload.messages
    }),
    next: (state, action) => Object.assign({}, state, {
      submitting: false,
      serverErrors: undefined
    })
  }
}

export const dataUserLogin = {
  [LOGIN_DONE]: {
    next: (state, action) => Object.assign({}, state, {
      username: action.payload.username
    })
  }
}
