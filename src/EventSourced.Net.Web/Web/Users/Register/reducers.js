import { REGISTER_SENT, REGISTER_DONE } from './actions'
import { convertServerErrors } from '../../forms/reducers'

export const uiRegister = {
  [REGISTER_SENT]: (state, action) =>
    Object.assign({}, state, {
      submitting: true,
      serverErrors: undefined
    }),
  [REGISTER_DONE]: {
    throw: (state, action) => Object.assign({}, state, {
      submitting: false,
      serverErrors: convertServerErrors(action.payload)
    }),
    next: (state, action) => Object.assign({}, state, {
      submitting: false,
      serverErrors: undefined
    })
  }
}
