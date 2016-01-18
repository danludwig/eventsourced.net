import fetch from 'isomorphic-fetch'
import { routeActions } from 'redux-simple-router'
import { registerMessages, verifyMessages } from './validation'
import { submitToApi } from '../../../client/forms/actions'
import { createAction } from 'redux-actions'

export const REGISTER_SENT = 'REGISTER_SENT'
export const REGISTER_DONE = 'REGISTER_DONE'

export function submitRegister(formInput) {
  return submitToApi({
    method: 'POST',
    url: '/register',
    formInput: formInput,
    send: sendRegister,
    fail: failRegister,
    done: receiveRegister
  })
}

function sendRegister(dispatch) {
  return dispatch(createAction(REGISTER_SENT)())
}

function failRegister(dispatch, context, response, serverErrors) {
  const error = new TypeError('Request failed.')
  error.serverErrors = serverErrors
  error.messages = registerMessages
  return dispatch(createAction(REGISTER_DONE)(error))
}

function receiveRegister(dispatch, context, response, data) {
  dispatch(createAction(REGISTER_DONE)({
    receivedAt: Date.now()
  }))
  const returnUrl = response.headers.get("location")
  dispatch(routeActions.push(returnUrl))
}

export const VERIFY_SENT = 'REGISTER/VERIFY_SENT'
export const VERIFY_DONE = 'REGISTER/VERIFY_DONE'

export function submitVerify(correlationId, formInput) {
  return submitToApi({
    method: 'POST',
    url: `/register/${correlationId}`,
    formInput: formInput,
    send: sendVerify,
    fail: failVerify,
    done: receiveVerify
  })
}

function sendVerify(dispatch) {
  return dispatch(createAction(VERIFY_SENT)())
}

function failVerify(dispatch, context, response, serverErrors) {
  const error = new TypeError('Request failed.')
  error.serverErrors = serverErrors
  error.messages = verifyMessages
  return dispatch(createAction(VERIFY_DONE)(error))
}

function receiveVerify(dispatch, context, response, data) {
  dispatch(createAction(VERIFY_DONE)({
    receivedAt: Date.now()
  }))
  const returnUrl = response.headers.get("location")
  dispatch(routeActions.push(returnUrl))
}
