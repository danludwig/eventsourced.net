import { handleActions } from 'redux-actions'
import { REGISTER_SENT, REGISTER_DONE, VERIFY_SENT, VERIFY_DONE } from './actions'
import { INITIALIZE_DONE } from '../../../client/actions'

const register = {
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

export const uiRegister = handleActions(register, { })

const verify = {
  [VERIFY_SENT]: (state, action) =>
    Object.assign({}, state, {
      submitting: true,
      serverErrors: undefined
    }),
  [VERIFY_DONE]: {
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

export const uiVerify = handleActions(verify, { })

const redeem = {
  [INITIALIZE_DONE]: {
    next: (state, action) => Object.assign({}, state, {
      data: action.payload.state.ui
         && action.payload.state.ui.redeem
          ? action.payload.state.ui.redeem.data : undefined
    })
  }
}

export const uiRedeem = handleActions(redeem, { })
