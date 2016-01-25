import { routeActions } from 'redux-simple-router'
import { registerMessages, verifyMessages, redeemMessages } from './validation'
import { createAction } from 'redux-actions'
import { SEND_WEBAPI } from '../../Shared/actions'

export const REGISTER_SENT = 'REGISTER_SENT'
export const REGISTER_DONE = 'REGISTER_DONE'

export function submitRegister(formInput) {
  return createAction(SEND_WEBAPI)({
    method: 'POST',
    url: '/register',
    formInput: formInput,
    send: sendRegister,
    fail: failRegister,
    done: receiveRegister
  })
}

function sendRegister(dispatch) {
  return createAction(REGISTER_SENT)()
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
  return createAction(SEND_WEBAPI)({
    method: 'POST',
    url: `/register/${correlationId}`,
    formInput: formInput,
    send: sendVerify,
    fail: failVerify,
    done: receiveVerify
  })
}

function sendVerify() {
  return createAction(VERIFY_SENT)()
}

function failVerify(dispatch, context, response, serverErrors) {
  const error = new TypeError('Request failed.')
  error.serverErrors = serverErrors
  error.messages = verifyMessages
  return dispatch(createAction(VERIFY_DONE)(error))
}

function receiveVerify(dispatch, context, response, data) {
  dispatch(createAction(VERIFY_DONE)({
    data,
    receivedAt: Date.now()
  }))
  const returnUrl = response.headers.get("location")
  dispatch(routeActions.push(returnUrl))
}

export const CREATE_LOGIN_SENT = 'REGISTER/CREATE_LOGIN_SENT'
export const CREATE_LOGIN_DONE = 'REGISTER/CREATE_LOGIN_DONE'
export const CREATE_LOGIN_CONCLUDED = 'REGISTER/CREATE_LOGIN_CONCLUDED'
export const CREATE_LOGIN_REVERSED = 'REGISTER/CREATE_LOGIN_REVERSED'

export function submitCreateLogin(correlationId, token, formInput) {
  return createAction(SEND_WEBAPI)({
    method: 'POST',
    url: `/register/${correlationId}/redeem?token=${encodeURIComponent(token)}`,
    formInput: formInput,
    send: sendCreateLogin,
    fail: failCreateLogin,
    done: receiveCreateLogin,
  })
}

function sendCreateLogin() {
  return createAction(CREATE_LOGIN_SENT)()
}

function failCreateLogin(dispatch, context, response, serverErrors) {
  const error = new TypeError('Request failed.')
  error.serverErrors = serverErrors
  error.messages = redeemMessages
  return dispatch(createAction(CREATE_LOGIN_DONE)(error))
}

function receiveCreateLogin(dispatch, context, response, data) {
  const returnUrl = response.headers.get("location")
  const socketUrl = response.headers.get('x-correlation-socket')
  const socket = new WebSocket(socketUrl)
  let isConstraintViolated = false
  socket.onmessage = socketMessage => {
    if (isConstraintViolated) return
    var messageData = { type: 'unknown' }
    try { messageData = JSON.parse(socketMessage.data) } catch (ex) { }
    if (messageData.isComplete) {
      dispatch(createAction(CREATE_LOGIN_CONCLUDED)())
      return dispatch(routeActions.push(returnUrl))
    }

    if (messageData.isComplete === false) {
      isConstraintViolated = true
      // parse out server errors
      let serverErrors = { }
      if (messageData.duplicateUsername) {
        serverErrors.username = [{
          reason: 'alreadyExists',
        }]
      }
      if (messageData.duplicateContact) {
        serverErrors.emailOrPhone = [{
          reason: 'alreadyExists',
          data: {
            emailOrPhone: messageData.duplicateContact
          }
        }]
      }
      return dispatch(createAction(CREATE_LOGIN_REVERSED)({
        serverErrors,
        messages: redeemMessages
      }))
    }
  }
  return dispatch(createAction(CREATE_LOGIN_DONE)({
    data,
    receivedAt: Date.now()
  }))
  //const returnUrl = response.headers.get("location")
  //dispatch(routeActions.push(returnUrl))
}
