import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'

export const LOG_IN = 'LOG_IN'
function logIn(formData) {
  return {
    type: LOG_IN,
    formData
  }
}

export const RECEIVE_LOG_IN_RESPONSE = 'RECEIVE_LOG_IN_RESPONSE'
function receiveLogInResponse(formData, username) {
  return {
    type: RECEIVE_LOG_IN_RESPONSE,
    formData,
    username,
    receivedAt: Date.now()
  }
}

export const RECEIVE_LOG_IN_ERROR = 'RECEIVE_LOG_IN_ERROR'
function receiveLogInError(formData, serverErrors) {
  return {
    type: RECEIVE_LOG_IN_ERROR,
    formData,
    serverErrors
  }
}

export function submitLogin(formData) {
  // submitLogin action
  return dispatch => {
    dispatch(logIn(formData))

    return fetch(`/api/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'X-Requested-With': 'isomorphic-fetch'
      },
      credentials: 'same-origin',
      body: JSON.stringify(formData)
    })
      .then(response => {
        if (!response.ok) {
          return response.json().then(json => {
            dispatch(receiveLogInError(formData, json))
          })
        }
        const returnUrl = response.headers.get("location")
        return response.json().then(json => {
          dispatch(receiveLogInResponse(formData, json.username))
          //dispatch(pushPath(returnUrl))
          window.location = returnUrl
        })
      })
  }
}
