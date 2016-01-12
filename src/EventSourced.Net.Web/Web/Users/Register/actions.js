import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { messages } from './validation'
import { submitToApi } from '../../forms/actions'
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

function sendRegister(dispatch, context) {
  return dispatch(sentRegister(context.formInput))
}
function sentRegister(formInput) {
  const action = createAction(REGISTER_SENT)()
  return action
}

function failRegister(dispatch, context, response, data) {
  return dispatch(failedRegister(context.formInput, data))
}
function failedRegister(formInput, serverErrors) {
  const error = new TypeError('Request failed.')
  error.formInput = formInput
  error.serverErrors = serverErrors
  error.messages = messages
  const action = createAction(REGISTER_DONE)(error)
  return action
}

function receiveRegister(dispatch, context, response, data) {
  dispatch(receivedRegister())
  //const returnUrl = response.headers.get("location")
  //dispatch(pushPath(returnUrl))
}
function receivedRegister() {
  const action = createAction(REGISTER_DONE)({
    receivedAt: Date.now()
  })
  return action
}
