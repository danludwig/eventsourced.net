import fetch from 'isomorphic-fetch'
import { submitToApi } from './forms/actions'

export const SENT_INITIALIZE = 'SENT_INITIALIZE'
export const FAILED_INITIALIZE = 'FAILED_INITIALIZE'
export const RECEIVED_INITIALIZE = 'RECEIVED_INITIALIZE'

export function submitInitialize(formInput) {
  return submitToApi({
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
  return {
    type: SENT_INITIALIZE
  }
}

function failInitialize(dispatch) {
  return dispatch(failedInitialize())
}
function failedInitialize() {
  return {
    type: FAILED_INITIALIZE
  }
}

function receiveInitialize(dispatch, context, response, data) {
  return dispatch(receivedInitialize(data))
}
function receivedInitialize(state) {
  return {
    type: RECEIVED_INITIALIZE,
    state
  }
}
