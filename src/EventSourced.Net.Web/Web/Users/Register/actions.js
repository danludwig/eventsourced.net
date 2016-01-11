import fetch from 'isomorphic-fetch'
import { pushPath } from 'redux-simple-router'
import { messages } from './validation'

export const SENT_REGISTER = 'SENT_REGISTER'
export const FAILED_REGISTER = 'FAILED_REGISTER'
export const RECEIVED_REGISTER = 'RECEIVED_REGISTER'

export function submitRegister(formInput) {
  // submitRegister action
  return dispatch => {
    dispatch(sentRegister(formInput))

    return fetch(`/api/register`, {
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
            dispatch(failedRegister(formInput, json))
          })
        }
        const returnUrl = response.headers.get("location")
        return response.json().then(json => {
          dispatch(receivedRegister(formInput))
          dispatch(pushPath(returnUrl))
          //window.location = returnUrl
        })
      })
  }
}

function sentRegister(formInput) {
  return {
    type: SENT_REGISTER,
    formInput
  }
}

function failedRegister(formInput, serverErrors) {
  return {
    type: FAILED_REGISTER,
    formInput,
    serverErrors,
    messages
  }
}

function receivedRegister(formInput) {
  return {
    type: RECEIVED_REGISTER,
    formInput,
    receivedAt: Date.now()
  }
}
