import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { registerMessages as messages } from './validation'
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
  error.messages = messages
  return dispatch(createAction(REGISTER_DONE)(error))
}

function receiveRegister(dispatch, context, response, data) {
  dispatch(createAction(REGISTER_DONE)({
    receivedAt: Date.now()
  }))
  //const returnUrl = response.headers.get("location")
  //dispatch(pushPath(returnUrl))
}

export function submitVerify(dispatch, formInput) {
  return dispatch(createAction('VERIFY_SENT'))
}
