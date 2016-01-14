import { REGISTER_SENT, REGISTER_DONE } from './actions'

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
