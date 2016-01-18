import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { messages } from './validation'
import { submitToApi } from '../../../client/forms/actions'
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
  return dispatch(createAction(LOGIN_SENT)())
}

function failLogin(dispatch, context, response, serverErrors) {
  const error = new TypeError('Request failed.')
  error.serverErrors = serverErrors
  error.messages = messages
  return dispatch(createAction(LOGIN_DONE)(error))
}

function receiveLogin(dispatch, context, response, data) {
  dispatch(createAction(LOGIN_DONE)({
    username: data.username,
    receivedAt: Date.now()
  }))
  const returnUrl = response.headers.get("location")
  //dispatch(pushPath(returnUrl))
  return window.location = returnUrl
}
