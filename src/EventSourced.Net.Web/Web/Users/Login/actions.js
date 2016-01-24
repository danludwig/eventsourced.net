import { routeActions } from 'redux-simple-router'
import { messages } from './validation'
import { createAction } from 'redux-actions'
import { SEND_WEBAPI } from '../../Shared/actions'

export const LOGIN_SENT = 'LOGIN_SENT'
export const LOGIN_DONE = 'LOGIN_DONE'

export function submitLogin(formInput, returnUrl) {
  const search = returnUrl ? `?returnUrl=${returnUrl}` : ''
  return createAction(SEND_WEBAPI)({
    method: 'POST',
    url: `/login${search}`,
    formInput: formInput,
    send: sendLogin,
    fail: failLogin,
    done: receiveLogin
  })
}

function sendLogin() {
  return createAction(LOGIN_SENT)()
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
  dispatch(routeActions.push(returnUrl))
  //return window.location = returnUrl
}
