import fetch from 'isomorphic-fetch'
import { submitToApi } from './forms/actions'
import { createAction } from 'redux-actions'

export const INITIALIZE_SENT = 'INITIALIZE_SENT'
export const INITIALIZE_DONE = 'INITIALIZE_DONE'

export function submitInitialize(formInput) {
  return submitToApi({
    method: 'GET',
    url: '/',
    send: sendInitialize,
    fail: failInitialize,
    done: receiveInitialize
  })
}

function sendInitialize(dispatch) {
  return dispatch(sentInitialize())
}
function sentInitialize() {
  const action = createAction(INITIALIZE_SENT)()
  return action
}

function failInitialize(dispatch) {
  return dispatch(failedInitialize())
}
function failedInitialize() {
  const error = new TypeError('Request failed.')
  const action = createAction(INITIALIZE_DONE)(error)
  return action
}

function receiveInitialize(dispatch, context, response, data) {
  return dispatch(receivedInitialize(data))
}
function receivedInitialize(state) {
  const action = createAction(INITIALIZE_DONE)({
    state,
    receivedAt: Date.now()
  })
  return action
}
