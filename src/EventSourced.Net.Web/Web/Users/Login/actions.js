import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { messages } from './validation'
import { submitToApi } from '../../forms/actions'
import { createAction } from 'redux-actions'

export const LOGIN_SENT = 'LOGIN_SENT'
export const LOGIN_DONE = 'LOGIN_DONE'

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

function sendLogin(dispatch) {
  return dispatch(sentLogin())
}
function sentLogin() {
  const action = createAction(LOGIN_SENT)()
  return action
}

function failLogin(dispatch, context, response, data) {
  return dispatch(failedLogin(context.formInput, data))
}
function failedLogin(formInput, serverErrors) {
  const error = new TypeError('Request failed.')
  error.formInput = formInput
  error.serverErrors = serverErrors
  error.messages = messages
  const action = createAction(LOGIN_DONE)(error)
  return action
}

function receiveLogin(dispatch, context, response, data) {
  dispatch(receivedLogin(context.formInput, data.username))
  const returnUrl = response.headers.get("location")
  //dispatch(pushPath(returnUrl))
  return window.location = returnUrl
}
function receivedLogin(formInput, username) {
  const action = createAction(LOGIN_DONE)({
    username,
    receivedAt: Date.now()
  })
  return action
}
