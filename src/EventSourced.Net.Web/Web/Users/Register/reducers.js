import { handleActions } from 'redux-actions'
import { REGISTER_SENT, REGISTER_DONE, VERIFY_SENT, VERIFY_DONE,
         CREATE_LOGIN_SENT, CREATE_LOGIN_DONE, CREATE_LOGIN_REVERSED } from './actions'
import { INITIALIZE_STATE } from '../../Shared/actions'

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
      messages: action.payload.messages,
    }),
    next: (state, action) => Object.assign({}, state, {
      submitting: false,
      serverErrors: undefined
    })
  }
}

export const uiVerify = handleActions(verify, { })

const redeem = {
  [INITIALIZE_STATE]: (state, action) =>
    Object.assign({}, state, {
      data: action.payload.app.ui
         && action.payload.app.ui.redeem
          ? action.payload.app.ui.redeem.data : undefined,
    }),
  [VERIFY_DONE]: {
    next: (state, action) => Object.assign({}, state, {
      submitting: false,
      data: action.payload.data,
      serverErrors: undefined,
    })
  },
  [CREATE_LOGIN_SENT]: (state, action) =>
    Object.assign({}, state, {
      submitting: true,
    }),
  [CREATE_LOGIN_DONE]: {
    throw: (state, action) => Object.assign({}, state, {
      submitting: false,
      serverErrors: action.payload.serverErrors,
      messages: action.payload.messages,
    }),
    next: (state, action) => Object.assign({}, state, {
      serverErrors: undefined,
    })
  },
  [CREATE_LOGIN_REVERSED]: (state, action) =>
    Object.assign({}, state, {
      submitting: false,
      serverErrors: action.payload.serverErrors,
      messages: action.payload.messages,
    }),
}

export const uiRedeem = handleActions(redeem, { })
