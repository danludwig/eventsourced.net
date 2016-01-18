import { REGISTER_SENT, REGISTER_DONE, VERIFY_SENT, VERIFY_DONE } from './actions'

export const uiRegister = {
  [REGISTER_SENT]: (state, action) =>
    Object.assign({}, state, {
      submitting: true,
      serverErrors: undefined
    }),
  [REGISTER_DONE]: {
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

export const uiVerify = {
  [VERIFY_SENT]: (state, action) =>
    Object.assign({}, state, {
      verify: {
        submitting: true,
        serverErrors: undefined
      }
    }),
  [VERIFY_DONE]: {
    throw: (state, action) => Object.assign({}, state, {
      verify: {
        submitting: false,
        serverErrors: action.payload.serverErrors,
        messages: action.payload.messages
      }
    }),
    next: (state, action) => Object.assign({}, state, {
      verify: {
        submitting: false,
        serverErrors: undefined
      }
    })
  }
}
