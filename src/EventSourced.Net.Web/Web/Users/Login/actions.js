import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { messages } from './validation'
import { submitToApi } from '../../forms/actions'

export const SENT_LOGIN = 'SENT_LOGIN'
export const FAILED_LOGIN = 'FAILED_LOGIN'
export const RECEIVED_LOGIN = 'RECEIVED_LOGIN'

export function submitLogin(formInput) {
  return submitToApi({
    method: 'POST',
    url: '/login',
    formInput: formInput,
    send: sendLogin,
    fail: failLogin,
    done: receiveLogin
  })
}

function sendLogin(dispatch, context) {
  return dispatch(sentLogin(context.formInput))
}
function sentLogin(formInput) {
  return {
    type: SENT_LOGIN,
    formInput
  }
}

function failLogin(dispatch, context, response, data) {
  return dispatch(failedLogin(context.formInput, data))
}
function failedLogin(formInput, serverErrors) {
  return {
    type: FAILED_LOGIN,
    formInput,
    serverErrors,
    messages
  }
}

function receiveLogin(dispatch, context, response, data) {
  dispatch(receivedLogin(context.formInput, data.username))
  const returnUrl = response.headers.get("location")
  //dispatch(pushPath(returnUrl))
  return window.location = returnUrl
}
function receivedLogin(formInput, username) {
  return {
    type: RECEIVED_LOGIN,
    formInput,
    username,
    receivedAt: Date.now()
  }
}
