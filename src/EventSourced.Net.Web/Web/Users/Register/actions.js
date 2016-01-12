import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { messages } from './validation'
import { submitToApi } from '../../forms/actions'

export const SENT_REGISTER = 'SENT_REGISTER'
export const FAILED_REGISTER = 'FAILED_REGISTER'
export const RECEIVED_REGISTER = 'RECEIVED_REGISTER'

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
  return {
    type: SENT_REGISTER,
    formInput
  }
}

function failRegister(dispatch, context, response, data) {
  return dispatch(failedRegister(context.formInput, data))
}
function failedRegister(formInput, serverErrors) {
  return {
    type: FAILED_REGISTER,
    formInput,
    serverErrors,
    messages
  }
}

function receiveRegister(dispatch, context, response, data) {
  dispatch(receivedRegister(context.formInput))
  //const returnUrl = response.headers.get("location")
  //dispatch(pushPath(returnUrl))
}
function receivedRegister(formInput) {
  return {
    type: RECEIVED_REGISTER,
    formInput,
    receivedAt: Date.now()
  }
}
