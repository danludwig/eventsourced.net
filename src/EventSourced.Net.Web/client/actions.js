import fetch from 'isomorphic-fetch'
import { submitToApi } from './forms/actions'
import { createAction } from 'redux-actions'

export const REDUX_INIT = '@@redux/INIT'
export const INITIALIZE_SENT = 'INITIALIZE_SENT'
export const INITIALIZE_DONE = 'INITIALIZE_DONE'

export function submitInitialize(formInput) {
  return submitToApi({
    method: 'GET',
    url: `${window.location.pathname}${window.location.search}`,
    send: sendInitialize,
    fail: failInitialize,
    done: receiveInitialize
  })
}

function sendInitialize(dispatch) {
  return dispatch(createAction(INITIALIZE_SENT)())
}

function failInitialize(dispatch) {
  const error = new TypeError('Request failed.')
  return dispatch(createAction(INITIALIZE_DONE)(error))
}

function receiveInitialize(dispatch, context, response, state) {
  return dispatch(createAction(INITIALIZE_DONE)({
    state,
    receivedAt: Date.now()
  }))
}
