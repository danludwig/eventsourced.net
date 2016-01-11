import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { messages } from './validation'

export const SENT_LOGIN = 'SENT_LOGIN'
export const FAILED_LOGIN = 'FAILED_LOGIN'
export const RECEIVED_LOGIN = 'RECEIVED_LOGIN'

export function submitLogin(formInput) {
  // submitLogin action
  return dispatch => {
    dispatch(sentLogin(formInput))

    return fetch(`/api/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'X-Requested-With': 'isomorphic-fetch'
      },
      credentials: 'same-origin',
      body: JSON.stringify(formInput)
    })
      .then(response => {
        if (!response.ok) {
          return response.json().then(json => {
            dispatch(failedLogin(formInput, json))
          })
        }
        const returnUrl = response.headers.get("location")
        return response.json().then(json => {
          dispatch(receivedLogin(formInput, json.username))
          //dispatch(pushPath(returnUrl))
          window.location = returnUrl
        })
      })
  }
}

function sentLogin(formInput) {
  return {
    type: SENT_LOGIN,
    formInput
  }
}

function failedLogin(formInput, serverErrors) {
  return {
    type: FAILED_LOGIN,
    formInput,
    serverErrors,
    messages
  }
}

function receivedLogin(formInput, username) {
  return {
    type: RECEIVED_LOGIN,
    formInput,
    username,
    receivedAt: Date.now()
  }
}
